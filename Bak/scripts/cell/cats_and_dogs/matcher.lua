-- 猫狗大战匹配器
-- 与战斗不是同一个Cell服。

local M = {}

local log = require("log"):new("cats_and_dogs.matcher")
local timer_queue = c_timer_queue.CTimerQueue()
local base_rpc_router = require("rpc.base_rpc_router")
local remote_runner = require("remote_run.remote_runner")
local my_svr_id = c_util.get_my_svr_id()
assert(my_svr_id)

-- 第一个请求者game_clt_id，nil表示无
local first_one
-- 机器人匹配定时器ID
local timer_id

-- 选择一个Cell开房间
local function match_player(second_one)
	assert(first_one)
	assert(second_one)
	local cell_svr_id = c_util.get_rand_svr_id();
	log:debug("match_player to cell_%u", cell_svr_id)
	local SVC = "rpc.CatsAndDogs"
	base_rpc_router.set_svc_dst_svr_id(first_one, SVC, cell_svr_id)
	base_rpc_router.set_svc_dst_svr_id(second_one, SVC, cell_svr_id)

	-- 让Cell开始战斗, Base 发送帐号名到战斗服, 需要根据对手 game_clt_id 来查找房间。
	local codes_fmt = [[
require("cats_and_dogs.on_matched").on_matched(%u, %u, %u, %u)]]
	remote_runner.run(first_one.base_svr_id, string.format(codes_fmt,
		first_one.base_rpc_clt_id, cell_svr_id,
		second_one.base_svr_id, second_one.base_rpc_clt_id))
	remote_runner.run(second_one.base_svr_id, string.format(codes_fmt,
		second_one.base_rpc_clt_id, cell_svr_id,
		first_one.base_svr_id, first_one.base_rpc_clt_id))
	first_one = nil
end  -- match_player()

-- 匹配机器人
local function match_robot()
	assert(first_one)
	log:debug("match_robot")
	local game_clt_id = c_game_clt_id.CGameCltId(my_svr_id, 0)
	assert(game_clt_id:is_local())
	match_player(game_clt_id)
end

-- 请求匹配
-- 注意： 匹配服与战斗服不是同一服，只是都开了这些服务。
function M.enter(game_clt_id)
	assert(game_clt_id)
	if not first_one then
		first_one = game_clt_id
		timer_id = timer_queue:insert_single_from_now(10.0, match_robot)
		log:debug("Enter Match: clt.%s", first_one:to_string())
		return
	end
	if first_one:equals(game_clt_id) then
		return
	end

	timer_queue:erase(timer_id)
	timer_id = nil
	match_player(game_clt_id)
end  -- enter()

-- 退出匹配
function M.exit(game_clt_id)
	assert(game_clt_id)
	log:debug("Exit match: clt.%s", game_clt_id:to_string())
	if not first_one then return end
	if game_clt_id:equals(first_one) then
		first_one = nil
	end
end  -- exit()

return M
