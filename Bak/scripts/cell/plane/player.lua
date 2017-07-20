-- player.lua
-- 玩家对象

--[[
Player = {
    id = 0,
    game_clt_id = CGameCltId(),
    account = "",
    log = log:new("player." .. account),
	birth_pos = {x=0,y=0}
}
--]]
local Player = {}

--local Log = require("log")
local log = require("log"):new("plane.player")
local config = require("config")
local pb = require("protobuf")
local Plane = require("plane.plane")
local hexagon_formation = require("plane.hexagon_formation")
local circle_formation = require("plane.circle_formation")
local BulletGroup = require("plane.bullet_group")
local router = require("rpc.base_rpc_router")
local rr = require("remote_run.remote_runner")

local gen_id = 0  -- 生成ID
local table_insert = table.insert
local table_remove = table.remove
local sinTable= require('math_sin')
local cosTable = require('math_cos')
local math_floor = math.floor

-- 不依赖 user_mgr 获取 account, 而是由调用者传入 account.
-- 因为将来多服实现时， cell 服上没有 user_mgr, 只有直连服上是有user_mgr。
function Player:new(game_clt_id, account,uid, nickname, myskinid, bulletid, room, level)
    assert("userdata" == type(game_clt_id))
    assert(account)
    local player = {
        id = generate_player_id(),
        uid = uid,
        game_clt_id = game_clt_id,
        account = account,
        nickname = nickname,
        level = level,
        struid = struid,
		timer_queue = c_timer_queue.CTimerQueue(),
        move_speed = 0,
		--pos = {x=config.born_pos.x,y=config.born_pos.y},
		pos = get_born_pos(),
		angle = 0,  -- 角度 0..360
        skinid = myskinid or 1,--玩家皮肤id
        bulletskinid = bulletid or 20001,--玩家子弹皮肤id
		-- player所在的房间
		in_room = room,
		-- 玩家的三架飞机
		small_plane = {},
		-- 小飞机数量
	    small_plane_count = 0,
		-- 玩家是否停止移动，停止的话就不计算距离
		is_stop = true,
        -- 玩家的积分
		score = 0,
        -- 玩家本局一共击杀的玩家数量
		killnum = 0,
        total_killnum = 0,
        destroynum = 0,
        combonum = 0,
        highest_combo = 0,
        is_god = false,
        wrap_circle_radius = 0,-- 包裹着玩家机群的半径
        formation_manager = nil,
        last_shot_time = 0,
        shot_cd = 0,

        -- 分裂相关变量
        split_move_speed = 0,--每次分裂时候计算该速度
        split_last_timer_id = 0,
        split_cd = 0,
        last_split_time = 0,--上次分裂的时间戳,用来验证
        -- log = Log:new("plane.player_" .. account),
    }
    setmetatable(player, self)
    self.__index = self

    if math.random(1, 2) == 1 then--代表是圆形
        player.formation_manager = circle_formation
    else
        player.formation_manager = hexagon_formation
    end

    player:after_new()

    return player
end  -- new()

function Player:after_new()
	for  i = 1, config.player_born_plane_count do
        self:add_small_plane(true)
	end
    self.timer_queue:insert_single_from_now(0, function() 
        self:notify_god(1) 
    end)
    self.timer_queue:insert_single_from_now(math.floor(config.god_last_seconds * 1000), 
            function() self:notify_god(2) end)

end

function Player:on_kill_other()
    self.destroynum = self.destroynum + 1
    self.combonum = self.combonum + 1
    if self.combonum > self.highest_combo then
        self.highest_combo = self.combonum
    end
end

function Player:rpc_request(service_name, method_name, request_str, callback)
    assert("string" == type(service_name))
    assert("string" == type(method_name))
    assert("string" == type(request_str))
    assert(not callback or "function" == type(callback))
    -- log:debug("%d,%s rpc request: %s.%s", self.id, self.nickname, service_name, method_name)
    c_rpc.request_clt(self.game_clt_id, service_name, method_name,
        request_str, callback)
end  -- rpc_request()

function Player:add_small_plane(is_born_add)
	 -- 增加小飞机
    if is_born_add == false and self.small_plane_count == 0 then
        log:info("fuck, %d,%s,is already died, can not add", self.id,self.nickname)
        return
    end
    if self.small_plane_count >= config.max_plane_num then
        return
    end
	local plane_id_ = get_plane_id()
	self.small_plane_count = self.small_plane_count +1
    self:update_speed()
    -- 要放在第一个位置
    table_insert(self.small_plane, 1, Plane:new(plane_id_, self))
    local all_pos = self.formation_manager:calculate_pos(self.small_plane_count)
    for i = 1, self.small_plane_count do
        self.small_plane[i]:set_dest_pos(all_pos[i])
    end
    self.wrap_circle_radius = self.formation_manager:get_wrap_radius(self.small_plane_count)
    self.shot_cd = self.in_room:get_cd_by_plane_num(self.small_plane_count)
    self.split_cd = self.shot_cd

    -- 不需要在这里添加飞机,原因是1.如果是因为击中添加飞机,会在击中帧里通知客户端;如果出生时候添加飞机,会通过enter room response 通知自己 和 PlayerInfo 通知其他玩家的小飞机id
    --[[
    if is_born_add == false then
        local plane_bc_msg = {
            ms_and_id = { ms = self.cur_frame, id = self.id },
            new = { plane_id = plane_id_, move_speed = self.move_speed}
        }
        if self.split_last_timer_id ~= 0 then
            plane_bc_msg.new.move_speed = self.split_move_speed
        end
        self.in_room:broadcast("NewPlane", "plane.PlaneBcMsg", plane_bc_msg)
    end
    --]]
	--log:info("add_plane %d,%s plane_id = %d, self.small_plane_count:%d, speed=%f", self.id, self.nickname, plane_id_,self.small_plane_count, self.move_speed)
    return plane_id_
end

function Player:can_add_plane()
    if self.small_plane_count == 0 then
        return false
    end
    if self.small_plane_count >= config.max_plane_num then
        return false
    end

    return true
end

function Player:on_die()
    self.killnum = 0
    self.combonum = 0
    self.is_stop = true
    self.split_last_timer_id = 0
end

-- 通知玩家被 xxx 干掉了
function Player:notify_death(nickname, is_out_of_bound)
    for i = 1, config.player_born_plane_count do
        self:add_small_plane(true)
	    --log:debug("%d,%s 生成小飞机id=%d", self.id, self.nickname, self.small_plane[i])
    end
    local death_msg = {killedbyname = nickname,
        relive_seconds = math_floor(config.god_last_seconds * 100),
        is_out_of_bound = is_out_of_bound
    }
    self:rpc_request("plane.PlanePush", "NotifyDeath", pb.encode("plane.DeathMsg", death_msg))
    self:update_speed()
    self.pos = get_born_pos()
    self.in_room:broadcast_player_enter(self)
    self:notify_god(1)
    self.timer_queue:insert_single_from_now(math.floor(config.god_last_seconds * 1000), 
            function() self:notify_god(2) end)
end

function Player:notify_god(over)
    local opstr = "Op_On"
    self.is_god = true
    if over == 2 then
        self.is_god = false
        opstr = "Op_Off"
    end
    local state_msg = {playerid=self.id, state_num = "State_God", op = opstr}
	self.in_room:broadcast("NotifyStateChanged", "plane.StateMsg", state_msg)
end

-- y=k/(X+a)+b
function Player:update_speed()
    self.move_speed = config.movespeed_k / (self.small_plane_count + config.movespeed_a) + config.movespeed_b
    self.move_speed = math_floor(self.move_speed * 100) / 100
    if self.move_speed < 4 then
        self.move_speed = 4
    end
end

function Player:get_small_plane_by_id(planeid)
    local ret = nil
    for k,v in pairs(self.small_plane) do
            if v.id == planeid then
                ret = v
                break
            end
    end
    return ret
end

function Player:shoot()
    if self.small_plane_count == 0 then
        return
    end
    local nowtime = c_util.get_ms()
    if (nowtime - self.last_shot_time) < (self.shot_cd-config.shot_cd.delay_seconds) * 1000 then
        log:debug("%d,%s fire,cd is not ok,now=%u,last_shot=%u,cd=%f", self.id, self.nickname, nowtime, self.last_shot_time, self.shot_cd)
        return
    end
    local bullet_group = BulletGroup:new(get_plane_id(), self)
    --bullet_group:generate_bullets()
    self.in_room.bullet_groups[bullet_group.id] = bullet_group
    self.in_room:broadcast("Fire", "plane.FireSimpleBcMsg", bullet_group:construct_fire_bc_msg())
    self.last_shot_time = c_util.get_ms()
    --log:debug('%d,%s shot successfully,central_pos=(%f, %f)', self.id , self.nickname, bullet_group.central_pos.x, bullet_group.central_pos.y)
end

function Player:get_move_speed()
    if self.is_stop == true then
        if self.split_last_timer_id ~= 0 then
            return self.split_move_speed
        else
            return 0
        end
    else
        if self.split_last_timer_id ~=0 then
            return self.split_move_speed
        else
            return self.move_speed
        end
    end
end

function Player:frame_move(delta_time)
    --self.is_god = true
    local x,y = self:get_pos_after_move(delta_time)
    self.pos.x = x
    self.pos.y = y
    -- 遍历所有小飞机移动
    for i = 1, self.small_plane_count do
        self.small_plane[i]:frame_move(delta_time)
    end
end

function Player:get_pos_after_move(delta_time)
        local angle = math_floor(self.angle)
        local xx = 0 - sinTable[angle]
        local yy = cosTable[angle]
        -- debug open
        --local movex = self.move_speed * delta_time * xx
        --local movey = self.move_speed * delta_time * yy
        --log:info("%d,%s,xx=%f,yy=%f,move=(%f,%f),delta_time=%f,speed=%f", self.id, self.nickname, movex, movey, self.pos.x, self.pos.y, delta_time, self.move_speed)
        local speed = self:get_move_speed()
    	local destx = self.pos.x + speed * delta_time * xx
    	local desty = self.pos.y + speed * delta_time * yy
        return destx, desty
end

-- 玩家请求分裂,若只有一个飞机,则不能分裂; 大于1个飞机,将table最尾部的 (n+1)/2 架飞机删除掉
-- 创建类型为1的能源飞机,通知客户端皮肤id,目前暂定为玩家的皮肤id
-- 通知客户端批量删除小飞机
function Player:split()
    if self.small_plane_count <= 1 then
        return
    end
    local nowtime = c_util.get_ms()
    if (nowtime - self.last_split_time) < (config.split_cd -config.shot_cd.delay_seconds) * 1000 then
        log:debug("%d,%s split,cd is not ok,now=%u,last_shot=%u,cd=%f", self.id, self.nickname, nowtime, self.last_shot_time, self.shot_cd)
        return
    end
    self.last_split_time = nowtime

    if self.split_last_timer_id ~= 0 then
            self.timer_queue:erase(self.split_last_timer_id)
            self.split_last_timer_id = 0
    end

    local need_remove_plane_num = math.floor((self.small_plane_count + 1) / 2)
    local all_remove_plane_ids = {}
    local all_born_energy_planes = {}
    for i = 1,need_remove_plane_num do
        table_insert(all_remove_plane_ids, self.small_plane[self.small_plane_count].id)
        local plane_pos = self.small_plane[self.small_plane_count]:get_world_pos()
        table_insert(all_born_energy_planes, self.in_room:new_food(plane_pos.x, plane_pos.y, 1, math.floor((self.skinid + 9) / 10), self.angle, self.id, 0))
        self.small_plane[self.small_plane_count] = nil
        self.small_plane_count = self.small_plane_count - 1
    end

    local batch_add_energy_plane_bc_msg = {
        skinid = math.floor((self.skinid + 9) / 10),
        dir_angle = math_floor(self.angle * 100),
        add_planes = {},
    }
    for k,v in pairs(all_born_energy_planes) do
        local one_plane = {
            plane_id = v.id,
            x = math_floor(v.x * 100),
            y = math_floor(v.y * 100),
        }
        table_insert(batch_add_energy_plane_bc_msg.add_planes, one_plane)
    end
    self.in_room:broadcast("BatchAddEnergyPlane", "plane.BatchAddEnergyPlaneBcMsg", batch_add_energy_plane_bc_msg)

    local batch_remove_bc_msg = {
        playerid = self.id,
        plane_ids = {}
    }
    for k,v in pairs(all_remove_plane_ids) do
        table_insert(batch_remove_bc_msg.plane_ids, v)
    end
    self.in_room:broadcast("BatchRemovePlane", "plane.BatchRemovePlaneBcMsg", batch_remove_bc_msg)

    self:update_speed()
    self.split_move_speed = math_floor(config.split_speed_k * self.move_speed * 100) / 100
    -- log:debug('%d,%s send split speed=%f,movespeed=%f', self.id, self.nickname, self.split_move_speed, self.move_speed)

    local update_speed_bc = {
        playerid = self.id,
        speed = math_floor(self.split_move_speed * 100),
    }
    self.in_room:broadcast("UpdateSpeed", "plane.UpdateSpeedBcMsg", update_speed_bc)
    if self.is_stop == true then
        local move_bc_msg = {
            ms_and_id = {ms = self.cur_frame, id = self.id},
            info = {is_stop = false, angle = self.angle}
        }
	    self.in_room:broadcast("PlayerStopOrMove", "plane.StopOrMoveBeginBcMsg", move_bc_msg)
    end
    self.split_last_timer_id = self.timer_queue:insert_single_from_now(math.floor(config.split_last_seconds * 1000)
        , function ()
            self.split_last_timer_id = 0
            local update_speed_bc = {
                playerid = self.id,
                speed = math_floor(self.move_speed * 100),
            }
            --log:debug('%d,%s split speed=%f,send movespeed=%f', self.id, self.nickname, self.split_move_speed, self.move_speed)
            self.in_room:broadcast("UpdateSpeed", "plane.UpdateSpeedBcMsg", update_speed_bc)
        
            local move_bc_msg = {
                ms_and_id = {ms = self.cur_frame, id = self.id},
                info = {is_stop= self.is_stop, angle = math_floor(self.angle * 100)}
            }
    	    self.in_room:broadcast("PlayerStopOrMove", "plane.StopOrMoveBeginBcMsg", move_bc_msg)
        end)
end

-- 客户端角度是 1.5, 发过来的是 150, 服务器用的是 1.0,发给其他人的是 100
function Player:stop_or_start_move(is_stop, angle)
    local real_angle = math_floor(self.angle / 100)
    local bc_msg = {
        ms_and_id = {ms = self.in_room.cur_frame, id = self.id},
        info = {is_stop = is_stop, angle = math_floor(real_angle * 100)}
    }
	self.in_room:broadcast("PlayerStopOrMove", "plane.StopOrMoveBeginBcMsg", bc_msg)
	self.is_stop = is_stop
    if self.is_stop == false then
        self.angle = real_angle
    end
    if is_stop == true and self.split_last_timer_id ~= 0 then
        self.timer_queue:erase(self.split_last_timer_id)
        self.split_last_timer_id = 0
        local update_speed_bc = {
            playerid = self.id,
            speed = math_floor(self.move_speed * 100),
        }
        --log:debug('%d,%s split speed=%f,send movespeed=%f', self.id, self.nickname, self.split_move_speed, self.move_speed)
        self.in_room:broadcast("UpdateSpeed", "plane.UpdateSpeedBcMsg", update_speed_bc)
    end
	-- log:debug("receive msg " .. player.game_clt_id:to_string() .. ",is_stop = " .. tostring(player.is_stop))
end
-- is_out_of_edge= 1代表出界,需要通知客户端消失飞机
function Player:delete_small_plane(plane_id, is_out_of_edge)
    local to_be_removed_index = 0
	for  k,v in pairs(self.small_plane) do
		if v.id == plane_id then
            to_be_removed_index = k
			break
		end
	end

    if to_be_removed_index ~= 0 then
        table_remove(self.small_plane, to_be_removed_index)
		self.small_plane_count = self.small_plane_count -1
        if self.small_plane_count ~= 0 then
            self.shot_cd = self.in_room:get_cd_by_plane_num(self.small_plane_count)
            self.split_cd = self.shot_cd
            self:update_speed()
            local all_pos = self.formation_manager:calculate_pos(self.small_plane_count)
            for i = 1, self.small_plane_count do
                self.small_plane[i]:set_dest_pos(all_pos[i])
            end
            self.wrap_circle_radius = self.formation_manager:get_wrap_radius(self.small_plane_count)
        else    
            self.wrap_circle_radius = 0
            self:on_die()
        end

        local bc_msg = {
            ms_and_id = {ms = self.in_room.cur_frame, id = self.id},
            new = {plane_id = plane_id, move_speed = math_floor(self.move_speed * 100)}
        }
        if self.split_last_timer_id ~= 0 then
            bc_msg.new.move_speed = math_floor(self.split_move_speed * 100)
        end
        if is_out_of_edge == 1 then
            self.in_room:broadcast("RemovePlane", "plane.PlaneBcMsg", bc_msg)
        end
        --log:debug('%d,%s delete planeid=%d', self.id, self.nickname, plane_id)
        return true
    end

    return false
end

-- 飞机数量为0,玩家已死
function Player:is_dead()
    return self.small_plane_count == 0
end

-- 设置可以用钱了
function Player:set_user_can_op_money()
    rr.run_mfa(self.game_clt_id.base_svr_id,
        "user_mgr", "set_user_can_op_money",
        { self.game_clt_id.base_rpc_clt_id })
end  -- set_user_can_op_money()

function Player:backhall()
    self:rpc_request("plane.PlanePush", "BackHallOK", '')
    router.reset_svc_dst_svr_id(self.game_clt_id, "plane.Plane")
    -- 设置可以用钱了
    self:set_user_can_op_money()
end

function Player:on_room_destroy()
    self.timer_queue:erase_all()
    c_redis.set("room:" .. self.account, 0,function(reply_type)
        if reply_type ~= true then
            self.log:error("%s,%s offline, set room =0 failed", self.account, self.nickname)
        end
    end)

    -- 重置RPC路由
    router.reset_svc_dst_svr_id(self.game_clt_id, "plane.Plane")
    -- 设置可以用钱了
    self:set_user_can_op_money()
end  -- on_room_destroy()

return Player
