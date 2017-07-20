-- svc_hall.lua
-- 玩家大厅，作为服务，负责服务器所有房间管理,分配。

--[[
HALL = {
    rooms = {},
}
--]]

local HALL = { }

local log = require("log"):new("plane.hall")
local Room = require("plane.room")
-- 房间以game_clt_id为键
local rooms = {}

local room = Room:new()

function HALL.find_room(room_key)
	-- 查找一个房间，如果找不到，就创建一个新的
	local room = rooms[room_key]
    if room then
		return room
	end
	room = Room:new()
	return room
end

-- 进入房间
function HALL.enter_room(room_key)
	room_key = room_key:to_string()
	room = HALL.find_room(room_key)
	log:debug(" HALL.enter_room, roomkey=%s, room=%s", room_key, room )
	rooms[room_key] = room
    return room
end

--退出房间
function HALL.leave_room(room_key)

end

-- 获取房间
function HALL.get(room_key)
    return rooms[room_key:to_string()]
end

-- 获取房间列表
function HALL.HallList()
	local response = {}
	if not rooms then
		for k, room in pairs(self.rooms) do
			table.insert(response, k, room.get_room_left_can_in_num())
		end
	end
end

require("rpc_request_handler").register_service("hall.Hall", HALL)
return HALL
