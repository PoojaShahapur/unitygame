-- room_mgr.lua
-- 房间管理。

local M = { }

local log = require("log"):new("plane.room_mgr")
local config = require("config")
local Room = require("plane.room")
local CrazyRoom = require("plane.crazyroom")
local user_mgr = require("user_mgr")
local pb = require("protobuf")
local rr = require("remote_run.remote_runner")
local serpent = require("serpent")
local router = require("rpc.base_rpc_router")
-- 房间列表, 以room_id为键
local rooms_list = {}
local team_rooms_list = {}
local cur_room_id = 0
local timer_queue = c_timer_queue.CTimerQueue()
local table_insert = table.insert

function M.get_or_new_room(game_clt_id, mode)
    local game_clt_id_str = game_clt_id:to_string()
	log:debug("EnterRoom, game_clt_id_str = %s,mode=%d", game_clt_id_str, mode)

    local room = nil
    local roomid = 0
    if mode == 0 then
        for k,v in pairs(rooms_list) do
            if v:get_room_left_can_in_num() > 0 then
                room = v
                roomid = k
                break
            end
        end
    end

    if room == nil then
        cur_room_id = cur_room_id + 1
        --[[
        注意,下面这行代码使用std::make_shared创建了一个c++类的smart pointer,并返回引用
        lua里不要到处引用,否则时间到了,房间还未被回收
        目前引用Room的地方:
        rooms_list
        --]]
        local last_seconds = 0
        if mode == 0 then
            room = c_room.Room(cur_room_id)
            last_seconds = config.room.last_seconds
        else
            room = c_room.PurgatoryRoom(cur_room_id)
            last_seconds = config.purgatory_room_last_seconds
        end
        log:debug('create new room, cur_room_id = %d,last_seconds=%d', cur_room_id, last_seconds)
        roomid = cur_room_id
        timer_queue:insert_single_from_now(last_seconds *1000, function()
            log:debug('添加定时器room id=' .. roomid)
            M.room_timeover(roomid, mode)
        end)
        --[[
        room = Room:new(cur_room_id, function (room_id)
            log:debug("destroy callback, Room:"..room_id)
            M.erase_room(room_id)
        end)
        ]]
        rooms_list[cur_room_id] = room
    end
    return roomid, room
end

function M.enter_team_room(teamdata)
    local room = nil
    local roomid = 0
    for k,v in pairs(team_rooms_list) do
        if v:canThisTeamInRoom(teamdata)  then
            room = v
            roomid = k
            break
        end
    end

    if room == nil then
        cur_room_id = cur_room_id + 1
        log:debug('create new team room, cur_room_id = %d', cur_room_id)
        room = c_room.TeamRoom(cur_room_id)
        roomid = cur_room_id
        room:canThisTeamInRoom(teamdata)
        timer_queue:insert_single_from_now(config.room.last_seconds *1000, function()
            M.teamroom_timeover(roomid)
        end)
        team_rooms_list[cur_room_id] = room
    end

    return roomid, true
end

function M.reconnect_team_room(serverid, clientid, roomid, uid)
    log:debug('reconnect_team_room,roomid=%d,uid=%d', roomid, uid)
    local room = team_rooms_list[roomid]
    if not room then
        return
    end
    local resp_str = room:playerReconnect(serverid, clientid, uid)
    log:debug("resp_str=%s", resp_str)
    return resp_str
end

function M.join_team_room(teamdata, roomid)
    log:debug("%s", serpent.block(team_rooms_list))
    local room = team_rooms_list[roomid]
    if not room then
        return 1, "该房间已不存在!"
    end

    if not room:canThisTeamInRoom(teamdata) then
        return 1, "该房间已不能加入!"
    end

    return 0
end

function M.get_room_byid(id)
    return rooms_list[id]
end

function M.erase_player(game_clt_id)
    local game_clt_id_str = game_clt_id:to_string()
    log:debug("erase_player: " .. game_clt_id_str)
    local roomid = c_room.exitRoom(game_clt_id_str)
    if rooms_list[roomid].room_mode == 1 then
        M.erase_room(roomid)
    end
    -- 断线或者返回大厅通知session移除我的房间数据
    --[[ 
    if rooms_list[roomid].nonai_players_count == 0 then
        M.erase_room(roomid)
        collectgarbage()
    end
    --]]
end  -- erase_player

function M.room_timeover(room_id, mode)
    local room = rooms_list[room_id]
    if room == nil then
        log:error('时间到了找不到room'..room_id)
        return
    end
    log:info('room=%d,mode=%d时间已到',room_id, mode)
        M.give_reward(room, mode)
        local all_rankdata = room:get_all_player_rankdata()
        for k,v in pairs(all_rankdata) do
            if v.base_rpc_clt_id ~= 0 then
                local game_clt_id = c_game_clt_id.CGameCltId(v.base_svr_id, v.base_rpc_clt_id)
                router.reset_svc_dst_svr_id(game_clt_id, "plane.Plane")
                -- 设置可以用钱了
                rr.run_mfa(game_clt_id.base_svr_id,
                    "user_mgr", "set_user_can_op_money",
                    { game_clt_id.base_rpc_clt_id })
                M.erase_player(game_clt_id)
            end
        end


    room = nil
    M.erase_room(room_id)
end

function M.teamroom_timeover(roomid)
    local room = team_rooms_list[roomid]
    if not room then
        log:error('时间到了找不到room'..roomid)
        return
    end
    log:info('room=%d时间已到',roomid)
    M.give_team_reward(room)
    local all_rankdata = room:get_all_player_rankdata()
    for k,v in pairs(all_rankdata) do
        if v.base_rpc_clt_id ~= 0 then
            local game_clt_id = c_game_clt_id.CGameCltId(v.base_svr_id, v.base_rpc_clt_id)
            router.reset_svc_dst_svr_id(game_clt_id, "plane.Plane")
            -- 设置可以用钱了
            rr.run_mfa(game_clt_id.base_svr_id,
                "user_mgr", "set_user_can_op_money",
                { game_clt_id.base_rpc_clt_id })
            M.erase_player(game_clt_id)
        end
    end
    room = nil
    team_rooms_list[roomid] = nil
    collectgarbage()
    rr.run_mfa(1, "session_user_state_mgr", "destroy_room", {c_util.get_my_svr_id(), roomid})
end

-- 房间时间到时候,给奖励
function M.give_reward(room, roommode)
    local result_msg = {
        datas = {},
    }
    local mvpid = 0
    if roommode == 0 then
        mvpid = room:getmvpid()
    end
    local all_rankdata = room:get_all_player_rankdata()
    local rewardtbl = nil
    local roomstr = '普通模式'
    if roommode == 0 then
        rewardtbl = config.rankreward
    else
        roomstr = '炼狱模式'
        rewardtbl = config.purgatory_reward
    end
    local rank = 1
    -- v是c++里LuaRankData
    for k,v in pairs(all_rankdata) do
            local reward = rewardtbl[rank]
            local sugar_reward = (reward == nil and 0 or reward.sugar)
            local cookie_reward = (reward == nil and 0 or reward.cookie)
            local is_mvp = 0
            if v.id == mvpid then
                is_mvp = 1
            end
            local onedata = {
                playerid = v.id,
                nickname = v.nickname,
                username = v.account,
                killnum = v.destroynum,
                score = v.score,
                reward_sugar = sugar_reward,
                reward_cookie = cookie_reward,
                is_ai = v.isai,
                is_mvp = is_mvp,
                uid = v.id,
                rank = rank,
            }
            local need_add_champion = false
            local unlock_pur = 0
            if rank ==1 then
                need_add_champion = true
                unlock_pur = 1
            end
            table_insert(result_msg.datas, onedata)
                log:info("%s本局结束,给%s,%s发 money=%d,sugar=%d,unlock_pur=%d"
                    , roomstr,v.account, v.nickname, onedata.reward_cookie, onedata.reward_sugar, unlock_pur)
            if v.base_svr_id ~= 0 and v.base_rpc_clt_id ~= 0 then
                rr.run_mfa(v.base_svr_id, "user_mgr", "give_reward", 
                    {v.base_rpc_clt_id,cookie_reward, sugar_reward
                    , need_add_champion, v.destroynum, v.total_killnum, v.score, v.highest_combo, unlock_pur, roommode, rank, is_mvp})
            end
        rank = rank + 1
    end
    room:lua_broadcast("plane.PlanePush", "NotifyResultData", pb.encode("plane.ResultDataMsg", result_msg))
end

function M.give_team_reward(room)
    local result_msg = {
        datas = {},
    }
    local mvpid = room:getmvpid()
    local all_rankdata = room:get_all_player_rankdata()
    for k,v in pairs(all_rankdata) do
        local reward = config.team_rankreward[v.rank]
        local sugar_reward = (reward == nil and 0 or reward.sugar)
        local cookie_reward = (reward == nil and 0 or reward.cookie)
        local is_mvp = 0
        if v.id == mvpid then
            is_mvp = 1
        end
        local onedata = {
            playerid = v.id,
            nickname = v.nickname,
            username = v.account,
            killnum = v.destroynum,
            score = v.score,
            reward_sugar = sugar_reward,
            reward_cookie = cookie_reward,
            is_ai = v.isai,
            is_mvp = is_mvp,
            uid = v.id,
            rank = v.rank,
        }
        local need_add_champion = false
        if rank ==1 then
            need_add_champion = true
        end
        table_insert(result_msg.datas, onedata)
            log:info("团战模式本局结束,给%s,%s发 money=%d,sugar=%d"
                , v.account, v.nickname, onedata.reward_cookie, onedata.reward_sugar)
        if v.base_svr_id ~= 0 and v.base_rpc_clt_id ~= 0 then
            rr.run_mfa(v.base_svr_id, "user_mgr", "give_reward", 
                {v.base_rpc_clt_id,cookie_reward, sugar_reward
                , need_add_champion, v.destroynum, v.total_killnum, v.score, v.highest_combo, 0, 2, v.rank, is_mvp})
        end
    end
    room:lua_broadcast("plane.PlanePush", "NotifyResultData", pb.encode("plane.ResultDataMsg", result_msg))
end

function M.erase_room(room_id)
        log:debug("Delete room:"..room_id)
        rooms_list[room_id] = nil
        collectgarbage()
        rr.run_mfa(1, "session_user_state_mgr", "destroy_room", {c_util.get_my_svr_id(), room_id})
end

-- 由远程balancer调用，分配房间并加入
function M.enter_room(base_svr_id, base_rpc_clt_id, account,uid, nickname,mode, skinid, bulletid)
	assert("number" == type(base_svr_id))
	assert("number" == type(base_rpc_clt_id))
	assert("string" == type(nickname))
	local game_clt_id = c_game_clt_id.CGameCltId(base_svr_id, base_rpc_clt_id)
    local game_clt_id_str = game_clt_id:to_string()
    -- 如果是炼狱模式,直接创建一个满人房间
    --[[
    if mode == 1 then
        cur_room_id = cur_room_id + 1
        room = CrazyRoom:new(cur_room_id, function (room_id)
            log:debug("destroy callback, crazy room:"..room_id)
            M.erase_room(room_id)
        end)
        rooms_list[room.room_id] = room
        -- 炼狱模式与段位无关
	    local resp = room:enter(game_clt_id, account, uid, nickname, skinid, bulletid)
	    return room.room_id, resp
    end
    --]]
	local roomid, room = M.get_or_new_room(game_clt_id, mode)
    local serverid = game_clt_id.base_svr_id
    local clientid = game_clt_id.base_rpc_clt_id
	assert(room)
	room:playerEnter(serverid, clientid, account, uid, nickname, skinid, bulletid)
    --[[
    --如果房间满了,通知session下
    if room:get_room_left_can_in_num() == 0 then
        log:info("notify session full, svrid=%d,roomid=%d", c_util.get_my_svr_id(), room.room_id)
        rr.run_mfa(1, "balance_mgr", "add_fullroom", {c_util.get_my_svr_id(), 0}, function (ok)
            if ok then
                room.already_tell_session = 1
            end
        end)
    end
    ]]
	return roomid
end  -- assign_room()

function M.reconnect_room(base_svr_id, base_rpc_clt_id, roomid, uid)
    log:debug('reconnect_oom,roomid=%d,uid=%d', roomid, uid)
    local room = rooms_list[roomid]
    if not room then
        return
    end
    local resp_str = room:playerReconnect(base_svr_id, base_rpc_clt_id, uid)
    return resp_str
end

-- uid请求加入 roomid的房间
function M.req_join_normal_room(base_svr_id, base_rpc_clt_id, uid, roomid, account, nickname, skinid, bulletid) 
    log:info('请求加入普通模式房间id=%d, %d,%s,%s', roomid, uid, account, nickname)
    local retcode = 0
    local room = rooms_list[roomid]
    if not room then
        rr.run_mfa(base_svr_id, "user_mgr", "popMessageBox", 
            {base_rpc_clt_id, "找不到该玩家所在房间!"}
        )
        retcode = 1
        return
    end

    if room:get_room_left_can_in_num() <= 0 then
        rr.run_mfa(base_svr_id, "user_mgr", "popMessageBox", 
            {base_rpc_clt_id, "该房间已不允许加入!"}
        )
        retcode = 1
        return
    end
	room:playerEnter(base_svr_id, base_rpc_clt_id, account, uid, nickname, skinid, bulletid)
    rr.run_mfa(base_svr_id, "user_mgr", "reconnect_room", 
        {uid, c_util.get_my_svr_id(), roomid}
    )

    return retcode, uid, c_util.get_my_svr_id(), roomid 
end

-- 该函数是在目标服务器上被调用
-- 1. 去目标服务器找不到玩家或玩家的房间,弹框错误
-- 2. 找到房间了,但房间人数已满,弹框错误
-- 3. 直接回 EnterRoomResponse 给玩家
function M.join_room(roomid, base_svr_id, base_rpc_clt_id, dstacc, account,uid, nickname, skinid, bulletid)
    log:info('join_room, baseid=%d, rpc id=%d,dstacc=%s,nickname=%s', base_svr_id, base_rpc_clt_id, dstacc, nickname)
    local room = M.get_room_byid(roomid)
    if room == nil then
        rr.run_mfa(base_svr_id, "user_mgr", "popMessageBox", 
            {base_rpc_clt_id, "找不到玩家所在房间!"}
        )
        return 0,''
    end

    if room.type == "PlaneCrazyRoom" then
        rr.run_mfa(base_svr_id, "user_mgr", "popMessageBox", 
            {base_rpc_clt_id, "该玩家正在自虐,请不要打扰!"}
        )
        return 0,''
    end

    if room:get_room_left_can_in_num() <= 0 then
        rr.run_mfa(base_svr_id, "user_mgr", "popMessageBox", 
            {base_rpc_clt_id, "该房间人数已满或要结束了,不允许加入!"}
        )
        return 0,''
    end

	local game_clt_id = c_game_clt_id.CGameCltId(base_svr_id, base_rpc_clt_id)
	local resp = room:enter(game_clt_id, account, uid, nickname, skinid, bulletid)
    return room.room_id, resp
end

-- 由远程直连服调用
function M.on_disconnected(base_svr_id, base_rpc_clt_id)
	log:debug("on_disconnected %u.%u", base_svr_id, base_rpc_clt_id)
	local game_clt_id = c_game_clt_id.CGameCltId(base_svr_id, base_rpc_clt_id)
	M.erase_player(game_clt_id);
end  -- on_disconnected()

function M.give_session_stress_data()
    local full_room_count = 0
    for k,v in pairs(rooms_list) do
        if  v.already_tell_session == 1 or v:get_room_left_can_in_num() == 0 then
            v.already_tell_session = 1
            full_room_count = full_room_count + 1
        end
    end

    log:info("stress info,count=%d", full_room_count)
    return full_room_count
end

-- 获取人满的房间个数,假设A服务器上1个房间人满,B服务器上2个房间人满,那么新来玩家登录时候,负载均衡选择A服务器
function M.get_full_room_count()
    local count = 0
    for k,v in pairs(rooms_list) do
        if v:get_room_left_can_in_num() == 0 then
            count = count + 1
        end
    end

    return count
end

return M
