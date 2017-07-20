-- on_disconnected.lua
-- 小飞机下线处理

local M = {}

local log = require("log"):new(...)
local rr = require("remote_run.remote_runner")
local c_rpc_router = require("c_rpc_router")
local user_mgr = require("user_mgr")

function M.on_disconnected(rpc_clt_id)
    -- 查找房间服务器，注意不能用"EnterRoom".
    local room_svr_id = c_rpc_router.get_dst_svr_id(rpc_clt_id,
        "plane.Plane", "Fire")
    log:debug("on_disconnected: %d,roomsvrid=%d",rpc_clt_id, room_svr_id)
    if 0 == room_svr_id then return end  -- Todo: 有可能在设置路由之前断线

    -- 向房间服发送断线事件
    local my_svr_id = c_util.get_my_svr_id()
    rr.run_mfa(room_svr_id, "plane.room_mgr", "on_disconnected",
        { my_svr_id, rpc_clt_id })

    -- 重置RPC路由
    c_rpc_router.reset_svc_dst_svr_id(rpc_clt_id, "plane.Plane")
    -- 设置可以用钱了（需要吗？）
    user_mgr.set_user_can_op_money(rpc_clt_id)
end  -- on_disconnected()

return M
