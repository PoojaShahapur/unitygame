-- test_cats_and_dogs.lua

local M = {}

local pb = require("protobuf")
local user = require("user")
local inspect = require("inspect")
local log = require("log"):new(...)
local cur_x = 0
local cur_y = 0
local cur_angle = -90
local bullet_team_id = 0

local MAX_X = 200
local MAX_Y = 100

M.my_id = nil

local empty_msg_str = pb.encode("rpc.EmptyMsg", { })

local function login(account)
	local req = {account = account, password = "password"}
	local req_str = pb.encode("rpc.LoginRequest", req)
	user.rpc_request("rpc.Login", "Login", req_str)
end  -- login()

local function start_to_move()
	local req = { is_stop= false, angle = 0}
	local req_str = pb.encode("plane.MoveInfo", req)
	user.rpc_request("plane.Plane", "StopMove", req_str)
end  -- start_to_move()

local function on_enter_room(resp_str)
	local resp = pb.decode("plane.EnterRoomResponse", resp_str)
	log:debug("Entered room. My ID is %u. Room has %u players."..
		" Room timestamp is %ums.", resp.ms_and_id.id, #resp.players,
		resp.ms_and_id.ms)
	M.my_id = resp.ms_and_id.id
	start_to_move()  -- 开始时是停止不动的
	c_plane.set_active(true)
end  -- on_enter_room()

local function enter_room()
    local enter = {nickname="aaa", mode=0}
    local req_str = pb.encode("plane.EnterRoomMsg", enter)
	user.rpc_request("plane.Plane", "EnterRoom",
		req_str, on_enter_room)
end  -- enter_room()

function M.start_move()
    start_to_move()
end

function M.test(account)
	account = account or "DefaultAccount"
	login(account)
	enter_room()
end  -- test()

function M.fire()
	log:debug("Fire...")
	user.rpc_request("plane.Plane", "Fire", empty_msg_str)
end  -- fire()

function M.turn_to(angle)
	log:debug("Turn to angle: " .. angle)
	local msg = { angle = angle }
	local msg_str = pb.encode("plane.TurnToMsg", msg)
	user.rpc_request("plane.Plane", "TurnTo", msg_str)
end  -- turn_to()

function M.run(account)
	account = account or "test"..math.random(1,999999)
	login(account)
	enter_room()
end  -- run()

-- 移动同步消息, req = MoveToMsg()
function M.on_move_to(move_to_msg)
	local angle = move_to_msg.angle
	local x = move_to_msg.x
	local y = move_to_msg.y
	log:debug("on_move_to: (%f, %f) %f", x, y, angle)
	assert(angle >= 0 and angle <= 360, "Illegal angle: " .. angle)
	if (x < 2 and angle <= 180) or
		(x > MAX_X - 2 and angle >= 180) then
		c_plane.set_reaching_left_or_right()
		log:debug("Reaching left/right!")
	end
	if (y < 2 and angle <= 270 and angle >= 90) or
		(y > MAX_Y - 2 and (angle >= 270 or angle <= 90)) then
		c_plane.set_reaching_top_or_bottom()
		log:debug("Reaching top/bottom!")
	end
end  -- on_move_to()

return M
