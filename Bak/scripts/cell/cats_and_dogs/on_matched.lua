--[[
Base服上当匹配完成时，由匹配服调用。
--]]

local M = {}

local remote_runner = require("remote_run.remote_runner")
local user_mgr = require("user_mgr")
local my_svr_id = c_util.get_my_svr_id()

function M.on_matched(rpc_clt_id, cell_svr_id, peer_base_svr_id, peer_base_rpc_clt_id)
	local account = user_mgr.get_user_account(rpc_clt_id)
	if account then
		remote_runner.run(cell_svr_id, string.format(
			[[require("cats_and_dogs.room_mgr").enter(%u, %u, %u, %u, %q)]],
			peer_base_svr_id, peer_base_rpc_clt_id,
			my_svr_id, rpc_clt_id, account))
		return
	end

	-- 已退出则用机器人. account 为空表示我是机器人。
	-- 对手enter()时需用我的game_clt_id来找到房间。
	if 0 == peer_base_svr_id then
		return  -- 对手也是机器人，取消战斗
	end
	remote_runner.run(cell_svr_id, string.format(
		[[require('cats_and_dogs.room_mgr').enter(%u, %u, %u, %u)]],
		peer_base_svr_id, peer_base_rpc_clt_id,
		my_svr_id, rpc_clt_id))
end

return M
