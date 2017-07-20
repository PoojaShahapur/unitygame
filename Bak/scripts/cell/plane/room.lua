-- room.lua
-- 飞机对战房间。处理对战。
local Room = {
    type = "PlaneRoom",

}

--[[
Room 数据结构
{
    type = "PlaneRoom",
    players = { [game_clt_id_str] = Player() },
    players_count = 0,

    foods = { [food_id] = { x, y } },
    gen_food_id = 0,
}
--]]

local log = require("log"):new("plane.room")
local pb = require("protobuf")
local Player = require("plane.player")
local AIPlayer = require("plane.aiplayer")
local config = require("config")
local Rankmgr = require("plane.rankmgr")
local BulletGroup = require("plane.bullet_group")

local cur_room_id = 0

local math_pow = math.pow
local math_sqrt = math.sqrt
local math_floor = math.floor
local math_random = math.random
local table_insert = table.insert
local table_remove = table.remove
local pow2 = function(x,v) return (x*x) end

-- temp
local count = 1
local player_id = 0

function generate_player_id()
    player_id = player_id + 1
    log:debug('generate player id=%d', player_id)
    return player_id
end

function get_plane_id()
	count = count +1
	return count
end

function Room:get_cd_by_plane_num(plane_num)
    local new_cd = config.shot_cd.min_seconds + config.shot_cd.add_coef * plane_num
    if new_cd > config.shot_cd.max_seconds then
        new_cd = config.shot_cd.max_seconds
    end

    return new_cd
end

function get_born_pos()
    local pos = {
        x = math_random(config.x_min + 5, config.x_max - 5),
        y = math_random(config.y_min + 5, config.y_max - 5)
    }

    return pos
end

-- room同步所有玩家移动
function Room:sync_pos(interval)
	--log:info("sync_pos = %s", interval/1000)
	local bc_msg = {
		 curframe_and_roomid = { ms = self.cur_frame, id = self.room_id },
		 moves = {},
	}
    local bc_msg_moves_count = 0
    local t = c_util.get_ms()
	for k, player in pairs(self.players) do
        if player.is_stop == false or player.split_last_timer_id ~= 0 then--如果玩家再动
    		-- speed是速度，临时的
			--log:debug("player" .. player.game_clt_id:to_string() .. ".is_stop = " .. tostring(player.is_stop) .. "moving,x=" .. player.pos.x .. ",y=" .. player.pos.y .. ',movespeed=' .. player.move_speed)

    		local info = {
    			ms_and_id = {ms = self.cur_frame, id = player.id},
    			move_to =   {
			        angle = math_floor(player.angle * 100),
                    score = player.score,
			        x = math_floor(player.pos.x * 100),
        			y = math_floor(player.pos.y * 100),
					small_plane_ids = { }
    			}
    		}
            --log:debug('send pos to client, %d,%s,x,y=(%f,%f)', player.id, player.nickname, player.pos.x, player.pos.y)

	    	table_insert(bc_msg.moves, info)
            bc_msg_moves_count = bc_msg_moves_count + 1
        -- 2个条件不能放在一起,否则就把静止的客户端给动起来了
        end
	end
    if bc_msg_moves_count >= 1 then
	    self:broadcast("PackPlayerMoveTo", "plane.MoveToBcMsgRoom", bc_msg)
    end
    self.sysnc_time = c_util.get_ms()
end

-- 创建房间
function Room:new(roomid, destroy_handler)
    cur_room_id = roomid
    log:debug("Create room:"..cur_room_id)
    -- 一定确保只有一个定时器挂着
    --assert(config.split_cd > config.split_last_seconds)

    local room = {
        type = "PlaneRoom",
		timer_queue = c_timer_queue.CTimerQueue(),
        players = {},
        players_count = 0,
        ai_players_count = 0,
        can_use_ai_names = {},
        foods = {},
        rankmgr = {},
        cur_food_id = 0,
		-- 当前帧
		cur_frame = 1,
        room_id = cur_room_id,
		-- room销毁时间
		destroytime = config.room.last_seconds *1000,
        createtime = c_util.get_sys_ms(),
        end_timestamp = os.time()*1000 + config.room.last_seconds *1000,
        lastTick = c_util.get_ms(),
        on_destroy_handler = destroy_handler,
        sysnc_time = c_util.get_ms(),
        player_update_time = c_util.get_ms(),
        bg_update_time = c_util.get_ms(),-- bullet group update time
        bullet_groups = {},
    }

    setmetatable(room, self)
    self.__index = self
    room:init_foods()

    room.frame_timer_id = room.timer_queue:insert_repeat_from_now(0, 33,
        function()
            local now = c_util.get_ms()
	        --log:info("now = %s, lastTick=%s", now, room.lastTick)
            room:calc_room_frame(now - room.lastTick) 
            room.lastTick = now
        end)
    room.destroy_timer_id = room.timer_queue:insert_single_from_now(
        room.destroytime, function () room:destroytime_over() end)
    room.rankmgr = Rankmgr:new(room)
    --代表该房间有没有通知过session人数
    room.already_tell_session = 0
    local random_names_tbl = c_csv.CsvTable("random_name.csv")
    assert(random_names_tbl)
    local random_names_rs = random_names_tbl:get_all_records()
    local random_name_index = math_random(1, #random_names_rs)
    local all_name_index_tbl = {}
    for i = 1,config.room.hold_num do
        if (random_name_index + i - 1) == #random_names_rs then
            table_insert(all_name_index_tbl, random_name_index + i - 1)
        else
            table_insert(all_name_index_tbl, (random_name_index + i - 1) % #random_names_rs)
        end
    end

    for i = 1, config.room.hold_num do
        local aiplayer = AIPlayer:new(room, random_names_rs[all_name_index_tbl[i]].name)
    end
    log:debug('add destroy_timer:'..room.destroy_timer_id)

    return room
end  -- new()

function Room:destroy()
    log:debug('destroy room:'..self.room_id)
    self.timer_queue:erase_all()
    self.rankmgr = nil
    self.bullet_groups = nil
    self.foods = nil
    self.can_use_ai_names = nil

    -- 重置RPC路由, 停止定时器
    for k, player in pairs(self.players) do
        player:on_room_destroy()
    end  -- for
end

function Room:destroytime_over()
    log:debug('time is over,room Id:'..self.room_id)
    self.rankmgr:send_result_data()
    self.destroy_timer_id = nil
    if self.on_destroy_handler ~= nil then
        self.on_destroy_handler(self.room_id)
    end
end

function Room:calc_room_frame(interval)
    -- 服务器每 20ms计算一次,100ms同步给客户端
    local delta_time = (c_util.get_ms() - self.player_update_time) / 1000
    for k, player in pairs(self.players) do
        player:frame_move(delta_time)
    end  -- for
    self.player_update_time = c_util.get_ms()
    delta_time = (c_util.get_ms() - self.bg_update_time) / 1000
    for k,bullet_group in pairs(self.bullet_groups) do
        bullet_group:frame_move(delta_time)
    end
    self.bg_update_time = c_util.get_ms()
    self:frame_check_hit()

    if self.cur_frame % 1 == 0 then
	    self:sync_pos(interval)
    end
    self.cur_frame = self.cur_frame + 1
end

-- 填充帧击中里面,打中能源
function Room:full_frame_hit_energy_info(tbl, index_tbl,bullet_groupid, one_hit_energy_info)
    self:check_frame_hit_bullet_group_exist(tbl, index_tbl, bullet_groupid)
    table_insert(tbl[bullet_groupid].energyhits, one_hit_energy_info)
end

-- 填充帧击中里面,打中飞机
function Room:full_frame_hit_plane_info(tbl, index_tbl, bullet_groupid, one_hit_plane_info)
    self:check_frame_hit_bullet_group_exist(tbl, index_tbl,bullet_groupid)
    table_insert(tbl[bullet_groupid].planehits, one_hit_plane_info)
end

-- 检查tbl里面,该子弹组是否存在,不存在则创建;存在则不操作
function Room:check_frame_hit_bullet_group_exist(tbl, index_tbl, bullet_group_id)
    if tbl[bullet_group_id] == nil then
        tbl[bullet_group_id] = {energyhits = {}, planehits = {}}
        table_insert(index_tbl, bullet_group_id)
    end
end

-- 每帧遍历所有玩家和子弹检测
function Room:frame_check_hit()
    -- 广播给客户端的,批量添加新能源的消息
    local batch_add_food_bc = {
        foods = {},
    }
    -- 广播给客户端,该帧的击中信息
    local frame_hit_bc = {
        hits = {},
    }
    -- 所有子弹组的击中信息,key为子弹组的id,value为一个table,包含所有击中能源信息和击中小飞机信息
    local all_bullet_group_hit_info = {}
    local all_bullet_group_ids = {}
    -- 遍历每个玩家,看玩家的机群圆 和 子弹群圆 是否相交
    for k1, bullet_group in pairs(self.bullet_groups) do
        for k, player in pairs(self.players) do
            -- 子弹组不能打自己,不能打无敌的玩家
            if bullet_group.shoot_player ~= player and player.is_god ~= true and player:is_dead() ~= true then
                local central_distance = math_sqrt(pow2(bullet_group.central_pos.x-player.pos.x, 2) 
                + pow2(bullet_group.central_pos.y-player.pos.y, 2))
                -- debug open
                --log:debug('check_hit, %d,%s to bullet group id=%d,dis=%f, player radius=%f,bg radius=%f'
                --    ,player.id, player.nickname, bullet_group.id, central_distance,player.wrap_circle_radius, bullet_group.radius)
                if central_distance < (player.wrap_circle_radius + bullet_group.radius) then
                    -- 开始筛选可能会碰撞的小飞机,小飞机到子弹群群心距离>子弹群半径,或者子弹到小飞机群心距离大于小飞机机群半径
                    local possible_planes = {}
                    local possible_bullets = {}
                    for _, small_plane in pairs(player.small_plane) do
                        local plane_world_pos = small_plane:get_world_pos()
                        local plane_to_bullet_center_dis = math_sqrt(pow2(bullet_group.central_pos.x-plane_world_pos.x, 2)
                            + pow2(bullet_group.central_pos.y-plane_world_pos.y, 2))
                        if plane_to_bullet_center_dis < (config.modle_radius + bullet_group.radius) then
                            table_insert(possible_planes, small_plane)
                        end
                    end
    
                    for _, bullet in pairs(bullet_group.all_bullets) do
                        local bullet_world_pos = {x=bullet.local_pos.x+bullet_group.central_pos.x, y = bullet.local_pos.y + bullet_group.central_pos.y}
                        local bullet_to_plane_center_dis = math_sqrt(pow2(player.pos.x - bullet_world_pos.x, 2) 
                            + pow2(player.pos.y - bullet_world_pos.y, 2))
                        if bullet_to_plane_center_dis < (player.wrap_circle_radius + config.modle_radius) then
                            table_insert(possible_bullets, bullet)
                        end
                    end
    
                    -- 遍历所有的可能碰撞的飞机和子弹,发生碰撞
                    local bullet_hit_plane_pairs = {}
                    for _, bullet in pairs(possible_bullets) do
                        local be_hit_plane_index = 0
                        local bullet_world_pos = {x=bullet.local_pos.x+bullet_group.central_pos.x, y = bullet.local_pos.y + bullet_group.central_pos.y}
                        for index, plane in pairs(possible_planes) do
                            local plane_world_pos = plane:get_world_pos()
                            local dis = math_sqrt(pow2(bullet_world_pos.x-plane_world_pos.x, 2) + pow2(bullet_world_pos.y - plane_world_pos.y, 2))
                            if dis < (config.modle_radius + config.modle_radius) then
                                be_hit_plane_index = index
                                table_insert(bullet_hit_plane_pairs, {bullet=bullet, plane=plane})
                                break
                            end
                        end
                        if be_hit_plane_index ~= 0 then
                            table_remove(possible_planes, be_hit_plane_index)
                        end
                    end
    
                    -- 发消息告诉客户端,bulletid 击中 planeid 啦
                    for _, hitinfo in pairs(bullet_hit_plane_pairs) do
                        player:delete_small_plane(hitinfo.plane.id)
                        local one_hit_plane_info = {
                            bulletid = hitinfo.bullet.correspond_plane_id,
                            hit_playerid = player.id,
                            hit_planeid = hitinfo.plane.id,
                            ownerspeed = player.split_last_timer_id ~= 0 and player.split_move_speed or player.move_speed,
                            attackerid = 0,
                            addplaneid = 0,
                            speed = 0,
                        }
                        one_hit_plane_info.ownerspeed = math_floor(one_hit_plane_info.ownerspeed * 100)
                        local attacker = bullet_group.shoot_player
                        attacker.score = attacker.score + 100
                        -- 如果发子弹的玩家已经死亡,则不添加
                        if attacker:is_dead() ~= true then
                            attacker.killnum = attacker.killnum + 1
                            attacker.total_killnum = attacker.total_killnum + 1
                            if attacker.killnum % config.need_shot_num == 0 and attacker:can_add_plane()==true then
                    	    -- 射击的人增加小飞机
                        	    local newplaneid = attacker:add_small_plane(false)
                                if newplaneid ~= nil then
                                    one_hit_plane_info.attackerid = attacker.id
                                    one_hit_plane_info.addplaneid = newplaneid
                                    local tempspeed = attacker.split_last_timer_id ~= 0 and attacker.split_move_speed or attacker.move_speed
                                    one_hit_plane_info.speed = math_floor(tempspeed * 100)
                                end
                            end
                        end
                        bullet_group:remove_bullet_by_id(hitinfo.bullet.id)
                        if player:is_dead() == true then 
                            player:on_die()
                            attacker:on_kill_other()
                            --log:info("%d,%s,score=%d,is killed by %d,%s", player.id,player.nickname, player.score,attacker.id, attacker.nickname)
                            player.timer_queue:insert_single_from_now(math_floor(config.relive_seconds * 1000), 
                            function() player:notify_death(attacker.nickname) end)
                            if player.score ~= 0 then
                                local minus_score = math_floor((player.score + 1) / 2)
                                player.score = player.score -  minus_score
                                self.rankmgr:on_player_minus_score(player)
                                attacker.score = attacker.score + minus_score
                            end
                        end
                        self.rankmgr:on_player_add_score(attacker)
                        --log:debug('bulletid=%d hit planeid=%d', hitinfo.bullet.id, hitinfo.plane.id)
                        self:full_frame_hit_plane_info(all_bullet_group_hit_info, all_bullet_group_ids, bullet_group.id, one_hit_plane_info)
                    end
                end -- end if
            end
        end --end bullet_groups
    end -- end self.players

    -- 遍历所有能源,判断是否和子弹群相交,若相交,判断被哪个子弹击中
    for k1, bullet_group in pairs(self.bullet_groups) do
        for k, energy in pairs(self.foods) do
            local central_distance = math_sqrt(pow2(bullet_group.central_pos.x-energy.x, 2) 
            + pow2(bullet_group.central_pos.y-energy.y, 2))
            if central_distance < (config.modle_radius + bullet_group.radius) then
                local possible_bullet = nil
                for _, bullet in pairs(bullet_group.all_bullets) do
                    local bullet_world_pos = {x=bullet.local_pos.x+bullet_group.central_pos.x, y = bullet.local_pos.y + bullet_group.central_pos.y}
                    local bullet_to_food_center_dis = math_sqrt(pow2(energy.x - bullet_world_pos.x, 2) 
                        + pow2(energy.y - bullet_world_pos.y, 2))
                    if bullet_to_food_center_dis < (config.modle_radius + config.modle_radius) then
                        possible_bullet = bullet
                        break -- 找到一个能打中能源的子弹就break
                    end
                end -- end for all_bullets

                -- 移除该子弹,通知客户端移除能源,不管玩家能不能添加飞机,都要通知客户端移除子弹和能源
                if possible_bullet ~= nil then
                    local one_hit_energy_info = {
                        bulletid = possible_bullet.correspond_plane_id,
                        id = energy.id,
                        type = energy.type,
                        playerid = 0,
                        planeid = 0,
                        speed = 0,
                    }
                    local shooter = bullet_group.shoot_player
                    local newplaneid = nil
                    if shooter:is_dead() ~= true and shooter:can_add_plane() == true then
                        newplaneid = shooter:add_small_plane(false)
                    end
                    if newplaneid ~= nil then
                        one_hit_energy_info.playerid = shooter.id
                        one_hit_energy_info.planeid = newplaneid
                        local tempspeed = shooter.split_last_timer_id ~= 0 and shooter.split_move_speed or shooter.move_speed
                        one_hit_energy_info.speed = math_floor(tempspeed * 100)
                    end
                    if energy.type == 0 then
                        local newfood = self:new_food(nil, nil, nil, nil, nil, nil, 0)
                        local food_msg = {
                            food_id = newfood.id,
                            x = math_floor(newfood.x * 100),
                            y = math_floor(newfood.y * 100),
                        }
                        table_insert(batch_add_food_bc.foods, food_msg)
                    end
                    self.foods[k] = nil
                    bullet_group:remove_bullet_by_id(possible_bullet.id)
                    self:full_frame_hit_energy_info(all_bullet_group_hit_info, all_bullet_group_ids,bullet_group.id, one_hit_energy_info)
                    break
                end
            end
        end -- end bullet_groups
    end --end self.foods

    for k,v in pairs(all_bullet_group_ids) do
        local one_hit = {
            bulletgroupid = v,
            eneryhits = {},
            planehits = {},
        }

        one_hit.eneryhits = all_bullet_group_hit_info[v].energyhits
        one_hit.planehits = all_bullet_group_hit_info[v].planehits
        table_insert(frame_hit_bc.hits, one_hit)
    end
    --[[
    for _,v in pairs(frame_hit_bc.hits) do
        for _, onehit in pairs(v.planehits) do
            log:debug('send to client,groupid=%d, bulletid=%d,hit player=%d,planeid=%d,attackerid=%d,addplane=%d'
                , v.bulletgroupid, onehit.bulletid, onehit.hit_playerid, onehit.hit_planeid, onehit.attackerid, onehit.addplaneid)
        end
    end
    --]]
    self:broadcast("BatchAddFood", "plane.BatchAddFoodBcMsg", batch_add_food_bc)
    self:broadcast("NotifyFrameHit", "plane.FrameHitBcMsg", frame_hit_bc)
end

function Room:generate_food_id()
    return self.gen_food_id
end  -- generate_food_id()

-- 能源飞机也用该接口创建
function Room:new_food(x,y,foodtype, skinid, angle, playerid, broadcast)
    local ftype = foodtype or 0
    local skin = skinid or 0
    local bornx = x or math_random(config.x_min, config.x_max)
    local borny = y or math_random(config.x_min, config.x_max)
    local angle = angle or 0
    local playerid = playerid or 0
    local need_broad = broadcast or 1
    self.cur_food_id = self.cur_food_id + 1
    local food = { id = self.cur_food_id
        ,x = math_floor(bornx * 100) / 100
        , y = math_floor(borny * 100) / 100
        , type=ftype
        , skinid= skin
        , playerid = playerid
        , angle = math_floor(angle * 100)
    }
    self.foods[self.cur_food_id] = food
    if need_broad == 1 then
        local food_msg = {
            food_id = food.id,
            x = math_floor(food.x * 100),
            y = math_floor(food.y * 100),
        }
        self:broadcast("NewFood", "plane.FoodMsg", food_msg)
    end
    return food
end  -- new_food()

function Room:init_foods()
    for i = 1, config.room.food_num do
        self:new_food()
    end  -- for
end  -- init_foods()

function Room:broadcast_player_enter(player)
    local req = {
		id = player.id,
		name = player.nickname,
		move =  {
			angle = math_floor(player.angle * 100),
            score = player.score,
			x = math_floor(player.pos.x * 100),
			y = math_floor(player.pos.y * 100),
			small_plane_ids = { }
		},
		speed = math_floor(player.move_speed * 100),
        skinid = player.skinid,
        bulletskinid = player.bulletskinid,
        random_formation_id = player.formation_manager:get_formation_id(),
	}
	for  i = 1, player.small_plane_count do
		table_insert(req.move.small_plane_ids, player.small_plane[i].id)
	end
    self:broadcast("PlayerEnter", "plane.PlayerInfo", req)
    log:debug('roomid=%d,notify player enter %d,%s,left_num=%d', self.room_id, player.id, player.nickname, self:get_room_left_can_in_num())

end  -- broadcast_player_enter()

-- 广播 plane.PlanePush 请求
function Room:broadcast(mthd, req_name, req)
    --log:debug('room broadcast %s,req_name=%s', mthd, req_name)
    local req_str = pb.encode(req_name, req)
    for k, player in pairs(self.players) do
        player:rpc_request("plane.PlanePush", mthd, req_str)
    end  -- for
end  -- broadcast()

-- 返回EnterRoomResponse.
function Room:get_enter_room_response(player_id)
    local leftsecs = self:get_leftsecs()
    local ret = {
        ms_and_id = { ms = self.cur_frame, id = player_id },
        leftseconds = leftsecs > 0 and leftsecs or 0,
        players = {},
        foods = {},
        planes = {},
        room_id = self.room_id,
    }
    log:debug("Player %s enter room response start. room_id: %s.",
        tostring(player_id), tostring(ret.room_id))
    for k, player in pairs(self.players) do
        -- PlayerInfo
        if player:is_dead() ~= true then
            local info = {
                id = player.id,
                name = player.nickname,
                move = {
			        angle = math_floor(player.angle * 100),
                    score = player.score,
			        x = math_floor(player.pos.x * 100),
			        y = math_floor(player.pos.y * 100),
				    small_plane_ids = { }
			    },
		        speed = math_floor(player.move_speed * 100),
                skinid = player.skinid,
                bulletskinid = player.bulletskinid,
                random_formation_id = player.formation_manager:get_formation_id(),
            }
    
            for  i = 1, player.small_plane_count do
			    table_insert(info.move.small_plane_ids, player.small_plane[i].id)
            end
            table_insert(ret.players, info)
        end
    end  -- for
    local batch_add_plane_msgs = {}
    for food_id, food in pairs(self.foods) do
        if food.playerid ~= 0 then-- 说明是玩家分裂出来的能源飞机
            if batch_add_plane_msgs[food.playerid] ~= nil then
                local one_plane = {plane_id = food.id, x = math_floor(food.x * 100), y = math_floor(food.y*100)}
                table_insert(batch_add_plane_msgs[food.playerid].add_planes, one_plane)
            else
                batch_add_plane_msgs[food.playerid] = {}
                batch_add_plane_msgs[food.playerid].skinid = food.skinid
                batch_add_plane_msgs[food.playerid].dir_angle = food.angle
                batch_add_plane_msgs[food.playerid].add_planes = {}
                local one_plane = {plane_id = food.id, x = math_floor(food.x), y = math_floor(food.y*100)}
                table_insert(batch_add_plane_msgs[food.playerid].add_planes, one_plane)
            end
        else
            local food_msg = { food_id = food_id, x = math_floor(food.x *100), y = math_floor(food.y * 100), }
            table_insert(ret.foods, food_msg)
        end
    end  -- for
    for k,v in pairs(batch_add_plane_msgs) do
        table_insert(ret.planes, v)
    end
    --log:debug("Player %s enter room response end. room_id: %s.",
    --    tostring(player_id), tostring(ret.room_id))
    return ret
end  -- get_enter_room_response()

function Room:get_room_left_can_in_num()
    local left_num = config.room.hold_num > self.players_count and (config.room.hold_num - self.players_count) or 0
    if self:get_leftsecs() > config.room.forbid_join_seconds then
        return left_num
    else
        return 0
    end
end

function Room:get_leftsecs()
    local leftsecs = self.end_timestamp/1000 - os.time()
    return leftsecs > 0 and leftsecs or 0
end

-- 玩家加入,ai加入不走该函数.返回EnterRoomResponse.
-- 玩家加入,先剔除一个ai玩家,再将玩家加入
function Room:enter(game_clt_id, account,uid, nickname, skinid, bulletid, level)
    assert("string" == type(nickname))
    local game_clt_id_str = game_clt_id:to_string()
    local player = Player:new(game_clt_id, account,uid, nickname,skinid, bulletid, self, level)

	-- 自己进入
    self:broadcast_player_enter(player)
    -- 无敌通知必须要在客户端有了玩家之后
    player:notify_god(1)

    self.players[game_clt_id_str] = player
    self.players_count = self.players_count + 1
    self.rankmgr:on_player_enter(player)
    self:erase_one_ai_player()
    log:info("进入普通模式,玩家 %s,%d,nick=%s,分配id=%u,gamecltstr=%s,skin=%d,bullet=%d,人数%d"
        , account, uid, nickname, player.id, game_clt_id_str, skinid, bulletid, self.players_count)
	-- 别人广播给自己
    return self:get_enter_room_response(player.id)
	--return self:get_enter_room_9_screen_response(player.id)
end  -- enter()


-- 玩家的转向
function Room:player_turn_to(game_clt_id, msg)
	--log:debug("player_turn_to(),angle=" .. msg.angle)
    local player = self.players[game_clt_id:to_string()]
    assert(player, "turning player is not in room.")
	assert(msg.angle >= 0 and msg.angle <= 36000)
	player.angle = math_floor(msg.angle / 100)  -- 角度 0..360
    --player.is_stop = 0
	-- local bc_msg = {
	-- 	ms_and_id = self:get_ms_and_id(game_clt_id),
	-- 	turn_to = msg,
	-- }
	-- self:broadcast("Fire", "plane.TurnToBcMsg", bc_msg)
end

-- 开火
function Room:player_fire(game_clt_id)
    --log:info("player_fire()")
    local player = self.players[game_clt_id:to_string()]
	assert(player, "player_fire, game_clt_id not exist.")
    player:shoot()
end

-- 返回MsAndId
function Room:get_ms_and_id(game_clt_id)
    local player = self.players[game_clt_id:to_string()]
    assert(player, "Not in room.")
    return {
		-- 这个时间跟服务器很难校验，用frameid
        -- ms = c_util.get_ms(),
		ms = self.cur_frame,
        id = player.id,
    }
end

function Room:back_hall(game_clt_id)
    local game_clt_id_str = game_clt_id:to_string()
    local player = self.players[game_clt_id_str]
    if not player then return end
    player:backhall()
end

-- 玩家离开,先创建一个ai玩家,再将玩家离开
function Room:erase_player(game_clt_id)
    local game_clt_id_str = game_clt_id:to_string()
    local player = self.players[game_clt_id_str]
    if not player then return end
    local account = player.account
    local ms_and_id = self:get_ms_and_id(game_clt_id)
    if self.rankmgr ~= nil then
        self.rankmgr:on_player_exit(player)
    end
    self.players[game_clt_id_str] = nil
    self.players_count = self.players_count - 1

    log:debug("Erased player %s, Now %u players.",
        game_clt_id_str, self.players_count)
    self:broadcast("PlayerExit", "plane.MsAndId", ms_and_id)
    self:add_ai_player()

    return account
end  -- erase_player

-- 移除ai玩家,只会在玩家进入时候调用该函数
function Room:erase_one_ai_player()
    local aiplayer = nil
    for k,v in pairs(self.players) do
        if string.sub(k, 1, 4) == "_ai_" then
            aiplayer = v
            break
        end
    end
    if aiplayer == nil then
        return
    end
    self.rankmgr:on_player_exit(aiplayer)
    self.players[aiplayer.account].timer_queue:erase_all()
    self.players[aiplayer.account] = nil
    self.ai_players_count = self.ai_players_count - 1
    self.can_use_ai_names[aiplayer.id] = aiplayer.nickname
    log:debug('erase ai player %d,%s', aiplayer.id, aiplayer.account)
    local ms_and_id = {ms = self.cur_frame, id = aiplayer.id}
    self:broadcast("PlayerExit", "plane.MsAndId", ms_and_id)
end

-- 增加ai玩家,只会在玩家离开的时候调用
function Room:add_ai_player()
    for k,v in pairs(self.can_use_ai_names) do
        local aiplayer = AIPlayer:new(self, v)
        self.can_use_ai_names[k] = nil
        -- todo, 消息量太大,可以和通知enter合并为一起
        self.ai_players_count = self.ai_players_count + 1
        break
    end
end

function Room:stop_or_start_move(game_clt_id, msg)
    -- log:debug(' receive msg.ok=' .. tostring(msg.ok) .. 'game_clt_id=' .. game_clt_id:to_string())
	local game_clt_id_str = game_clt_id:to_string()
	local player = self.players[game_clt_id_str]
	if not player then 
        log:debug("fuck, receive stop move msg, could not find player id=" ..game_clt_id_str)
        return 
    end
    player:stop_or_start_move(msg.is_stop, msg.angle)
end

function Room:player_req_rank_data(game_clt_id)
	local game_clt_id_str = game_clt_id:to_string()
	local player = self.players[game_clt_id_str]
	if not player then 
        log:debug("fuck, player req rank data, could not find player id=" ..game_clt_id_str)
        return 
    end
    self.rankmgr:send_rank_data_onreq(player)
end

function Room:player_split(game_clt_id)
	local game_clt_id_str = game_clt_id:to_string()
	local player = self.players[game_clt_id_str]
	if not player then 
        log:debug("fuck, player split, could not find player id=" ..game_clt_id_str)
        return 
    end

    player:split()
end

function Room:small_plane_die(game_clt_id, msg)
	local game_clt_id_str = game_clt_id:to_string()
	local player = self.players[game_clt_id_str]
	if not player then 
        log:debug("fuck, player req rank data, could not find player id=" ..game_clt_id_str)
        return 
    end

    -- 如果从玩家的小飞机列表里找不到该小飞机,说明该玩家可能使用了作弊工具
    local plane = player:get_small_plane_by_id(msg.planeid)
	assert(plane, "Could not find " .. player.id .. "," .. player.nickname ..  "plane id=" .. msg.planeid)
	player:delete_small_plane(msg.planeid, 1)
    if player:is_dead() then
        -- 如果玩家的坐标是在地图外,要设置上出边界死亡的标记
        if player.pos.x >= config.x_max or player.pos.x <= config.x_min or player.pos.y >= config.y_max or player.pos.y <= config.y_min then
            if player.score ~= 0 then
                local minus_score = math_floor((player.score + 1) / 2)
                player.score = player.score - minus_score
                self.rankmgr:on_player_minus_score(player)
            end
            player.pos = get_born_pos()
            player:notify_death(player.nickname, 1)
        else
            player:notify_death(player.nickname, 0)
        end
    end
end

return Room
