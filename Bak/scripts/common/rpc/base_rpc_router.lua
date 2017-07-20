--- Cell 服设置 Base 服的 rpc 路由.
-- 应用 `c_remote_rpc_router_modifier` 实现。
-- @module rpc.base_rpc_router

local M = {}

local c = require("c_remote_rpc_router_modifier")

function M.reset_svc_dst_svr_id(game_clt_id, service)
	local modifier = c.CRemoteRpcRouterModifier(game_clt_id.base_svr_id)
	modifier:reset_svc_dst_svr_id(game_clt_id.base_rpc_clt_id, service)
end

function M.set_svc_dst_svr_id(game_clt_id, service, dst_svr_id, on_result)
	local modifier = c.CRemoteRpcRouterModifier(game_clt_id.base_svr_id)
	modifier:set_svc_dst_svr_id(game_clt_id.base_rpc_clt_id, service, dst_svr_id, on_result)
end  -- set_svc_dst_svr_id()

function M.set_mthd_dst_svr_id(game_clt_id, service, method, dst_svr_id, on_result)
	local modifier = c.CRemoteRpcRouterModifier(game_clt_id.base_svr_id)
	modifier:set_mthd_dst_svr_id(game_clt_id.base_rpc_clt_id,
		service, method, dst_svr_id, on_result)
end  -- set_mthd_dst_svr_id()

return M
