--[[
User class.
Author: Jin Qing (http://blog.csdn.net/jq0123)
--]]

local User = {}
--[[
User = {
game_clt_id = CGameCltId,
account = "",
log = log:new("user." .. rpc_clt_id)  -- 使用rpc_clt_id安全，account无法控制长度和内容
timer_queue = c_timer_queue.CTimerQueue()  定时器队列，下线时会自动清空
}
--]]

local Log = require("log")
local Bagm = require("plane.bagm")
local Skinm = require("plane.skinm")
local Varm = require("plane.varm")
local pb = require("protobuf")
local excelm = require("excelm")
local objectbm = require('excelm').objectbm
local levelbm = require("excelm").levelbm
local Object = require('plane.object')
local my_svr_id = c_util.get_my_svr_id()
local mdb = require("database")
local router = require("rpc.base_rpc_router")
local serpent = require('serpent')
local http_client = require("httpclient")
local rr = require("remote_run.remote_runner")
local json = require("json")
local config = require("config")
local time = require("time")
-- 一页里面有6条数据
local one_page_data_num = 6
-- 最多关注100个玩家
local follow_limit = 100

MLoader("cell.plane.signin_mgr");

assert(my_svr_id > 0)

--[[ 数据库字段列表:
_id为openid
uid为全服唯一玩家id,自增
account 为用户名,系统默认分配UFO+uid的用户名,玩家也可以自己设置;
header_imgid, header_imgurl 为头像id,头像url
sex 为玩家性别
area_code 为地区码
age 为玩家年龄
viewed_num 为个人主页访问计数
voice_playednum 为个人语音介绍播放次数
sign 为玩家个性签名
last_login_time 为上次登录时间戳
last_change_account_time 为上次修改昵称的时间戳
total_game 总游戏的场次
total_champion 拿到冠军的局数
total_mvp 拿到mvp局数
total_destroy 总共团灭次数
total_kill 总共击杀的飞机数
highest_score 历史最高单局得分
highest_combo 历史最高连杀 combo 数
objstr 为道具数据存储
skinstr 为皮肤数据存储
money,ticket.plastic的价值和稀有程度依次提升
flags 位保留字段
]]
function User:make_db_data(openid, userid)
    return {
        _id = openid,
        uid = userid,
        account = 'UFO' .. userid,
        header_imgid = 1,
        header_imgurl = '',
        sign = '这个家伙很懒',
        sex = 0,
        area_code = 1,
        age = 10,
        viewed_num = 0,
        voice_playednum = 0,
        last_login_time = os.time(),
        last_logout_time = os.time(),
        last_change_account_time = 0,
        total_game = 0,
        total_champion = 0,
        total_mvp = 0,
        total_destroy = 0,
        total_kill = 0,
        highest_score = 0,
        highest_combo = 0,
        objstr = '',
        skinstr = '',
        varstr = '',
        money = 0,
        ticket = 0,
        plastic = 0,
        flags = 0,
        level = 1,
        state = 0,
        shorturl = '',
        total_get = 0,--历史总共获得棒棒糖
        pur_highest_score = 0,--炼狱最高得分
        can_in_pur = 0,--是否能进入炼狱模式
        purgatory_reward = 0,--炼狱模式奖励
        can_in_team = 0,--是否能进团队模式
    }
end

-- login_info 为 db查询到的玩家数据
function load_data_from_db(login_info)
    return {
        openid = login_info['_id'],
        uid = login_info['uid'],
        account = login_info['account'],
        -- 更换头像时保存
        header_imgid = login_info['header_imgid'],
        header_imgurl = login_info['header_imgurl'],
        -- 更换个人资料时保存
        sex = login_info['sex'],
        area_code = login_info['area_code'],
        age = login_info['age'],
        sign = login_info['sign'],
        -- 主页被访问时更新
        viewed_num = login_info['viewed_num'],
        voice_playednum = login_info['voice_playednum'],
        last_login_time = login_info['last_login_time'] or 0,
        last_logout_time = login_info['last_logout_time'] or 0,
        --last_login_time = os.time(),
        --每局结束时候更新
        total_game = login_info['total_game'],
        total_champion = login_info['total_champion'],
        total_mvp = login_info['total_mvp'],
        total_destroy = login_info['total_destroy'],
        purgatory_reward = login_info['purgatory_reward'] or 0,
        total_kill = login_info['total_kill'],
        highest_score = login_info['highest_score'],
        highest_combo = login_info['highest_combo'],
        pur_highest_score = login_info['pur_highest_score'] or 0,
        can_in_pur = login_info['can_in_pur'] or 0,
        can_in_team = login_info['can_in_team'] or 0,
        last_change_account_time = login_info['last_change_account_time'] or 0,
        --objstr = '',
        --skinstr = '',
        --当钱发生变化时候更新
        money = login_info['money'],
        ticket = login_info['ticket'],
        plastic = login_info['plastic'],
        flags = login_info['flags'],
        level = login_info['level'] or 1,
        shorturl = login_info['shorturl'] or '',
        total_get = login_info['total_get'] or 0,
    }
end

function User:new(rpc_clt_id, login_info)
	assert(rpc_clt_id > 0)
    local user = load_data_from_db(login_info)
	user.game_clt_id = c_game_clt_id.CGameCltId(my_svr_id, rpc_clt_id)
	user.log = Log:new("user." .. rpc_clt_id .. ' ' .. user.openid .. ',' .. user.uid .. ',' .. user.account)
    --user.log:info("%s", serpent.block(user))
    user.can_op_money = 1
    user.current_redis_taskid = 0--redis异步任务的id
    user.all_redis_task_endtime = {}-- key:redis异步任务id,value:该任务结束的毫秒时间戳,超过该时间戳后,任务不会继续执行
    user.last_search_friend_time = 0
    user.need_save = false
    user.bagm = Bagm:new(user)
    local objstr = ''
    if login_info ~= nil and login_info['objstr'] ~= nil then
        objstr = login_info['objstr']
    end
    user.bagm:unserialize(objstr)

    local skinstr = ''
    if login_info ~= nil and login_info['skinstr'] ~= nil then
        skinstr = login_info['skinstr']
    end
    user.skinm = Skinm:new(user)
    user.skinm:unserialize(skinstr)

    local varstr = ''
    if login_info ~= nil and login_info['varstr'] ~= nil then
        varstr = login_info['varstr']
    end
    user.varm = Varm:new(user)
    user.varm:unserialize(varstr, user.last_login_time)
	
	--签到系统
	local signinstr = ''
    if login_info ~= nil and login_info[GlobalNS.signin_mgr.DB_KEY] ~= nil then
        signinstr = login_info[GlobalNS.signin_mgr.DB_KEY]
    end
	user.mSigninSys = GlobalNS.new(GlobalNS.signin_mgr);
	user.mSigninSys:setPlayer(user);
	user.mSigninSys:init();
	user.mSigninSys:unserialize(signinstr);

	setmetatable(user, self)
	self.__index = self
	user.timer_queue = c_timer_queue.CTimerQueue()
    user.last_loop_timestamp = os.time()
    if user.last_login_time ~= 0 and time.is_two_time_in_different_day(user.last_login_time, os.time()) then
        user:zero_clock()
        if time.is_two_time_in_different_month(user.last_login_time, os.time()) then
            user:month_zero_clock()
        end
    end
    user.timer_queue:insert_repeat_from_now(5000,5000,
        function () user:loop5Second() end)
	return user
end  -- new()

-- 异步查询 关注/粉丝 数据
function User:async_query_follow(taskid, ctx, reply, uid)
    local redis_task = self.all_redis_task_endtime[taskid]
    if redis_task == nil then
        return
    end
    -- 查好了关注的总数,去查对应页的uid集合
    if redis_task.step == "num" then
        if reply.type == 3 then
            local total_page_num = (reply.integer + one_page_data_num - 1) / one_page_data_num
            if redis_task.page > total_page_num or redis_task.page == 0 then
                return
            end
            local start_index = one_page_data_num * (redis_task.page - 1)
            local end_index = start_index + one_page_data_num - 1
            local one_page_data_msg = {
                total_num = reply.integer,
                page_num = redis_task.page,
                datas = {},
            }
            redis_task.step = "getuid"
            redis_task.msg = one_page_data_msg
            local key = "following:" .. self.uid
            if redis_task.following == 1 then
                key = "followed:" .. self.uid
            end
	        c_redis.command("zrange", key, {start_index, end_index}, function(reply)
                    self:async_query_follow(taskid, ctx, reply)
                end)
        else
            self.log:error("zcard,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
        end
    -- 获得到了所有的uid,接下来去 mongodb查这些uid数据
    elseif redis_task.step == "getuid" then
         if 2 == reply.type and #reply.elements > 0 then
             --local mongodb_in_str = '{"uid":{"$in":[1,2,3]}}'
             local uid_list = {
             }
             local id_str = ''
             for k,v in pairs(reply.elements) do
                 if v.type == 1 then
                     id_str = id_str .. v.str .. ','
                     table.insert(uid_list, tonumber(v.str))
                 else
                     self.log:info("解析关注列表 elements error,type=%d,integer=%d,str=%s", v.type,v.integer, v.str)
                 end
             end
             -- 去 mongodb 中批量查询数据
             id_str = string.sub(id_str, 1, #id_str - 1) --去掉末尾的逗号
             local mongodb_query_str = '{"uid":{"$in":[' .. id_str .. ']}}'
             redis_task.step = "getdata"
             redis_task.uids_list = uid_list
             rr.run_mfa(1, "login_mgr", "check_uids_state", {redis_task.uids_list}, function (all_uids_state)
                 for _,uid_state in pairs(all_uids_state) do
                     redis_task.all_uids_state[uid_state.uid] = uid_state.state
                 end
                mdb:query_b(mdb.db, mdb.collection, mongodb_query_str, '', function(results)
                    self:async_query_follow(taskid, ctx, results)
                end)
             end)
         else
            self.log:error("zrange,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
         end
    -- 获得到了每个账号的数据，接下来查看我是否关注了每个账号
    elseif redis_task.step == "getdata" then
        local need_check_follow_num = 0
         for i = 1,#reply do
             local user_info = mdb:get_table(reply[i])
             -- 需要检查我是否关注的玩家的数量
             local uid = user_info['uid']
             local one_follow_data = {
                 account = user_info['account'],
                 header_imgid = user_info['header_imgid'],
                 header_imgurl = user_info['header_imgurl'],
                 sex = user_info['sex'],
                 area_code = user_info['area_code'],
                 age = user_info['age'],
                 already_follow = 0,
                 level = user_info['level'],
                 state = redis_task.all_uids_state[uid],
                 uid = user_info['uid'],
             }
             redis_task.msg.datas[uid] = one_follow_data
             need_check_follow_num = need_check_follow_num + 1
             c_redis.command("zscore", "followed:" .. uid, self.uid, function(reply)
                    self:async_query_follow(taskid, ctx, reply, uid)
                 end)
         end
         redis_task.need_check_follow_num = need_check_follow_num
         redis_task.step = "checkfollow"
     elseif redis_task.step == "checkfollow" then
         if 1 == reply.type or 4 == reply.type then
             if redis_task.msg.datas[uid] ~= nil then
                 redis_task.msg.datas[uid].already_follow = (reply.type == 4 and 0 or 1)
                 redis_task.need_check_follow_num = redis_task.need_check_follow_num - 1
                 if redis_task.need_check_follow_num == 0 then
                     local datas = redis_task.msg.datas
                     redis_task.msg.datas = {}
                     for k,v in pairs(datas) do
                         table.insert(redis_task.msg.datas, v)
                     end
                     --self.log:info("%s", serpent.block(redis_task))
                    if redis_task.following == 1 then
                        c_rpc.reply_to(ctx, pb.encode("plane.OnePageFollowedDataMsg", redis_task.msg))
                    else
                        c_rpc.reply_to(ctx, pb.encode("plane.OnePageFollowingDataMsg", redis_task.msg))
                    end
                    --self.log:info('query follow finish, return client, %s', serpent.block(redis_task.msg))
                    self.all_redis_task_endtime[taskid] = nil
                 end
             end
         else
            self.log:error("zscore,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
         end
    end
end

-- 去mongo查uid的数据,发给客户端
-- uids_state 为 session上通知过来的uid集合和uid集合的玩家状态
-- total 是总共好友的个数
function User:send_friend_data(ctx, uids_state, current_page, total)
    local ret_msg = {
        total_num = total,
        page_num = current_page,
        datas = {},
    }

    local ordered_uids = {}
    local state_dict = {}
    local id_str = ''
    for _, uid_state in pairs(uids_state) do
        id_str = id_str .. uid_state.uid .. ','
        table.insert(ordered_uids, uid_state.uid)
        state_dict[uid_state.uid] = uid_state.state
    end

    id_str = string.sub(id_str, 1, #id_str - 1) --去掉末尾的逗号
    local mongodb_query_str = '{"uid":{"$in":[' .. id_str .. ']}}'
    local data_dict = {}
    mdb:query_b(mdb.db, mdb.collection, mongodb_query_str
        , {account=1,uid=1,header_imgid=1,header_imgurl=1,sex=1,level=1}, function(results)
            for i = 1,#results do
                local user_info = mdb:get_table(results[i])
                local one_friend_data = {
                    account = user_info['account'],
                    uid = user_info['uid'],
                    header_imgid = user_info['header_imgid'],
                    header_imgurl = user_info['header_imgurl'],
                    sex = user_info['sex'],
                    level = user_info['level'],
                    state = state_dict[user_info['uid']],
                }
                data_dict[one_friend_data.uid] = one_friend_data
            end
            for i =1,#ordered_uids do
                table.insert(ret_msg.datas, data_dict[ordered_uids[i]])
            end
            self.log:debug("%s", serpent.block(uids_state))
            self:rpc_request("plane.TeamPush", "NotifyOnePageFriendData", pb.encode("plane.OnePageFriendDataMsg", ret_msg))
    end)
end

function User:async_check_followed(taskid, ctx, reply, uid)
    local redis_task = self.all_redis_task_endtime[taskid]
    if redis_task == nil then
        return
    end

    if 1 == reply.type or 4 == reply.type then
        redis_task.already_follow_dict[uid] = (reply.type == 4 and 0 or 1)
        redis_task.need_check_num = redis_task.need_check_num - 1
        if redis_task.need_check_num == 0 then
            local result_msg = {
                results = {},
            }

            for k,v in pairs(redis_task.already_follow_dict) do
                table.insert(result_msg.results, {uid = k, already_followed = v})
            end
            c_rpc.reply_to(ctx, pb.encode("plane.CheckFollowResultMsg", result_msg))
            --self.log:info("%s", serpent.block(result_msg))
            self.all_redis_task_endtime[taskid] = nil
        end
    else
    end
end

function User:is_valid_user()
	return true
end

function User:get_rpc_clt_id()
	return self.game_clt_id.base_rpc_clt_id
end  -- get_rpc_clt_id()

function User:rpc_request(service_name, method_name, request, callback)
	assert("string" == type(service_name))
	assert("string" == type(method_name))
	assert("string" == type(request))
	assert(not callback or "function" == type(callback))
	--self.log:debug("rpc request: %s.%ssvrid=%d,rpcid=%d", service_name, method_name, self.game_clt_id.base_svr_id, self.game_clt_id.base_rpc_clt_id)
	c_rpc.request_clt(self.game_clt_id, service_name, method_name, request, callback)
end  -- rpc_request

function User:send_all_money_to_client()
    local msg = {
        money = self.money,
        ticket = self.ticket,
        plastic = self.plastic,
    }
    --self.log:info('money=%s', serpent.block(msg))
    self:rpc_request("plane.ObjectPush", "NotifyAllMoneyInfo", pb.encode("plane.AllMoneyMsg", msg))
end

function User:online()
    self.log:info("online")
    -- 将玩家的金钱同步给客户端
    -- 将玩家包裹内所有道具发送给客户端
    self:give_share_reward()
    if self.shorturl == '' then
        self:req_short_url()
    end
    --[[
    --下面为溢出代码,下次玩家上线时候读取到的是 (unsigned int)-4,就会给玩家发无限多奖励
    mdb:update_b(mdb.db, mdb.collection, '{"uid" : ' .. self.uid .. '}', "$inc"
    , {purgatory_reward = -2}, function(result)
        if result == 0 then
            self.log:info('成功获得炼狱奖励=%d', self.purgatory_reward)
        else    
            self.log:error('获得炼狱奖励失败=%d', self.purgatory_reward)
        end
    end)
    mdb:query_b(mdb.db, mdb.collection, {uid = self.uid}, {purgatory_reward = 1}, function(results)
        if #results > 0 then
            mdb:update_b(mdb.db, mdb.collection, '{"uid" : ' .. self.uid .. '}', "$inc"
            , {purgatory_reward = -2}, function(result)
                if result == 0 then
                    self.log:info('成功获得炼狱奖励=%d', self.purgatory_reward)
                else    
                    self.log:error('获得炼狱奖励失败=%d', self.purgatory_reward)
                end
            end)
        end
    end)
    ]]
    self.log:debug('lua函数异常后后面代码可以继续运行')
    self.bagm:send_all_objects_to_client()
    self.skinm:online()
    self:send_all_money_to_client()
    self:give_purgatory_reward()
    if self.level == 0 then
        self.log:error("上线时段位为0,强制设为1级")
        self.level = 1
        mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {level = self.level})
    end
    rr.run_mfa(1, "session_user_state_mgr", "online", {c_util.get_my_svr_id(), self.uid})
    -- 为了断g重连,这个房间服务器id应该在进入房间时候设置,在退出房间(每局结束)时候清除
    --[[
    c_redis.set("room:" .. self.account, c_util.get_my_svr_id(),function(reply_type)
        if reply_type == true then
        else
            self.log:error("%s,%d,%s offline, set login key=0 failed", self.openid, self.uid, self.account)
        end
    end)
    ]]
	
	self.mSigninSys:online();
end

function User:give_season_reward()
    local month = os.date("%m", os.time())
    if month == '07' then
        return
    end
    -- 根据自己段位,算出本赛季应得的奖励
    self.log:info("发放赛季奖励 level=%d", self.level)
    local base = levelbm[self.level]
    if base == nil then
        return
    end
    local season_reward = config.season_reward[base.type]
    if season_reward == nil then
        return
    end

    local need_send_client_money = false
    for k,v in pairs(season_reward) do
        local obj_base = objectbm[v.objid]
        if obj_base ~= nil then
            if Object.is_skin(obj_base) then
                if self.skinm:is_skinid_actived(obj_base.activeskinid) then
                    self.log:info("赛季奖励,皮肤id=%d已拥有", obj_base.activeskinid)
                else
                    self.skinm:active_skinid(obj_base.activeskinid)
                end
            elseif Object.is_bulletskin(obj_base) then
                if self.skinm:is_bullet_owned(obj_base.baseid) then
                    self.log:info("赛季奖励,子弹id=%d已拥有", obj_base.baseid)
                else
                    self.skinm:active_bullet(obj_base)
                end
            elseif Object.is_money(obj_base) then
                need_send_client_money = true
                self.money = self.money + v.num
                self.log:info("获得饼干%d个,赛季奖励", v.num)
            elseif Object.is_ticket(obj_base) then
                need_send_client_money = true
                self.ticket = self.ticket + v.num
                self.log:info("获得糖果%d个,赛季奖励", v.num)
            elseif Object.is_plastic(obj_base) then
                need_send_client_money = true
                self.plastic = self.plastic + v.num
                self.log:info("获得塑料%d个,赛季奖励", v.num)
            end
        end
    end

    self.level = 1
    self.need_save = true
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {money = self.money
    , ticket = self.ticket, plastic = self.plastic, level = self.level})
    if need_send_client_money then
        self:send_all_money_to_client()
    end
end
function User:zero_clock_give_purgatory_reward(num)
                    self:add_money_ticket(0, num)
                    self.need_save = true
                    self:popMessageBox("恭喜您获得炼狱排行榜奖励" .. math.floor(num) .. "个棒棒糖")
                    self.log:info('成功获得炼狱奖励=%d', num)
end

function User:give_purgatory_reward()
    -- 如果有炼狱排行榜奖励可以拿,那就去拿
    if self.purgatory_reward > 0 then
        local int,float = math.modf(self.purgatory_reward)
        if float == 0.0 then
            mdb:update_b(mdb.db, mdb.collection, '{"uid" : ' .. self.uid .. '}', "$inc"
            , {purgatory_reward = 0 - self.purgatory_reward }, function(result)
                if result == 0 then
                    self:add_money_ticket(0, self.purgatory_reward)
                    self.need_save = true
                    self:popMessageBox("恭喜您获得炼狱排行榜奖励" .. math.floor(self.purgatory_reward) .. "个棒棒糖")
                    self.log:info('成功获得炼狱奖励=%d', self.purgatory_reward)
                else    
                    self.log:error('获得炼狱奖励失败=%d', self.purgatory_reward)
                end
            end)
        else
            self.log:error("online, 炼狱奖励为浮点数=%d", self.purgatory_reward)
        end
    elseif self.purgatory_reward < 0 then
        self.log:error("online, 炼狱奖励<0=%d", self.purgatory_reward)
    end
end

function User:req_short_url()
    local longurl = 'https://yf.ztgame.com?acc=' .. self.account
    http_client:get(mdb.shorturl .. longurl, function (result)            
            if string.find(result, 'Internal Server Error') ~= nil then
	            self.log:info("获得短链接时,短链接服务器故障")
                return
            else
	            self.log:info("获得短链接, result:"..result)
            end
            local ret = json.decode(result)
            if result ~= '' and ret['origin'] == longurl then
                self.shorturl = ret['link']
                 mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
                     {
                         shorturl = self.shorturl,
                     }
                 )

                 local short_link_msg = {
                     link = self.shorturl,
                 }
                self:rpc_request("plane.PushSocialInfo", "NotifyShortLink", pb.encode("plane.ShortLinkMsg", short_link_msg))
            end
        end)
end

function User:send_share_data(ctx)
    local todaystr = os.date("%Y%m%d")
    local key = "reward:" .. todaystr .. ':' .. self.account
    --self.log:info('send share data, key%s', key)
	c_redis.get(key, function(reply_type, v)
		if 0 == reply_type then
            v = tonumber(v)
            if v > 0 then
                --todo,这块可以直接封装成函数,获得还能加多少钱
                local old_share_num = self.varm:get_daily_var(var_today_share_get_sugar)
                if old_share_num == nil then old_share_num = 0 end
                local new_share_num = self.varm:add_daily_var(var_today_share_get_sugar, 2 * v)--别人点击1次,我能获得2个棒棒糖
                local num = new_share_num - old_share_num
                if num > 0 then
                    self.ticket = self.ticket + num
                    self.total_get = self.total_get + num
                    self:send_all_money_to_client()
                    self.need_save = true
                    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
                        {ticket = self.ticket, total_get = self.total_get}
                    )
                end
                c_redis.set(key, 0)
    
                local share_msg = {
                    share_get = self.varm:get_daily_var(var_today_share_get_sugar),
                    game_get = self.varm:get_daily_var(var_today_game_get_sugar),
                    total_get = self.total_get,
                }
                local resp_str = pb.encode("plane.ShareDataMsg", share_msg)
                c_rpc.reply_to(ctx, resp_str)
            else
                local share_msg = {
                    share_get = self.varm:get_daily_var(var_today_share_get_sugar),
                    game_get = self.varm:get_daily_var(var_today_game_get_sugar),
                    total_get = self.total_get,
                }
                local resp_str = pb.encode("plane.ShareDataMsg", share_msg)
                c_rpc.reply_to(ctx, resp_str)
            end
        else
                local share_msg = {
                    share_get = self.varm:get_daily_var(var_today_share_get_sugar),
                    game_get = self.varm:get_daily_var(var_today_game_get_sugar),
                    total_get = self.total_get,
                }
                local resp_str = pb.encode("plane.ShareDataMsg", share_msg)
                c_rpc.reply_to(ctx, resp_str)
				self.log:error("Redis error,type=%d,%s", reply_type, tostring(v))
        end
    end)
end

function User:give_share_reward()
    local todaystr = os.date("%Y%m%d")
    local key = "reward:" .. todaystr .. ':' .. self.account
	c_redis.get(key, function(reply_type, v)
        --self.log:info("reply type=%d,v=%s,%s", reply_type, tostring(type(v)), tostring(v))
        v = tonumber(v)
		if 0 == reply_type and v > 0 then
            --todo,这块可以直接封装成函数,获得还能加多少钱
            local old_share_num = self.varm:get_daily_var(var_today_share_get_sugar)
            if old_share_num == nil then old_share_num = 0 end
            local new_share_num = self.varm:add_daily_var(var_today_share_get_sugar, 2 * v)
            local num = new_share_num - old_share_num
            self.log:info("从redis中读取点击次数%d,今日已获得%d个", v, old_share_num)
            if num > 0 then
                self.log:info("糖果|分享获得 %d", num)
                self.ticket = self.ticket + num
                self.total_get = self.total_get + num
                self:send_all_money_to_client()
                self.need_save = true
                mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
                    {ticket = self.ticket, total_get = self.total_get}
                )
            end
            c_redis.command("decr", key, {v})
        end
    end)
end

function User:offline()
    self:save()
    local now = os.time()
    mdb:update_b(mdb.db, mdb.collection, {_id=self.openid},nil, {last_logout_time = now}, function(result)
        if result ~= 0 then
            self.log:error('更新下线时间=%d失败', now)
        end
    end)
    rr.run_mfa(1, "session_user_state_mgr", "offline", {self.openid, self.account, self.uid})
    self.log:info("offline")
    -- 为了断线重连,这个房间服务器id应该在进入房间时候设置,在退出房间(每局结束)时候清除
    --[[
    c_redis.set("room:" .. self.account, 0,function(reply_type)
        if reply_type == true then
        else
            self.log:error("%s,%d,%s offline, set login key=0 failed", self.openid, self.uid, self.account)
        end
    end)
    ]]
end

function User:save()
    local all_obj = self.bagm:serialize()
    local skin_str = self.skinm:serialize()
    local varstr = self.varm:serialize()
	local signinStr = self.mSigninSys:serialize();
    self.log:info('save, len(all_obj)=%d,len(all_skin)=%d,varstr=%d'
        , string.len(all_obj),string.len(skin_str), string.len(varstr))
    -- mongodb 接口暂时有问题,用 update_b 会导致0变成 0/0,进而 mdb:get_table获取出来数据不完整
    mdb:update(mdb.db, mdb.collection, {_id=self.openid},
        {
            pur_highest_score= self.pur_highest_score,
        }
    )
    mdb:update_b(mdb.db, mdb.collection, {_id=self.openid},nil, 
    {
        objstr = all_obj,
        skinstr = skin_str,
        sign = self.sign,
        varstr =varstr,
		signinStr = signinStr,
    },
    function (result)
        if result ~= 0 then
            self.log:error('存档失败')
        end
    end)
end

function User:save_bag()
    local all_obj = self.bagm:serialize()
    self.log:info('save_bag, all_obj=%d,%s', string.len(all_obj),all_obj)
    if string.len(all_obj) ~= 0 then
        mdb:update_b(mdb.db, mdb.collection, {_id=self.openid}, nil,{objstr = all_obj})
    end
end

-- 往 tbl 中插入 key,value对
function insert_key_value_from_str(str,tbl)
        local key,value = str:match('([^=]+)=([^=]+)')
        if key == nil or value == nil then
            return false
        else
            tbl[key] = value
            return true
        end
end

function User:popMessageBox(content)
    local msg = {
        content = content,
    }
    self:rpc_request("plane.ObjectPush", "PopMessageBox", pb.encode("plane.MessageBoxMsg", msg))
end

function User:set_pur_highest_score(score)
    self.log:info('set_pur_highest_score,score=%d', score)
    self.pur_highest_score = score
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {pur_highest_score = score})
end

function User:add_money_ticket(money, ticket)
    self.money = self.money + money
    self.ticket = self.ticket + ticket
    self:send_all_money_to_client()
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {money = self.money, ticket = self.ticket, plastic = self.plastic})
end

function User:sub_money(num)
    self.money = self.money - num
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {money = self.money, ticket = self.ticket, plastic = self.plastic})
    self:send_all_money_to_client()
end

-- 每局比赛结束后加游戏币和糖果
function User:add_money_after_game(money, sugar, need_add_champion, destroynum,killnum,score,highest_combo, unlock_pur, roommode, rank, is_mvp)
    self.total_game = self.total_game + 1
    if self.can_in_pur == 0 and unlock_pur == 1 then
        self:rpc_request("plane.PushSocialInfo", "NotifyPurgatoryUnlock", '')
        self.can_in_pur = unlock_pur
    end
    if roommode == 0 then
        local newlevel = excelm:get_newlevel_by_personal_rank(self.level, rank)
        if newlevel == 0 then
            self.log:error("计算段位为0,旧段位%d,排名%d", self.level, rank)
        else
            self.log:info("本局结束,更新段位,旧段位%d,新段位%d,can_in_team=%d,rank=%d"
                , self.level, newlevel, self.can_in_team, rank)
            self.level = newlevel
             -- 更新段位排行榜数据
             self:update_levelrank()
             if self.level >= 8 and self.can_in_team == 0 then
                 self.log:debug("send msg")
                self:rpc_request("plane.PushSocialInfo", "NotifyTeamUnlock", '')
                self.can_in_team = 1
             end
        end
        self:rpc_request("plane.PlanePush", "SetMyLevel", pb.encode("plane.LevelMsg", {level = newlevel}))
    elseif roommode == 2 then
        local newlevel = excelm:get_newlevel_by_team_rank(self.level, rank)
        if newlevel == 0 then
            self.log:error("计算段位为0,旧段位%d,排名%d", self.level, rank)
        else
            self.log:info("本局结束,更新段位,旧段位%d,新段位%d,can_in_team=%d,rank=%d"
                , self.level, newlevel, self.can_in_team, rank)
            self.level = newlevel
             -- 更新段位排行榜数据
             self:update_levelrank()
             if self.level >= 8 and self.can_in_team == 0 then
                self:rpc_request("plane.PushSocialInfo", "NotifyTeamUnlock", '')
                self.can_in_team = 1
             end
        end
        self:rpc_request("plane.PlanePush", "SetMyLevel", pb.encode("plane.LevelMsg", {level = newlevel}))
    end
    if need_add_champion == true then
        self.total_champion= self.total_champion + 1
    end
    if is_mvp == 1 then
        self.total_mvp = self.total_mvp + 1
    end
    self.total_destroy = self.total_destroy + destroynum
    self.total_kill = self.total_kill + killnum
    if score > self.highest_score then
        self.highest_score = score
    end
    if highest_combo > self.highest_combo then
        self.highest_combo = highest_combo
    end
    --根据每日获得上限,算出应该给多少糖果
    local oldsugar = self.varm:get_daily_var(var_today_game_get_sugar)
    if oldsugar == nil then oldsugar = 0 end
    local newsugar = self.varm:add_daily_var(var_today_game_get_sugar, sugar)
    sugar = newsugar - oldsugar
    self.log:info('money=%d,newsugar=%d,oldsugar=%d', money, newsugar, oldsugar)
    local need_send_client = false
    local roomstr = (roommode == 0 and '普通模式' or '炼狱模式')
    if money > 0 then
        need_send_client = true
        self.money = self.money + money
        self.log:info("饼干|%s获得 %d", roomstr, money)
    end
    if sugar > 0 then
        need_send_client = true
        self.ticket = self.ticket + sugar
        self.total_get = self.total_get + sugar
        self.log:info("糖果|%s获得 %d", roomstr, sugar)
    end
    if need_send_client then
        self:send_all_money_to_client()
    end
        mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
            {
                money = self.money ,
                ticket = self.ticket,
                plastic = self.plastic,
                total_game = self.total_game,
                total_champion = self.total_champion,
                total_destroy = self.total_destroy,
                total_kill = self.total_kill,
                highest_score = self.highest_score,
                highest_combo = self.highest_combo,
                total_get = self.total_get,
                total_mvp = self.total_mvp,
                can_in_pur = self.can_in_pur,
                can_in_team = self.can_in_team,
                level = self.level,
            }
         )
end

-- 更新段位排行榜数据
function User:update_levelrank()
    if self.level > 0 then
	    c_redis.command("zadd", "ranklevel:1707", {self.level, self.uid}
            , function(reply)
            if reply.type ~= 3 then
                self.log:error("更新段位排行榜失败,level=%d,return error type=%d,integer=%d,str=%s"
                    , self.level, reply.type,reply.integer, reply.str)
            end
        end)
    end
end

function User:set_headerimg(id, url)
    self.header_imgid = id
    self.header_imgurl = url
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
        {header_imgid = self.header_imgid, header_imgurl = self.header_imgurl}
    )
end

function User:change_account(ctx, new_acc)
    if self.account == new_acc then
        return
    end
    if string.find(new_acc,' ') or string.find(new_acc,'%%') then
        self:popMessageBox("昵称中含有非法字符!")
        return
    end
    local op_result_msg = {
        retcode = 0,
        error_str = '',
        new_account = '',
    }
    local now = math.floor(c_util.get_sys_ms() / 1000)
    if now - self.last_change_account_time < 24 * 86400 then
        op_result_msg.retcode = 1
        op_result_msg.error_str = '距离上次更名需要一个月后,才能再次更名哦!'
        local resp_str = pb.encode("plane.SocialOpResultMsg", op_result_msg)
        c_rpc.reply_to(ctx, resp_str)
        return
    end
    if string.sub(new_acc, 1, 3) == "UFO" then
        op_result_msg.retcode = 1
        op_result_msg.error_str = '账号名中包含敏感字符,请换个昵称再试吧!'
        local resp_str = pb.encode("plane.SocialOpResultMsg", op_result_msg)
        c_rpc.reply_to(ctx, resp_str)
        return
    end

    local key = "ufo:" .. new_acc
    c_redis.command("get", key, function(reply)
		if 4 == reply.type then
            c_redis.set(key, self.uid,function(reply_type)
                if reply_type == true then
                    self.account = new_acc
                    self.log:info('acc=%s, new_=%s', self.account, new_acc)
                    self.last_change_account_time = now
                    -- 修改完后,存db
                    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
                        {
                            account = self.account,
                            last_change_account_time = now,
                        }
                    )
                
                    op_result_msg.new_account = self.account
                    local resp_str = pb.encode("plane.SocialOpResultMsg", op_result_msg)
                    c_rpc.reply_to(ctx, resp_str)
                else
                    op_result_msg.retcode = 1
                    op_result_msg.error_str = '账号名重复,请换个昵称试试吧!'
                    local resp_str = pb.encode("plane.SocialOpResultMsg", op_result_msg)
                    c_rpc.reply_to(ctx, resp_str)
                    log:info("往redis中set %s:%d 时失败,修改昵称失败", key, self.uid)
                end
            end)
        else
            self.log:error("zscore,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
            op_result_msg.retcode = 1
            op_result_msg.error_str = '账号名重复,请换个昵称试试吧!'
            local resp_str = pb.encode("plane.SocialOpResultMsg", op_result_msg)
            c_rpc.reply_to(ctx, resp_str)
        end
    end)
end

function User:change_personalinfo(req)
    if req.sex ~=0 and req.sex ~= 1 then
        return
    end
    if req.age < 6 or req.age > 80 then
        return
    end
    if req.areacode < 0 or req.areacode > 34 then
        return
    end

    self.sex = req.sex
    self.age = req.age
    self.area_code = req.areacode
    self.log:info('change personal, now sex=%d,%d,%d'
        , self.sex, self.age, self.area_code)
    -- 修改完后,存db
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, 
        {
            sex = self.sex,
            age = self.age,
            area_code = self.area_code,
        }
    )
end

function User:change_sign(sign)
    self.log:info("change sign,=%s", sign)
    if #sign > 140 then
        return
    end
    self.sign = sign
end

function User:send_account_main_page_info(ctx, uid)
    local main_page_info_msg = {
    }
    if uid == self.uid then
        main_page_info_msg.account = self.account
        main_page_info_msg.header_imgid = self.header_imgid
        main_page_info_msg.header_imgurl = self.header_imgurl
        main_page_info_msg.sign = self.sign or ''
        main_page_info_msg.sex = self.sex
        main_page_info_msg.area_code = self.area_code
        main_page_info_msg.age = self.age
        main_page_info_msg.following_num = 0
        main_page_info_msg.followed_num = 0
        main_page_info_msg.viewed_num = 0
        main_page_info_msg.voiceurl = ''
        main_page_info_msg.level = self.level
        main_page_info_msg.voice_playernum = 0
        main_page_info_msg.next_change_account_ts = self.last_change_account_time + 24 * 86400
        --self.log:info('mainpage=%s', serpent.block(main_page_info_msg))
	    c_redis.command("zcard", "following:" .. self.uid, nil, function(reply)
               if reply.type == 3 then
                   main_page_info_msg.following_num = reply.integer
	                c_redis.command("zcard", "followed:" .. self.uid, nil, function(reply)
                        if reply.type == 3 then
                            main_page_info_msg.followed_num =reply.integer
	                        c_redis.command("get", "viewed:" .. self.uid, nil, function(viewed_reply)
                                if viewed_reply.type == 1 then
                                    main_page_info_msg.viewed_num = tonumber(viewed_reply.str)
                                end
                                --self.log:info("%s", serpent.block(main_page_info_msg))
                                local resp_str = pb.encode("plane.MainPageInfoMsg", main_page_info_msg)
                                c_rpc.reply_to(ctx, resp_str)
                            end)
                        else
                            self.log:error("zcard,followed,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
                        end
                        end)
               else
                    self.log:error("zcard,following,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
               end
            end)
    else
	    mdb:query_b(mdb.db, mdb.collection, {uid=uid}, '', function (results)
                local user_info = nil
                local uid = 0
                if #results > 0 then
                    user_info = mdb:get_table(results[1])
                    uid = user_info.uid
                    main_page_info_msg.account = user_info.account
                    main_page_info_msg.header_imgid = user_info.header_imgid
                    main_page_info_msg.header_imgurl = user_info.header_imgurl
                    main_page_info_msg.sign = user_info.sign
                    main_page_info_msg.sex = user_info.sex == 0 and "BOY" or "GIRL"
                    main_page_info_msg.area_code = user_info.area_code
                    main_page_info_msg.age = user_info.age
                    main_page_info_msg.level = user_info.level
                    main_page_info_msg.following_num = 0
                    main_page_info_msg.followed_num = 0
                    main_page_info_msg.viewed_num = 0
                    main_page_info_msg.voiceurl = 0
                    main_page_info_msg.voice_playernum = 0
                    main_page_info_msg.already_followed = 0
                    main_page_info_msg.next_change_account_ts = self.last_change_account_time + 24 * 86400
	            c_redis.command("zcard", "following:" .. uid, nil, function(reply)
                    if reply.type == 3 then
                        main_page_info_msg.following_num = reply.integer
	                        c_redis.command("zcard", "followed:" .. uid, nil, function(reply)
                                if reply.type == 3 then
                                    main_page_info_msg.followed_num =reply.integer
	                                c_redis.command("incr", "viewed:" .. uid, nil, function(reply)
                                        if reply.type ==3 then
                                            main_page_info_msg.viewed_num = reply.integer
                                        end
                                        c_redis.command("zscore", "followed:" .. uid, self.uid, function(reply)
                                            local reply_type= reply.type
                                            if 1 == reply_type or 4 == reply_type then
                                                main_page_info_msg.already_followed = (reply_type == 4 and 0 or 1)
                                            end
                                            rr.run_mfa(1, "login_mgr", "check_uids_state", {{uid}}, function (all_uids_state)
                                                for _,uid_state in pairs(all_uids_state) do
                                                    main_page_info_msg.state = uid_state.state
                                                    break
                                                end
                                        self.log:info("%s", serpent.block(main_page_info_msg))
                                                local resp_str = pb.encode("plane.MainPageInfoMsg", main_page_info_msg)
                                                c_rpc.reply_to(ctx, resp_str)
                                            end)
                                        end)
                                    end)
                                else
                                            local resp_str = pb.encode("plane.MainPageInfoMsg", main_page_info_msg)
                                            c_rpc.reply_to(ctx, resp_str)
                                    self.log:error("zcard,followed,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
                                end
                                end)
                    else
                                            local resp_str = pb.encode("plane.MainPageInfoMsg", main_page_info_msg)
                                            c_rpc.reply_to(ctx, resp_str)
                            self.log:error("zcard,following,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
                    end
                    end)
                end
            end)
    end
end

function User:send_game_page_info(ctx, account)
    local game_page_info_msg = {
        account = account,
    }
    if account == self.account then
        game_page_info_msg.total_game = self.total_game
        game_page_info_msg.total_champion = self.total_champion
        game_page_info_msg.total_mvp = self.total_mvp
        game_page_info_msg.total_destroy = self.total_destroy
        game_page_info_msg.total_kill = self.total_kill
        game_page_info_msg.highest_score= self.highest_score
        game_page_info_msg.highest_combo = self.highest_combo
        game_page_info_msg.header_imgid = self.header_imgid
        --self.log:info('gamepage=%s', serpent.block(game_page_info_msg))
        local resp_str = pb.encode("plane.GamePageInfoMsg", game_page_info_msg)
        c_rpc.reply_to(ctx, resp_str)
    else
	    mdb:query_b(mdb.db, mdb.collection, {account=account}, '', function (results)
                local user_info = nil
                if #results > 0 then
                    user_info = mdb:get_table(results[1])
                    game_page_info_msg.total_game = user_info.total_game
                    game_page_info_msg.total_champion = user_info.total_champion
                    game_page_info_msg.total_mvp = user_info.total_mvp
                    game_page_info_msg.total_destroy = user_info.total_destroy
                    game_page_info_msg.total_kill = user_info.total_kill
                    game_page_info_msg.highest_score= user_info.highest_score
                    game_page_info_msg.highest_combo = user_info.highest_combo
                    game_page_info_msg.header_imgid = user_info.header_imgid
                end
                local resp_str = pb.encode("plane.GamePageInfoMsg", game_page_info_msg)
                c_rpc.reply_to(ctx, resp_str)
            end)
    end
end

function User:send_skin_page_info(ctx, account)
    local skin_info_msg = {
        account = account,
        indexes = {},
    }
    if account == self.account then
        skin_info_msg.indexes = self.skinm:full_skin_page_msg()
        local resp_str = pb.encode("plane.SkinPageInfoMsg", skin_info_msg)
        c_rpc.reply_to(ctx, resp_str)
    else
        -- 从db里查到别人的皮肤str字段,然后反序列化出来
        mdb:query_b(mdb.db, mdb.collection, {account = account}, {skinstr = 1}, function(results)
            if #results > 0 then
                local skin_info = mdb:get_table(results[1])
                local temp_skinm = Skinm:new()
                temp_skinm:unserialize(skin_info['skinstr'])
                skin_info_msg.indexes = temp_skinm:full_skin_page_msg()
                local resp_str = pb.encode("plane.SkinPageInfoMsg", skin_info_msg)
                c_rpc.reply_to(ctx, resp_str)
            end
        end)
    end
end

function User:follow(ctx, uid)
    --self.log:info('%s,%s,%d请求关注 %d', self.openid, self.account, self.uid, uid)
    if uid == 0 then
        return
    end
    local follow_result_msg = {
        uid = uid,
        result = {
            retcode = 0,
            error_str = '',
        }
    }

    if uid == self.uid then
        follow_result_msg.result.retcode = 1
        follow_result_msg.resp_str.error_str = '不能关注自己哦!'
        self:rpc_request("plane.PushSocialInfo", "FollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
        return
    end

    -- 直接调用zadd,若修改记录条数为0,则代表已经关注过
    local now = os.time()
    local arg = 'NX ' .. now .. ' ' .. uid
    local key = "following:" .. self.uid
	c_redis.command("zcard", key, nil, function(reply)
        if reply.type == 3 then
            if reply.integer > follow_limit then
                follow_result_msg.result.retcode = 1
                follow_result_msg.resp_str.error_str = '您已达到关注上限100人,不能再关注啦!'
                self:rpc_request("plane.PushSocialInfo", "FollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
                return
            end
        else
            self.log:error("查询关注上限zcard,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
            return
        end
	    c_redis.command("zadd", key, {"NX", now, uid}, function(reply)
	        if 3 == reply.type then
                if reply.integer == 1 then
                    self.log:info('关注 %d成功', uid)
                    key = "followed:" .. uid
                    c_redis.command("zadd", key, {"NX", now, self.uid}, function(reply)
                        if 3 == reply.type and reply.integer == 1 then
                            self.log:info('将用户 %s,%s,%d 添加到 %d的粉丝列表成功success', self.openid, self.account, self.uid, uid)
                            self:rpc_request("plane.PushSocialInfo", "FollowSuccess"
                                , pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
                            rr.run_mfa(1, "session_user_state_mgr", "on_add_friend", {self.uid, uid})
                        else
                            self.log:info('将用户 %s,%s,%d 添加到 %d的粉丝列表失败failed', self.openid, self.account, self.uid, uid)
                            self:rpc_request("plane.PushSocialInfo", "FollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
                        end
                    end)
                else
                    follow_result_msg.result.retcode = 1
                    follow_result_msg.result.error_str = '您已关注过该玩家,无需再关注啦!'
                    self:rpc_request("plane.PushSocialInfo", "FollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
                end
	        else
	            self.log:error("粉丝列表更新失败 zadd %s %s", key, arg)
                self:rpc_request("plane.PushSocialInfo", "FollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
	        end  -- if
        end)
    end)

end

function User:unfollow(ctx, uid)
    --self.log:info('%s,%s,%d请求取关 %d', self.openid, self.account, self.uid, uid)
    if uid == 0 then
        return
    end
    local follow_result_msg = {
        uid = uid,
        result = {
            retcode = 0,
            error_str = '',
        }
    }

    if uid == self.uid then
        follow_result_msg.result.retcode = 1
        follow_result_msg.resp_str.error_str = '不能取关自己哦!'
        self:rpc_request("plane.PushSocialInfo", "UnfollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
        return
    end

    -- 直接调用zrem,若修改记录条数为0,则代表不在关注状态
    local key = "following:" .. self.uid
	c_redis.command("zrem", key, uid, function(reply)
	    if 3 == reply.type then
            if reply.integer == 1 then
                self.log:info('取关 %d成功 success', uid)
                key = "followed:" .. uid
                c_redis.command("zrem", key, self.uid, function(reply)
                    if 3 == reply.type and reply.integer == 1 then
                        self.log:info('将用户 %s,%s,%d 从 %d的粉丝列表移除成功 success', self.openid, self.account, self.uid, uid)
                        self:rpc_request("plane.PushSocialInfo", "UnfollowSuccess"
                            , pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
                        rr.run_mfa(1, "session_user_state_mgr", "on_remove_friend", {self.uid, uid})
                    else
                        self.log:info('将用户 %s,%s,%d 从 %d的粉丝列表移除失败 failed', self.openid, self.account, self.uid, uid)
                    end
                end)
            else
                follow_result_msg.result.retcode = 1
                follow_result_msg.result.error_str = '您已关注过' .. account .. ',无需再关注啦!'
            end
	    else
	        self.log:error("粉丝列表移除失败 zrem %d", key, uid)
	    end  -- if
        self:rpc_request("plane.PushSocialInfo", "UnfollowSuccess", pb.encode("plane.FollowUnfollowDataMsg", follow_result_msg))
    end)

end

function User:view_follow(ctx, account, page, following)
    local key = "following:" .. self.uid
    if following == 1 then
        key = "followed:" .. self.uid
    end
    --self.log:info("获取 %s,%s,%d 的%s列表", self.openid, self.account, self.uid, following == 0 and "关注" or "粉丝")
    self.current_redis_taskid = self.current_redis_taskid + 1
    self.all_redis_task_endtime[self.current_redis_taskid] = {
            end_ts = c_util.get_sys_ms() + 2000,
            step = "num",
            page = page,
            key = key,
            following = following,
            all_uids_state = {},
        }
	c_redis.command("zcard", key, nil, function(reply)
        self:async_query_follow(self.current_redis_taskid, ctx, reply)
        end)
end

function User:find_friend_by_acc(ctx, account)
    if account == self.account then
        return
    end
    local friend_result_msg = {
        retcode = 0,
        error_str = '',
        data = { },
    }
    local now = c_util.get_sys_ms()
    if now - self.last_search_friend_time < 1000 then
        friend_result_msg.retcode = 1
        friend_result_msg.error_str = "请求过于频繁,请3秒后再试！"
        local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
        c_rpc.reply_to(ctx, resp_str)
        return
    end
    mdb:query_b(mdb.db, mdb.collection, {account = account},
        {header_imgid=1, header_imgurl=1,sex=1,area_code=1,age=1,already_follow=1,uid=1}, function(results)
            if #results > 0 then
                local user_info = mdb:get_table(results[1])
                local data_tbl = friend_result_msg.data
                data_tbl.account = account
                data_tbl.header_imgid = user_info['header_imgid']
                data_tbl.header_imgurl = user_info['header_imgurl']
                data_tbl.sex = user_info['sex']
                data_tbl.area_code = user_info['area_code']
                data_tbl.age = user_info['age']
                data_tbl.already_follow = 0
                data_tbl.uid = user_info['uid']
                c_redis.command("zscore", "followed:" .. user_info['uid'], self.uid, function(reply)
                    local reply_type= reply.type
                        if 1 == reply_type or 4 == reply_type then
                            data_tbl.already_follow = (reply_type == 4 and 0 or 1)
                            local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
                            c_rpc.reply_to(ctx, resp_str)
                        else
                            self.log:error("zscore,return error type=%d,integer=%d,str=%s", reply_type,reply.integer, reply.str)
                        end
                 end)
            else
                friend_result_msg.retcode = 1
                friend_result_msg.error_str = "您要找的账号不存在,请检查输入是否有误！"
                local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
                c_rpc.reply_to(ctx, resp_str)
            end
    end)
    self.last_search_friend_time = c_util.get_sys_ms()
end

function User:loop5Second()
    local now = os.time()
    local now_ms = c_util.get_sys_ms()
    self.skinm:loop5Second(now)

    local need_remove = {}
    for k,v in pairs(self.all_redis_task_endtime) do
        if v.end_ts > now_ms then
            table.insert(need_remove, k)
        end
    end
    for i = 1,#need_remove do
        self.all_redis_task_endtime[need_remove[i]] = nil
    end

    if time.is_two_time_in_different_day(self.last_loop_timestamp, now) then
        self:zero_clock()
        --[[
        local month1 = os.date("%m", self.last_loop_timestamp)
        local month2 = os.date("%m", now)
        self.log:info("last loop ts=%d,month=%s,now=%d,month=%s", self.last_loop_timestamp, month1, now, month2)
        ]]
        if time.is_two_time_in_different_month(self.last_loop_timestamp,now) then
            self:month_zero_clock()
        end
    end
    self.last_loop_timestamp = now

    if self.need_save == true then
        self:save()
        self.need_save = false
    end
end

function User:zero_clock()
    self.pur_highest_score = 0
    self.varm:zero_clock()
end

function User:month_zero_clock()
	self:give_season_reward()
	self.mSigninSys:onNewMonthStart();
end

function User:find_friend_in_follow(ctx, account, followed)
    if account == self.account then
        return
    end
    local key = 'following:' .. self.uid
    if followed == 1 then
        key = 'followed:' .. self.uid
    end
    local friend_result_msg = {
        retcode = 0,
        error_str = '',
        data = { },
    }
    local now = c_util.get_sys_ms()
    if now - self.last_search_friend_time < 1000 then
        friend_result_msg.retcode = 1
        friend_result_msg.error_str = "请求过于频繁,请3秒后再试！"
        local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
        c_rpc.reply_to(ctx, resp_str)
        return
    end
	c_redis.get("ufo:" .. account, function(reply_type, v)
		if 0 == reply_type then
            local find_uid = v
            --self.log:info('find_uid, key=%s,%d', key,find_uid)
            c_redis.command("zscore", key, find_uid, function(reply)
                if reply.type == 4 then
                    friend_result_msg.retcode = 1
                    friend_result_msg.error_str = '没有搜到结果!'
                    local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
                    c_rpc.reply_to(ctx, resp_str)
                elseif reply.type == 1 then
                    -- 我是关注了该玩家,现在查下该玩家的基本信息
                    mdb:query_b(mdb.db, mdb.collection, {account = account},
                        {header_imgid=1, header_imgurl=1,sex=1,area_code=1,age=1, uid = 1}, function(results)
                        if #results > 0 then
                            local user_info = mdb:get_table(results[1])
                            local data_tbl = friend_result_msg.data
                            data_tbl.account = account
                            data_tbl.header_imgid = user_info['header_imgid']
                            data_tbl.header_imgurl = user_info['header_imgurl']
                            data_tbl.sex = user_info['sex']
                            data_tbl.area_code = user_info['area_code']
                            data_tbl.age = user_info['age']
                            data_tbl.uid = user_info['uid']
                            local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
                            c_rpc.reply_to(ctx, resp_str)
                        else
                            self.log:error("fatal error,%s,%s,%d %s 了%s,%s,但mongodb查不到数据"
                                , self.openid, self.account, self.uid, followed == 1 and "粉" or "关注", account, tostring(find_uid))
                        end
                        end)
                else
                    self.log:error("find_friend_in_following,error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
                end--end of if
             end)
        else
            friend_result_msg.retcode = 1
            friend_result_msg.error_str = '账号' .. account .. '不存在!'
            local resp_str = pb.encode("plane.FindFriendResultMsg", friend_result_msg)
            c_rpc.reply_to(ctx, resp_str)
        end --end of if
    end)
end

function User:check_already_followed(ctx, uidlist)
    -- 防止客户端发过来大坨数据,拖垮服务器
    --self.log:info("check follow %s", serpent.block(uidlist))
    if #uidlist > 20 then
        return
    end
    self.current_redis_taskid = self.current_redis_taskid + 1
    self.all_redis_task_endtime[self.current_redis_taskid] = {
            end_ts = c_util.get_sys_ms() + 2000,
            need_check_num = #uidlist,
            already_follow_dict = {},
        }
    for i = 1,#uidlist do    
        -- 判断我是否关注了这个uid哥们
        c_redis.command("zscore", "followed:" .. uidlist[i], self.uid, function(reply)
                self:async_check_followed(self.current_redis_taskid, ctx, reply, uidlist[i])
            end)
    end
end

function User:send_level_rank_data(ctx)
    local all_uid_str = ''
    local ret_msg = {
        datas = {},
    }
    local myself = {
        acc = self.account,
        imageid = self.header_imgid,
        sex = self.sex,
        level = self.level,
        uid = self.uid,
    }
    table.insert(ret_msg.datas, myself)
	c_redis.command("zrevrange", "ranklevel:1707", {0, 99, "withscores"}
        , function(reply)
            if 2 == reply.type and #reply.elements > 0 and #reply.elements % 2 == 0 then
                local uid2score = {}
                local order_uids = {}
                for i = 1, #reply.elements,2 do
                    local uidstr = reply.elements[i].str
                    uid2score[tonumber(uidstr)] = tonumber(reply.elements[i+1].str)
                    all_uid_str = all_uid_str .. uidstr .. ','
                    table.insert(order_uids, tonumber(uidstr))
                end

                -- 去 mongodb 批量查询数据
                all_uid_str = string.sub(all_uid_str, 1, #all_uid_str - 1)
                local mongodb_query_str = '{"uid":{"$in":[' .. all_uid_str .. ']}}'
                mdb:query_b(mdb.db, mdb.collection, mongodb_query_str, {account = 1, header_imgid=1,sex=1,uid=1,level=1 }, function(results)
                    local all_datas = {}
                    for i = 1,#results do
                        local user_info = mdb:get_table(results[i])
                        local data = {
                            acc = user_info['account'],
                            imageid = user_info['header_imgid'],
                            sex = user_info['sex'],
                            level = user_info['level'],
                            uid = user_info['uid'],
                        }
                        all_datas[user_info['uid']] = data
                    end

                    for i = 1,#order_uids do
                        local data = all_datas[order_uids[i]]
                        table.insert(ret_msg.datas, data)
                    end
                    c_rpc.reply_to(ctx, pb.encode("plane.LevelRankDataMsg", ret_msg))
                end)
            else
               self.log:error("zrange,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
               c_rpc.reply_to(ctx, pb.encode("plane.LevelRankDataMsg", ret_msg))
            end
        end)
end


-- =0代表今日,=1代表昨日
function User:send_pur_rank_data(ctx, ranktype)
    local all_uid_str = ''
    local ret_msg = {
        type = ranktype,
        datas = {},
    }
    --插入我自己的数据
    local myself = {
        acc = self.account,
        score = self.pur_highest_score,
        sugar = 0,
        imageid = self.header_imgid,
        sex = self.sex,
    }
    table.insert(ret_msg.datas, myself)

    local today = os.date("%y%m%d")
    local key = "rankpur:" .. today
    if ranktype == 1 then--昨日排行榜
        local now = os.time()
        key = "rankpur:" .. os.date("%y%m%d", now - 86400)
    end
	c_redis.command("zrevrange", key, {0, 99, "withscores"}
        , function(reply)
            if 2 == reply.type and #reply.elements > 0 and #reply.elements % 2 == 0 then
                local uid2score = {}
                local order_uids = {}
                for i = 1, #reply.elements,2 do
                    local uidstr = reply.elements[i].str
                    uid2score[tonumber(uidstr)] = tonumber(reply.elements[i+1].str)
                    all_uid_str = all_uid_str .. uidstr .. ','
                    table.insert(order_uids, tonumber(uidstr))
                end

                -- 去 mongodb 批量查询数据
                all_uid_str = string.sub(all_uid_str, 1, #all_uid_str - 1)
                local mongodb_query_str = '{"uid":{"$in":[' .. all_uid_str .. ']}}'
                mdb:query_b(mdb.db, mdb.collection, mongodb_query_str, {account = 1, header_imgid=1,sex=1,uid=1,level=1 }, function(results)
                    local all_datas = {}
                    for i = 1,#results do
                        local user_info = mdb:get_table(results[i])
                        local data = {
                            acc = user_info['account'],
                            score = uid2score[user_info['uid']],
                            imageid = user_info['header_imgid'],
                            sex = user_info['sex'],
                            level = user_info['level'],
                            uid = user_info['uid'],
                        }
                        all_datas[user_info['uid']] = data
                    end

                    for i = 1,#order_uids do
                        local data = all_datas[order_uids[i]]
                        data.sugar = config.purgatory_rankreward[i],
                        table.insert(ret_msg.datas, data)
                    end
                    c_rpc.reply_to(ctx, pb.encode("plane.PurgatoryRankDataMsg", ret_msg))
                end)
            else
               self.log:error("zrange,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
               c_rpc.reply_to(ctx, pb.encode("plane.PurgatoryRankDataMsg", ret_msg))
            end
        end)
end

function User:send_season_reward(ctx)
    c_rpc.reply_to(ctx, config.season_level_reward_pbstr)
end

function User:buy_item(baseid, money, ticket, plastic)
    if self.can_op_money ~= 1 then
        self.log:info('%s,%s 企图在房间内购买道具id=%d', self.openid, self.account, baseid)
        return nil
    end
    local obj_base = objectbm[baseid]
    if obj_base == nil then
        log:info('%s,%s 企图创建不存在的道具id=%d', self.openid, self.account, baseid)
        return nil
    end

    if Object.is_skin(obj_base) then
        --检查是否已拥有该皮肤,如果拥有,则不能再次购买该皮肤
        if self.skinm:is_skinid_actived(obj_base.activeskinid) then
            self:popMessageBox('您已拥有该皮肤,无需再购买啦!')
            return
        end
    elseif Object.is_bulletskin(obj_base) then
        if self.skinm:is_bullet_owned(obj_base.baseid) then
            self:popMessageBox('您已拥有该子弹,无需再购买啦!')
            return
        end
    end

    if money ~= 0 and self.money < money then
        self:popMessageBox('您的饼干不足,快去战斗赚取更多的饼干吧!')
        return
    else
        self.money = self.money - money
        self.log:info("饼干消耗|消耗%d,购买 %d,%s,%d", money, obj_base.baseid, obj_base.name, obj_base.type)
    end

    if ticket ~= 0 and self.ticket < ticket then
        self:popMessageBox('您的糖果不足,快去战斗赚取更多的糖果吧!')
        return
    else
        self.ticket = self.ticket - ticket
        self.log:info("糖果消耗|消耗%d,购买 %d,%s,%d", ticket, obj_base.baseid, obj_base.name, obj_base.type)
    end

    if plastic ~= 0 and self.plastic < plastic then
        self:popMessageBox('您的塑料块不足,快去战斗赚取更多的塑料块吧!')
        return
    else
        self.plastic = self.plastic - plastic
    end
    mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {money = self.money, ticket = self.ticket, plastic = self.plastic})

    if Object.is_skin(obj_base) then
        self.skinm:active_skinid(obj_base.activeskinid)
    elseif Object.is_bulletskin(obj_base) then
        self.skinm:active_bullet(obj_base)
    else
        local obj = Object:new(baseid, 1)
        self.bagm:addObj(obj)
    end
    self:send_all_money_to_client()
    -- 检查道具id是否存在(在创建道具时候会检查)
    -- 分别检查对应的金钱需求是否满足
    -- 若满足,添加道具到玩家包裹,通知客户端添加道具,扣除金钱
end

-- 根据 'a=1 b=2 c=3' 返回3个键值对
function User:get_pairs(str)
    local tbl = {}
    -- 如果没空格,那就一个键值对,直接分隔
    local space_location = string.find(str, ' ')
    self.log:info('space location=%s', str)
    if space_location == nil then
        if insert_key_value_from_str(str, tbl) == false then
            return false
        end
    else
        while space_location ~= nil do
            if insert_key_value_from_str(string.sub(str, 1, space_location - 1), tbl) == false then
                return false
            end
            str = string.sub(str, space_location + 1)
            space_location = string.find(str, ' ')
        end

        if insert_key_value_from_str(str, tbl) == false then
            return false
        end
    end

    return tbl
end

function User:do_gm_cmd(ctx, cmd)
    if true then
        return
    end
        self.log:debug('run gm cmd=' .. cmd)
        if string.sub(cmd, 1, 5) == "fetch" then
            local tbl = self:get_pairs(string.sub(cmd, 7))
            self.log:info('get_pairs,ret=%s', tostring(tbl))
            if tbl == false or tbl['id'] == nil then
                self.log:info('%s,%s gm指令 %s 有误,无法执行', self.openid, self.account, cmd)
            else
                if tbl['num'] == nil then
                    tbl['num'] = 1
                end
                local obj = Object:new(tonumber(tbl['id']), tonumber(tbl['num']))
                if obj ~= nil then
                    self.bagm:addObj(obj)
                end
            end
        elseif string.sub(cmd, 1, 3) == "del" then
            local tbl = self:get_pairs(string.sub(cmd, 5))
            if tbl == false or tbl['id'] == nil then
                self.log:info('%s,%s gm指令 %s 有误,无法执行', self.openid, self.account, cmd)
            else
                self.bagm:remove_by_baseid(tonumber(tbl['id']))
            end
        elseif string.sub(cmd, 1, 3) == "pop" then
            local tbl = self:get_pairs(string.sub(cmd, 5))
            if tbl == false or tbl['str'] == nil then
                self.log:info('%s,%s gm指令 %s 有误,无法执行', self.openid, self.account, cmd)
            else
                self:popMessageBox(tbl['str'])
            end
        elseif string.sub(cmd, 1, 8) == "addmoney" then
            local tbl = self:get_pairs(string.sub(cmd, 10))
            local addnum = tbl['num']
            if addnum == nil then
                addnum = 1
            end

            self.money = self.money + math.floor(tonumber(addnum))
            mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {money = self.money})
        elseif string.sub(cmd, 1, 6) == "follow" then
            local tbl = self:get_pairs(string.sub(cmd, 8))
            local account = tbl['acc']
            self:follow(ctx, account)
        elseif string.sub(cmd, 1, 8) == "unfollow" then
            local tbl = self:get_pairs(string.sub(cmd, 10))
            local account = tbl['acc']
            self:unfollow(ctx, account)
        elseif string.sub(cmd, 1, 5) == "viewf" then
            self:view_follow(ctx, self.account, 1, 0)
        elseif string.sub(cmd, 1, 8) == "setmoney" then
            local tbl = self:get_pairs(string.sub(cmd, 10))
            local account = tbl['acc']
            mdb:update(mdb.db, mdb.collection, {account=account}, {money = tonumber(tbl['num'])})
        elseif string.sub(cmd, 1, 9) == "setticket" then
            local tbl = self:get_pairs(string.sub(cmd, 11))
            local account = tbl['acc']
            mdb:update(mdb.db, mdb.collection, {account=account}, {money = tonumber(tbl['num'])})
        elseif string.sub(cmd, 1, 6) == "setpla" then
            local tbl = self:get_pairs(string.sub(cmd, 8))
            local account = tbl['acc']
            mdb:update(mdb.db, mdb.collection, {account=account}, {money = tonumber(tbl['num'])})
        elseif string.sub(cmd, 1, 8) == "submoney" then
            local tbl = self:get_pairs(string.sub(cmd, 10))
            local subnum = tbl['num']
            if subnum == nil then
                subnum = 1
            end
            self.money = self.money - math.floor(tonumber(addnum))
            if self.money < 0 then
                self.money = 0
            end
            mdb:update(mdb.db, mdb.collection, {_id=self.openid}, {money = self.money})
        else
        end
end

return User
