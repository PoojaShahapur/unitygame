--[[
CatsAndDogsRoom class.
猫狗大战房间。处理对战。
--]]

local CatsAndDogsRoom = {
	type = "CatsAndDogsRoom",
}

--[[
CatsAndDogsRoom 数据结构
{
	type = "CatsAndDogsRoom",
	id = Integer,
	log = Log

	player_1 = Player
	player_2 = Player

	wind_strength = 0,  -- 本回合风力: -100..100, 以1->2方向为正
	next_turn_timer_id = 0,  -- 触发下一回合的定时器
}
--]]

local Log = require("log")
local random = math.random
local pb = require("protobuf")
local timer_queue = c_timer_queue.CTimerQueue()
local Player = require("cats_and_dogs.player")

local TURN_SEC = 10.0  -- 一个回合秒数，超时即进入下一回合

local gen_room_id = 0
local function generate_room_id()
	gen_room_id = gen_room_id + 1
	return gen_room_id
end

-- 由我创建房间，我可能已下线，用机器人代表。my_account为空表示机器人
function CatsAndDogsRoom:new(peer_game_clt_id, my_game_clt_id, my_account)
	local id = generate_room_id()
	local room = {
		type = "CatsAndDogsRoom",
		id = id,
		log = Log:new("cats_and_dogs.room." .. id),
	}

	setmetatable(room, self)
	self.__index = self

	room.log:debug("Create room.")
	room:enter_first(my_game_clt_id, my_account)
	room:wait_second(peer_game_clt_id)
	return room
end  -- new()

local function send_attacked_msg_to(player, strength, is_hit)
	assert("number" == type(strength))
	if player.is_robot then return end  -- robot
	local result = "MISS"
	if is_hit then result = "HIT" end
	local req = {strength = strength, result = result }
	player:rpc_request("rpc.CatsAndDogsPush", "GotAttack",
		pb.encode("rpc.GotAttackMsg", req))
end

-- 开始新回合，并设置10s开始下一回合
function CatsAndDogsRoom:new_turn()
	self.log:debug("new_turn")
	local player_1 = self.player_1
	local player_2 = self.player_2
	assert(player_1 and player_2)

	local wind_strength = math.random(-100, 100)
	self.wind_strength = wind_strength

	player_1.end_of_turn = false
	player_2.end_of_turn = false
	local req_str = pb.encode("rpc.NewTurnMsg", {wind_strength = wind_strength})
	player_1:rpc_request("rpc.CatsAndDogsPush", "NewTurn", req_str)
	if player_2.is_robot then
		timer_queue:insert_single_from_now(math.random() * TURN_SEC * 9/10,
			function() self:robot_attack() end)
	else
		player_2:rpc_request("rpc.CatsAndDogsPush", "NewTurn", req_str)
	end

	-- 10s后开始下一回合
	timer_queue:erase(self.next_turn_timer_id)
	self.next_turn_timer_id = timer_queue:insert_single_from_now(TURN_SEC,
		function() self:new_turn() end)
end

-- 是否打中，投掷力strength: 0..100, 风力wind_strength: -100..100 (顺风为正)
local function get_is_hit(strength, wind_strength)
	local diff = math.abs(strength * 2 + wind_strength - 100)
	return math.random(0, diff) < 10
end

-- 返回是否击中, 攻击者hp为0时总是不中
local function attack_from_to(attacker, defender, strength, wind_strength, log)
	log:debug("attack from to : Attacker: %s , defender: %s",
		attacker.account, defender.account)
	attacker.end_of_turn = true
	is_hit = attacker.hp > 0 and get_is_hit(strength, wind_strength)
	if is_hit then defender.hp = defender.hp - 1 end
	send_attacked_msg_to(defender, strength, is_hit)
	return is_hit
end  -- attack_from_to()

-- return AttackResponse.AttackResult
function CatsAndDogsRoom:attack_from(attacker_clt_id, strength)
	assert("number" == type(strength))
	local player_1 = self.player_1
	local player_2 = self.player_2
	local is_hit = false
	if attacker_clt_id:equals(player_1.game_clt_id) then
		if player_1.end_of_turn then return "WRONG_TURN" end
		is_hit = attack_from_to(player_1, player_2, strength,
			self.wind_strength, self.log)
	elseif attacker_clt_id:equals(player_2.game_clt_id) then
		if player_2.end_of_turn then return "WRONG_TURN" end
		is_hit = attack_from_to(player_2, player_1, strength,
			-self.wind_strength, self.log)
	else
		assert(false, "Attacker is illegal. " .. attacker_clt_id.to_string())
	end

	if is_hit then return "HIT" end
	return "MISS"
end  -- attack_from()

-- 检查本回合，进入下一回合或结束。
function CatsAndDogsRoom:check_turn()
	if self:check_win() then return end
	if not self.player_1.end_of_turn then return end
	if not self.player_2.end_of_turn then return end
	self:new_turn()  -- 开始下一回合
end  -- check_turn()

local function send_result(player, is_win)
	assert(player)
	assert("boolean" == type(is_win))
	player:rpc_request("rpc.CatsAndDogsPush", "SetResult",
		pb.encode("rpc.CatsAndGogsResult", {win = is_win}))
end

-- 检查胜负，胜负已定则返回true
function CatsAndDogsRoom:check_win()
	local player_1 = self.player_1
	local player_2 = self.player_2
	if player_1.hp > 0 and player_2.hp > 0 then
		return false
	end

	self.log:debug("End.")
	send_result(player_1, player_1.hp > 0)
	send_result(player_2, player_2.hp > 0)

	timer_queue:erase(self.next_turn_timer_id)
	return true
end  -- check_win()

-- 返回机器人投掷力。分段越细，机器人就越准。
local function get_robot_strength(wind_strength)
	local perfect = (100 + wind_strength) / 2
	if perfect < 30 then return math.random(30) end
	if perfect < 60 then return math.random(30, 60) end
	return math.random(60, 100)
end

function CatsAndDogsRoom:robot_attack()
	local wind = self.wind_strength
	local strength = get_robot_strength(wind)
	self.log:debug("robot_attack, wind=%d, strength=%d", wind, strength)
	attack_from_to(self.player_2, self.player_1, strength, wind, self.log)
	self:check_turn()
end  -- robot_attack()

-- 加入第1方
function CatsAndDogsRoom:enter_first(game_clt_id, account)
	assert(game_clt_id)
	assert(not self.player_1)
	self.player_1 = Player:new(game_clt_id, account)
end

-- 等待第2方。如果第2方是机器人，则立即开始战斗。
function CatsAndDogsRoom:wait_second(game_clt_id)
	assert(game_clt_id)
	assert(self.player_1)
	if 0 == game_clt_id.base_rpc_clt_id then  -- 非真实玩家
		assert(self:enter_second(game_clt_id))  -- 立即开始
	end
end

-- 加入第2方。
-- 如果双方都为robot, 则返回false
-- 加人后须重新加入房间管理
function CatsAndDogsRoom:enter_second(game_clt_id, account)
	local player_1 = self.player_1
	local player_2 = Player:new(game_clt_id, account)
	self.player_2 = player_2

	if player_1.is_robot then
		if player_2.is_robot then
			return false
		end
		-- 总是让p2为机器人，方便写代码
		self.player_1, self.player_2 = player_2, player_1
	end

	self.log:debug("match_player")
	local req_to_1 = { peer_name = player_2.account }
	local req_to_2 = { peer_name = player_1.account }
	player_1:rpc_request("rpc.CatsAndDogsPush", "EnterRoom",
		pb.encode("rpc.EnterRoomMsg", req_to_1))
	player_2:rpc_request("rpc.CatsAndDogsPush", "EnterRoom",
		pb.encode("rpc.EnterRoomMsg", req_to_2))

	self:new_turn()  -- 开始回合
	return true
end

return CatsAndDogsRoom
