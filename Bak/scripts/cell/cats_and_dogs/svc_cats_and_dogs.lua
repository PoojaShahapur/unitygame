local M = {}

local log = require("log"):new("cats_and_dogs.svc")
local pb = require("protobuf")
local empty_msg_data = pb.encode("rpc.EmptyMsg", {})
local matcher = require("cats_and_dogs.matcher")
local room_mgr = require("cats_and_dogs.room_mgr")

log:debug("loading service...")

function M.EnterMatch(ctx, content)
	log:debug("EnterMatch")
	c_rpc.reply_to(ctx, empty_msg_data)
	matcher.enter(ctx:get_game_clt_id())
end  -- EnterMatch()

function M.Exit(ctx, content)
	matcher.exit(ctx:get_game_clt_id())
	c_rpc.reply_to(ctx, empty_msg_data)
end  -- EnterMatch()

local function reply_attack(ctx, result)
	local resp = {result = result}
	local encoded = pb.encode("rpc.AttackResponse", resp)
	c_rpc.reply_to(ctx, encoded)
end

function M.Attack(ctx, content)
	log:debug("Attack")
	local req = assert(pb.decode("rpc.AttackRequest", content))

	local game_clt_id = ctx:get_game_clt_id()
	local room = room_mgr.get(game_clt_id)
	-- room 有可能空，因为这可能是死前发出的最后一击
	if room then
		local result = room:attack_from(game_clt_id, req.strength)
		reply_attack(ctx, result)
		room:check_turn()  -- 下一回合或者结束
	else
		reply_attack(ctx, "MISS")
	end
end  -- EnterMatch()

-- 虽然Rpc由Base转发，但仍按直连客户端那样实现服务。
require("rpc_request_handler").register_service("rpc.CatsAndDogs", M)
return M
