-- room_mgr.lua
-- 猫狗大战房间管理。
-- 房间建立双索引，任一方的game_clt_id都可以索引到。但不对base_svr_id=0的建索引。

local M = { rooms = {} }

local log = require("log"):new("cats_and_dogs.room_mgr")
local Room = require("cats_and_dogs.room")

-- 是否是机器人
local function is_robot(game_clt_id)
	-- 机器人的base_rpc_clt_id为非法值(0)
	return 0 == game_clt_id.base_rpc_clt_id
end

-- 用 game_clt_id:to_string() 为键
local function set_clt2room(game_clt_id, room)
	assert(game_clt_id)
	-- 机器人不保存索引(因为机器人都是相同的索引)
	if not is_robot(game_clt_id) then
		M.rooms[game_clt_id:to_string()] = room
	end
end

local function set_kv(key_room, val_room)
	assert(key_room)
	local player_1 = key_room.player_1
	assert(player_1)
	set_clt2room(player_1.game_clt_id, val_room)

	local player_2 = key_room.player_2
	if player_2 then
		set_clt2room(player_2.game_clt_id, val_room)
	end
end

function M.insert(room)
	set_kv(room, room)
end

function M.erase(room)
	set_kv(room, nil)
end

function M.get(game_clt_id)
	assert(game_clt_id.base_svr_id)
	if is_robot(game_clt_id) then
		return nil  -- 机器人无房间，必须按玩家查找
	end
	return M.rooms[game_clt_id:to_string()]
end

-- my_account 为空表示已下线，使用机器人
local function enter_one(peer_game_clt_id, my_game_clt_id, my_account)
	assert(peer_game_clt_id)
	assert(my_game_clt_id)
	log:debug("enter_one: peer(%s), me(%s, %s)",
		peer_game_clt_id:to_string(), my_game_clt_id:to_string(), my_account)

	-- 房间可能已由对方创建，可由任一方的game_clt_id检索到(机器人检索返回nil)
	local room = M.get(peer_game_clt_id) or M.get(my_game_clt_id)
	if room then
		if (not room:enter_second(my_game_clt_id, my_account)) then
			M.erase(room)  -- 2个机器人，取消战斗
			return
		end
	else
		-- 用双方的game_clt_id创建房间，并加入第1人. 如果对手为机器人，则立即开始对战。
		room = Room:new(peer_game_clt_id, my_game_clt_id, my_account)
	end
	-- 加人后须重新加入房间管理
	M.insert(room)
end  -- enter_one()

-- 坐等对方，由base服调用， my_account 为空表示已下线，使用机器人
function M.enter(peer_base_svr_id, peer_base_rpc_clt_id,
				my_base_svr_id, my_base_rpc_clt_id, my_account)
	assert(0 ~= my_base_svr_id)
	peer_game_clt_id = c_game_clt_id.CGameCltId(
		peer_base_svr_id, peer_base_rpc_clt_id)
	my_game_clt_id = c_game_clt_id.CGameCltId(
		my_base_svr_id, my_base_rpc_clt_id)
	enter_one(peer_game_clt_id, my_game_clt_id, my_account)
end

return M
