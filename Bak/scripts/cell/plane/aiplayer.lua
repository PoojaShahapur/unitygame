-- aiplayer.lua 玩家对象

local AIPlayer = {}

--local Log = require("log")
local log = require("log"):new("plane.aiplayer")
local config = require("config")
local pb = require("protobuf")
local Plane = require("plane.plane")
local hexagon_formation = require("plane.hexagon_formation")
local circle_formation = require("plane.circle_formation")
local BulletGroup = require("plane.bullet_group")

local table_insert = table.insert
local table_remove = table.remove
local sinTable= require('math_sin')
local cosTable = require('math_cos')
local math_random = math.random
local math_floor = math.floor
local pi = math.pi

local tempid = 0
function ai_tempid()
    tempid = tempid + 1
    return tempid
end


STATE_IDLE = 1--AI的闲逛状态
STATE_ATTACK = 2--攻击状态

--[[
self.next_angle = 0
self.already_set_next_angle = 0
每次移动时候判断 self.already_set_next_angle == 0 并且要出界,设置 self.next_andle = 相反方向; self.already_set_next_angle = 1
创建1秒定时器,定时器触发时随机改变方向;
定时器触发函数里面,检测若 self.next_angle ~= 0 直接设置 self.angle = self.next_angle; self.next_angle = 0; 若 self.next_angle = 0,则设置 self.already_set_next_angle = 0
这样做,因为是按相反反向走的,而且下次改变方向定时器触发时间戳 - 检测出界的时间戳 <= 1s,能保证玩家在下1秒内走到界内;
--]]
--
-- index 代表是第几个ai玩家
function AIPlayer:new(room, name)
    local aiid = generate_player_id()
    local aiplayer = {
        id = aiid,
        game_clt_id = 0,
        account = '_ai_' .. aiid,
        --nickname = '你感受过绝望吗',
        nickname = name,
        --nickname = 'ai_' .. aiid,
		timer_queue = c_timer_queue.CTimerQueue(),
        move_speed = 0,
		--pos = {x=config.born_pos.x + (ai_tempid() % 20) * 5,y=config.born_pos.y},
		 pos = get_born_pos(),
		angle = math_random(0, 359),  -- 角度 0..360
        --angle = 0,
        skinid = 1,--基础皮肤,编号从1-30
        bulletskinid = 20001,--玩家子弹皮肤id
		-- AIPlayer所在的房间
		in_room = room,
		-- 玩家的三架飞机
		small_plane = {},
		-- 小飞机数量
	    small_plane_count = 0,
		-- 玩家是否停止移动，停止的话就不计算距离
		is_stop = false,
        -- 玩家的积分
		score = 0,
        -- 玩家本局一共击杀的玩家数量
		killnum = 0,
        total_killnum = 0,
        is_god = false,
        wrap_circle_radius = 0,-- 包裹着玩家机群的半径
        formation_manager = nil,
        last_shot_time = 0,
        shot_cd = 0,
        destroynum = 0,
        -- 根据当前cd 添加定时器发射子弹,存储下来定时器id,每次触发时候,检测 last_cd_when_timer_created - shot_cd > 0.1s, 就删除并新建一个定时器.
        last_cd_when_timer_created = 0.0,
        shot_timer_id = 0,
        -- 检查变换方向是否有效的定时器id
        -- 若变换方向1秒后,还未走到地图内,则重新随机方向
        change_dir_checker_id = 0,
        -- ai 玩家每过一段时间重新换方向
        change_dir_timer = 0,
        attack_change_dir_timer = 0,
        next_angle = 0,
        already_set_next_angle = 0,
        state = STATE_IDLE,--ai的状态,1为闲逛,2为追击,死亡后重设状态为闲逛
        target_player = nil,
        search_enemy_timer = 0,
        attack_trigger_time = 0,--攻击更换方向的次数
    }
    setmetatable(aiplayer, self)
    self.__index = self

    if math.random(1, 2) == 1 then--代表是圆形
        aiplayer.formation_manager = circle_formation
    else
        aiplayer.formation_manager = hexagon_formation
    end

    aiplayer:after_new()

    return aiplayer
end  -- new()

function AIPlayer:change_state(state, player)
    if STATE_ATTACK == state then
        self.target_player = player
        --self.timer_queue:erase(self.search_enemy_timer)
        self.attack_change_dir_timer = self.timer_queue:insert_repeat_from_now(0, 500, function()
            self:change_dir_when_attack()
        end)    
    else
        self.target_player = nil--todo,不需要吧..
        if self.attack_change_dir_timer ~= 0 then
            self.timer_queue:erase(self.attack_change_dir_timer)
        end
        self.attack_change_dir_timer = 0
    self.search_enemy_timer = self.timer_queue:insert_repeat_from_now(0, 500, 
            function() 
                self:find_nearest_enemy()
            end)
    end
        self.state = state
end

--闲逛结束,开始干人
function AIPlayer:find_nearest_enemy()
    if self:is_dead() == true then
        return
    end

    if self.state == STATE_ATTACK then
        return
    end

    local min_dis = math.maxinteger
    local dest_player = nil
    for k,v in pairs(self.in_room.players) do
        if v.game_clt_id ~= 0 and v:is_dead() ~= true then
            local disx = v.pos.x - self.pos.x
            local disy = v.pos.y - self.pos.y
            local dis = disx * disx + disy * disy
            if dis < min_dis and dis < 144 then
                min_dis = dis
                dest_player = v
            end
        end
    end

    if dest_player ~= nil then
        --log:info("%d,%s change target=%d.%s", self.id, self.nickname, dest_player.id, dest_player.nickname)
        self:change_state(STATE_ATTACK, dest_player)
    end
end

function AIPlayer:after_new()
	for  i = 1, config.player_born_plane_count do
        self:add_small_plane(true)
	end
    self:notify_god(1)
    self.timer_queue:insert_single_from_now(math.floor(config.god_last_seconds * 1000), 
            function() self:notify_god(2) end)
    self.search_enemy_timer = self.timer_queue:insert_repeat_from_now(0, 500, 
            function() 
                self:find_nearest_enemy()
            end)
    self.shot_timer_id = self.timer_queue:insert_repeat_from_now(0,  4 * self.shot_cd * 1000, function()
        self:shoot()
        end)        
    self.change_dir_timer = self.timer_queue:insert_repeat_from_now(0, config.change_direction_seconds * 1000, function()
            self:change_direction_on_timer()
        end)    

    self.in_room:broadcast_player_enter(self)
    self.in_room.players[self.account] = self
    self.in_room.ai_players_count = self.in_room.ai_players_count + 1
    self.in_room.rankmgr:on_player_enter(self)
	log:debug("New AIPlayer. account=%s,id=%d, fid=%d,speed=%f,cd=%f"
        , self.nickname, self.id,self.formation_manager:get_formation_id(), self.move_speed, self.shot_cd)
end

function AIPlayer:on_kill_other()
    self.destroynum = self.destroynum + 1
end

function AIPlayer:rpc_request(service_name, method_name, request_str, callback)
    return
end  -- rpc_request()

function AIPlayer:can_add_plane()
    if self.small_plane_count == 0 then
        return false
    end
    if self.small_plane_count >= config.ai_max_plane_num then
        return false
    end

    return true
end

function AIPlayer:add_small_plane(is_born_add)
	 -- 增加小飞机
    if is_born_add == false and self.small_plane_count == 0 then
        log:info("fuck, %d,%s,is already died, can not add", self.id,self.nickname)
        return
    end
    if self.small_plane_count >= config.ai_max_plane_num then
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

    -- 不需要在这里添加飞机,原因是1.如果是因为击中添加飞机,会在击中帧里通知客户端;如果出生时候添加飞机,会通过enter room response 通知自己 和 PlayerInfo 通知其他玩家的小飞机id
    --[[
    if is_born_add == false then
        local plane_bc_msg = {
            ms_and_id = { ms = self.cur_frame, id = self.id },
            new = { plane_id = plane_id_, move_speed = self.move_speed}
        }
        self.in_room:broadcast("NewPlane", "plane.PlaneBcMsg", plane_bc_msg)
    end
    --]]
	--log:info("ai add_plane %d,%s plane_id = %d, self.small_plane_count:%d", self.id, self.nickname, plane_id_,self.small_plane_count)
    return plane_id_
end

-- 通知玩家被 xxx 干掉了
function AIPlayer:notify_death(nickname, is_out_of_bound)
    for i = 1, config.player_born_plane_count do
        self:add_small_plane(true)
	    --log:debug("%d,%s 生成小飞机id=%d", self.id, self.nickname, self.small_plane[i])
    end
    local death_msg = {killedbyname = nickname,
        relive_seconds = math_floor(config.god_last_seconds * 100),
        is_out_of_bound = is_out_of_bound
    }
    self:rpc_request("plane.PlanePush", "NotifyDeath", pb.encode("plane.DeathMsg", death_msg))
    self.is_stop = false
    self:update_speed()
    self.pos = get_born_pos()
    self.in_room:broadcast_player_enter(self)
    self:notify_god(1)
    self.timer_queue:insert_single_from_now(math.floor(config.god_last_seconds * 1000), 
            function() self:notify_god(2) end)
end

function AIPlayer:notify_god(over)
    local opstr = "Op_On"
    self.is_god = true
    if over == 2 then
        self.is_god = false
        opstr = "Op_Off"
    end
    local state_msg = {playerid=self.id, state_num = "State_God", op = opstr}
	self.in_room:broadcast("NotifyStateChanged", "plane.StateMsg", state_msg)
end

function AIPlayer:update_speed()
    self.move_speed = config.movespeed_k / (self.small_plane_count + config.movespeed_a) + config.movespeed_b
    if self.move_speed < 4 then
        self.move_speed = 4
    end
    --self.move_speed = 0.01
end

function AIPlayer:get_small_plane_by_id(planeid)
    local ret = nil
    for k,v in pairs(self.small_plane) do
            if v.id == planeid then
                ret = v
                break
            end
    end
    return ret
end

function AIPlayer:shoot()
    if self.small_plane_count == 0 then
        return
    end
    local nowtime = c_util.get_ms()
    if (c_util.get_ms() - self.last_shot_time) < (self.shot_cd-config.shot_cd.delay_seconds) * 1000 then
        log:debug("%d,%s fire,cd is not ok,now=%u,last_shot=%u,cd=%f", self.id, self.nickname, nowtime, self.last_shot_time, self.shot_cd)
        return
    end
    local bullet_group = BulletGroup:new(get_plane_id(), self)
    --bullet_group:generate_bullets()
    self.in_room.bullet_groups[bullet_group.id] = bullet_group
    self.in_room:broadcast("Fire", "plane.FireSimpleBcMsg", bullet_group:construct_fire_bc_msg())
    self.last_shot_time = c_util.get_ms()
end

function AIPlayer:change_direction_on_edge()
    local new_angle = math.random(self.angle - 60, self.angle + 60)
    if new_angle < 0 then
        new_angle = new_angle + 360
    end
    --self.angle = new_angle
    self.angle = (self.angle + 180) % 360
    --log:debug('%d,%s change angle edge = %f,state=%d', self.id , self.nickname, self.angle, self.state)
end

function AIPlayer:change_direction_on_timer()
    if self:is_dead() == true then
        return
    end
    if self.state == STATE_ATTACK then
        return
    end
    if self.next_angle ~= 0 then
        self.angle = self.next_angle
        self.next_angle = 0
    else
        self.already_set_next_angle = 0
        self.angle = math.random(0, 359)
    end
    --log:debug('%d,%s change angle timer= %f,state=%d', self.id , self.nickname, self.angle, self.state)
end

-- 算下玩家按当前方向移动 200ms 后的位置点A,再计算我的位置和点A的夹角,设为方向
function AIPlayer:change_dir_when_attack()
    if self:is_dead() == true then
        return
    end
    --[[
    self.attack_trigger_time = self.attack_trigger_time + 1
    if self.attack_trigger_time > 12 then
        self:change_state(STATE_IDLE)
        self.attack_trigger_time = 0
        return
    end
    ]]
    if self.target_player == nil then
        return
    end

    if self.target_player:is_dead() == true then
        self:change_state(STATE_IDLE)
        return
    end

    local x,y = self.target_player:get_pos_after_move(0.4)
    if self.pos.y == y and  self.pos.x == x then
        return
    end

    -- 假设AI是 (0,0),player=(0,10),那 angle=0
    -- AI=(0,10),player=(0,0),angle=180
    -- AI=(3,4),player=(4,5),angle=-45=360-45
    local rad = math.atan2(y - self.pos.y, x - self.pos.x) - pi / 2
    self.angle = math_floor(rad * 180 / pi)
    if self.angle < 0 then
        self.angle = 360 + self.angle
    end
    --log:info('mypos=(%f,%f),player=(%f,%f),angle=%d', self.pos.x
        --, self.pos.y, self.target_player.pos.x, self.target_player.pos.y, self.angle)
end

function AIPlayer:is_close_to_edge(x, y)
    local x = x or self.pos.x
    local y = y or self.pos.y
    if x <= config.x_min + 5 or x >= config.x_max - 5 or y <= config.y_min + 5 or y >= config.y_max - 5 then
        return true
    end

    return false
end

function AIPlayer:on_die()
    self.killnum = 0
    self.is_stop = true
    self.change_dir_checker_id = 0
    -- 死亡后变成游荡状态
    self:change_state(STATE_IDLE)
end

function AIPlayer:frame_move(delta_time)
    if self.is_stop == false then
        local angle = math_floor(self.angle)
        local xx = 0 - sinTable[angle]
        local yy = cosTable[angle]
        local destx = self.pos.x + self.move_speed * delta_time * xx
        local desty = self.pos.y + self.move_speed * delta_time * yy
        if self.already_set_next_angle == 0 and self:is_close_to_edge(destx, desty) == true then
            self.next_angle = (self.angle + 180) % 360
            self.already_set_next_angle = 1
        end
    	self.pos.x = destx
        self.pos.y = desty
        -- debug open
        --log:info("%d,%s,move=(%f,%f),delta_time=%f,speed=%f", self.id, self.nickname, self.pos.x, self.pos.y, delta_time, self.move_speed)
    end

    -- 遍历所有小飞机移动
    for i = 1, self.small_plane_count do
        self.small_plane[i]:frame_move(delta_time)
    end

    if math.abs(self.shot_cd - self.last_cd_when_timer_created) > 0.1 then
        self.last_cd_when_timer_created = self.shot_cd
        self.timer_queue:erase(self.shot_timer_id)
        self.shot_timer_id = self.timer_queue:insert_repeat_from_now(0, 4 * self.shot_cd * 1000, function()
            self:shoot()
        end)        
    end
end

-- reason = 0代表出界,否则代表被击中,需要
-- todo, ai 玩家出界后通知客户端
function AIPlayer:delete_small_plane(plane_id)
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
            self:update_speed()
            local all_pos = self.formation_manager:calculate_pos(self.small_plane_count)
            for i = 1, self.small_plane_count do
                self.small_plane[i]:set_dest_pos(all_pos[i])
            end
            self.wrap_circle_radius = self.formation_manager:get_wrap_radius(self.small_plane_count)
        else    
            self.wrap_circle_radius = 0
        end
        return true
    end

    return false
end
function AIPlayer:is_dead()
    return self.small_plane_count == 0
end

function AIPlayer:on_room_destroy()
    self.timer_queue:erase_all()
end

return AIPlayer
