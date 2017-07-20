local Player = {}
--[[
Player = {
	game_clt_id = CGameCltId
	account = "",
	is_robot = boolean
	log = log:new("player." .. account)

	end_of_turn = false,  -- 本回合结束，即已经攻击结束
	hp = 0,  -- 血量，为0时判为失败
}
--]]

local Log = require("log")

function Player:new(game_clt_id, account)
	assert(game_clt_id)
	local account2 = account or "Robot"
	local INIT_HP = 3
	local player = {
		game_clt_id = game_clt_id,
		account = account2,
		is_robot = not account,
		log = Log:new("cats_and_dogs.player." .. account2),
		hp = INIT_HP,
	}
	player.log:debug("New player. account=%s", account)

	setmetatable(player, self)
	self.__index = self
	return player
end  -- new()

function Player:is_valid_player()
	return true
end

function Player:rpc_request(service_name, method_name, request_str, callback)
	assert("string" == type(service_name))
	assert("string" == type(method_name))
	assert("string" == type(request_str))
	assert(not callback or "function" == type(callback))
	if self.is_robot then return end
	self.log:debug("rpc request: %s.%s", service_name, method_name)
	c_rpc.request_clt(self.game_clt_id, service_name, method_name,
		request_str, callback)
end  -- rpc_request()

return Player
