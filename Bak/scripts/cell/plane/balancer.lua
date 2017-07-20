-- balancer.lua
-- 小飞机负载均衡器

local M = {}

local log = require("log"):new(...)
local rr = require("remote_run.remote_runner")
local router = require("rpc.base_rpc_router")
local pb = require("protobuf")
local room_mgr = require("plane.room_mgr")
local user_mgr = require("user_mgr")

local timer_queue = c_timer_queue.CTimerQueue()
local my_svr_id = c_util.get_my_svr_id()
local svr_infos = {}  -- 各服压力信息 svr_infos[svr_id] = stress

local min_stress_svr_id = my_svr_id

-- 获取房间服务器ID.
-- 暂时实现为随机。可以按服务器负载来选择。
local function get_room_svr_id()
    if c_util.is_valid_svr_id(min_stress_svr_id) == false then
        min_stress_svr_id = c_util.get_rand_svr_id()
    end
    return min_stress_svr_id
end  -- get_room_svr_id()

local function assign_room_2(ctx, svr_id, result)
    assert("table" == type(result))
    log:debug("assign_room_2()")
    -- 向直连服设置rpc路由, 然后再应答
    local game_clt_id = ctx:get_game_clt_id()
    local user = user_mgr.get_user(game_clt_id.base_rpc_clt_id)
    if user ~= nil then
        user.can_op_money = 0
        c_redis.set("room:" .. user.account, svr_id,function(reply_type)
            if reply_type ~= true then
                self.log:error("%s,%d,%s offline, set login key=0 failed", user.openid, user.uid, user.account)
            end
        end)
        router.set_svc_dst_svr_id(game_clt_id, "plane.Plane", svr_id, function()
            local resp_str = pb.encode("plane.EnterRoomResponse", result)
            c_rpc.reply_to(ctx, resp_str)
        end)
    end
end  -- on_assigned_room()

local function assign_room_1(ctx, account, nickname, skinid, bulletid)
    assert(account)
    assert("string" == type(nickname))

    -- 1. 向svr_id请求开房间
    -- 2. 设置直连服的rpc路由
    -- 3. 最后应答客户端
    local svr_id = assert(get_room_svr_id())
    local game_clt_id = ctx:get_game_clt_id()
    local base_svr_id = game_clt_id.base_svr_id
    local base_rpc_clt_id = game_clt_id.base_rpc_clt_id
    rr.run_mfa(svr_id, "plane.room_mgr", "enter_room",
        -- game_clt_id不是table, 不能直接传
        {base_svr_id, base_rpc_clt_id, account, nickname, skinid, bulletid},
        function(result) assign_room_2(ctx, svr_id, result) end)
end  -- on_got_acccount_and_nick()

function M.assign_room(ctx, nickname)
    log:debug("assign_room()")
    local game_clt_id = ctx:get_game_clt_id()
    local base_svr_id = game_clt_id.base_svr_id
    local base_rpc_clt_id = game_clt_id.base_rpc_clt_id
    rr.run_mfa(base_svr_id, "user_mgr", "get_account_and_nick", {base_rpc_clt_id},
        function(ok, account, skinid, bulletid)
            if ok then assign_room_1(ctx, account, nickname, skinid, bulletid) end
        end)
end  -- assign_room()

local function report_stress_info()

    local stress_info = {
        user_count = user_mgr.get_count(),
        full_room_count = room_mgr.get_full_room_count()
    }

    log:debug('report,update_stress,myid=%d,user_count=%d,roomc=%d', my_svr_id, stress_info.user_count, stress_info.full_room_count)
    local global_svr_id = c_util.get_function_svr_id("PlaneBalancer") -- 报告给负载均衡功能服
    rr.run_mfa(global_svr_id, "plane.balancer", "update_stress",
        {my_svr_id, stress_info}) -- 直连服只负责报告压力
end

function M.init()

    -- 每20s发送一次。
    timer_queue:insert_repeat_now(20000, report_stress_info)
end

function M.update_stress(svr_id, stress_info)
    svr_infos[svr_id] = stress_info
    
    
    if svr_id ~= min_stress_svr_id then
        local min_stress_info = svr_infos[min_stress_svr_id]
        if min_stress_info ~= nil and stress_info.full_room_count < min_stress_info.full_room_count then
            min_stress_svr_id = svr_id
        end
    end
	log:debug("balancer:update_stress, svr_id:"..svr_id.." UserCnt:"..stress_info.user_count.." RoomCnt:"..stress_info.full_room_count .. 'min_stress=' .. min_stress_svr_id)
    return min_stress_svr_id
end  -- update_stress()

return M
