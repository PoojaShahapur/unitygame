--[[玩家进入,添加到排行榜的尾部.同时给自己发排行榜上全部数据,要是自己是排行榜前n名玩家,要通知所有玩家排行榜改变
玩家离开,table.remove(tbl, player.my_rank),更新并通知所有名次比自己低的玩家排行榜数据.
玩家得分增加,若为第1名,则排名不变,只单独通知客户端积分改变;否则,从我的下标n开始, 依次访问tbl的下标小于自己的元素n-1,m,一直找到自己积分<=积分的下标,最后先将自己移除,再插在下标为m+1的位置.同时遍历m+1,n,将player里面的名次修正过来.
得分减少,若为最后一名,则排名不变;否则,从下标n开始,一次访问下标大于自己的元素 n+1,m,一直找到积分<=自己的积分下标.再移除自己,插在下标为m-1的位置,同时遍历n,m-1,修正名次.
--]]

local config = require("config")
local excelm = require("excelm")
local mdb = require("database")
local log = require("log"):new("plane.rankmgr")
local pb = require("protobuf")
local Rankmgr = {
}

local rr = require("remote_run.remote_runner")
local table_insert = table.insert
local table_remove = table.remove

function Rankmgr:new(room)
    local mgr = {
        playerdata = {},
        my_room = room,
    }

    setmetatable(mgr, self)
    self.__index = self
    return mgr
end

function Rankmgr:full_rank_data_msg()
    local rank_data_msg = {
        my_rank = 0,
        my_score = 0,
        data = {},
    }
    -- 填充下排行榜的数据
    local iter_max = #self.playerdata < config.rank_show_num and #self.playerdata or config.rank_show_num
    for i = 1,iter_max do
        local onedata = {
            playerid = self.playerdata[i].id,
            playername = self.playerdata[i].nickname,
        }
        table_insert(rank_data_msg.data, onedata)
    end
    return rank_data_msg
end

function Rankmgr:on_player_enter(player)
    table_insert(self.playerdata, player)
    player.rank = #self.playerdata
    self:print_all_player()
    local rank_data_msg = self:full_rank_data_msg()
    -- 若我是前N名玩家,我的加入改变了排行榜的布局,通知下所有玩家
    if player.rank <= config.rank_show_num then
        for k,v in pairs(self.playerdata) do
            --log:debug('send rank to %d,%s', v.id, v.nickname)
            self:send_rank_data_to_player(v, rank_data_msg)
        end
    else
        -- 通知我自己就行了
        self:send_rank_data_to_player(player, rank_data_msg)
    end
end

function Rankmgr:send_rank_data_onreq(player)
    local rank_data_msg = self:full_rank_data_msg()
    self:send_rank_data_to_player(player, rank_data_msg)
end

function Rankmgr:send_rank_data_to_player(player, rank_data_msg)
    rank_data_msg.my_rank = player.rank
    rank_data_msg.my_score = player.score
    player:rpc_request("plane.PlanePush", "NotifyRankData", pb.encode("plane.RankDataMsg", rank_data_msg))
end

function Rankmgr:on_player_exit(player)
    for i = player.rank+1, #self.playerdata do
        self.playerdata[i].rank = self.playerdata[i].rank - 1
    end
    table.remove(self.playerdata, player.rank)
    self:print_all_player()
    local rank_data_msg = self:full_rank_data_msg()
    for k,v in pairs(self.playerdata) do
        self:send_rank_data_to_player(v, rank_data_msg)
    end
end

function Rankmgr:on_player_add_score(player)
    --log:debug("player_add_score,%d,%s, old_rank=%d", player.id,player.nickname, player.rank)
    local player_old_rank = player.rank
    if player.rank == 1 or (player.score <= self.playerdata[player.rank - 1].score) then -- 只通知我积分变了即可
        local score_msg = {my_score = player.score}
        player:rpc_request("plane.PlanePush", "NotifyMyScore", pb.encode("plane.ScoreMsg", score_msg))
    else 
        local final_pos = player.rank - 1
        for i = player.rank-1, 1,-1 do
            local iplayer = self.playerdata[i]
            if player.score <= iplayer.score then
                break
            end
            iplayer.rank = iplayer.rank + 1
            final_pos = i-1
        end
        table_remove(self.playerdata, player.rank)
        table_insert(self.playerdata, final_pos + 1, player)
        player.rank = final_pos + 1
        local rank_data_msg = self:full_rank_data_msg()
        for k,v in pairs(self.playerdata) do
            self:send_rank_data_to_player(v, rank_data_msg)
        end
    end
    self:print_all_player()
end

function Rankmgr:print_all_player()
    return
    --for k,v in pairs(self.playerdata) do
    --    log:debug("key=%d, playerid=%d,%s,rank=%d,score=%d", k, v.id,v.nickname, v.rank, v.score)
    --end
end

function Rankmgr:on_player_minus_score(player)
    --log:debug("player_minus_score,%d,%s, old_rank=%d", player.id, player.nickname, player.rank)
    local old_rank = player.rank
    if player.rank == #self.playerdata or (player.score >= self.playerdata[player.rank + 1].score) then--如果我是最后一名,或者减少后的得分仍大于我下面一个玩家
        local score_msg = {my_score = player.score}
        player:rpc_request("plane.PlanePush", "NotifyMyScore", pb.encode("plane.ScoreMsg", score_msg))
    else
        local final_pos = player.rank + 1
        for i = final_pos, #self.playerdata do
            if player.score >= self.playerdata[i].score then
                break
            end
            final_pos = i + 1
        end
        table_remove(self.playerdata, player.rank)
        table_insert(self.playerdata, final_pos - 1, player)
        player.rank = final_pos - 1
        for i = old_rank, final_pos -2 do
            self.playerdata[i].rank = self.playerdata[i].rank - 1
        end
        local rank_data_msg = self:full_rank_data_msg()
        for k,v in pairs(self.playerdata) do
            self:send_rank_data_to_player(v, rank_data_msg)
        end
    end
    self:print_all_player()
end

function Rankmgr:send_result_data()
    local result_msg = {
        datas = {},
    }

    local rewardtbl = nil
    local roomstr = '普通模式'
    local roommode = 0
    if self.my_room.type == "PlaneCrazyRoom" then
        rewardtbl = config.purgatory_reward
        roomstr = '炼狱模式'
        roommode = 1
    else
        rewardtbl = config.rankreward
    end
    local rank = 1
    for k,v in pairs(self.playerdata) do
        local reward = rewardtbl[rank]
        local sugar_reward = (reward == nil and 0 or reward.sugar)
        local cookie_reward = (reward == nil and 0 or reward.cookie)
        local onedata = {
            playerid = v.id,
            nickname = v.nickname,
            username = v.account,
            killnum = v.destroynum,
            score = v.score,
            reward_sugar = sugar_reward,
            reward_cookie = cookie_reward,
            is_ai = v.game_clt_id == 0 and 1 or 0,
            uid = v.game_clt_id == 0 and rank or v.uid,
        }
        local need_add_champion = false
        local unlock_pur = 0
        if rank ==1 then
            need_add_champion = true
            unlock_pur = 1
        end
        table_insert(result_msg.datas, onedata)
        if v.game_clt_id ~= 0 then
            log:info("%s本局结束,给%s,%s发 money=%d,sugar=%d,unlock_pur=%d", roomstr,v.account, v.nickname, onedata.reward_cookie, onedata.reward_sugar, unlock_pur)
            rr.run_mfa(v.game_clt_id.base_svr_id, "user_mgr", "give_reward", 
                {v.game_clt_id.base_rpc_clt_id,cookie_reward, sugar_reward, need_add_champion, v.destroynum, v.total_killnum, v.score, v.highest_combo, unlock_pur, roommode, rank})
        end
        rank = rank + 1
    end

    self.my_room:broadcast("NotifyResultData", "plane.ResultDataMsg", result_msg)
end

return Rankmgr
