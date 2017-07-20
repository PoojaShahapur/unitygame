-- 一个子弹组
local BulletGroup = {}

local log = require("log"):new("plane.bullet_group")
local config = require("config")
local sinTable= require('math_sin')
local cosTable = require('math_cos')
local math_floor = math.floor
local math_pow = math.pow

-- 子弹实体,就1个id + local_pos 属性
--

--根据玩家的飞机数量,算出子弹存活时间
function get_bullet_lifetime_by_num(num)
    return config.lifetime_k * math_pow(num, 1.0 / config.lifetime_a) + config.lifetime_b
end

-- 放在 room 里统一管理
-- 中心点坐标,子弹群的半径,方向,子弹组的id,子弹组的速度,发射玩家,一堆子弹实体
function BulletGroup:new(groupid, player)
    assert(player)
    local group = {
        id = groupid,
        shoot_player = player,
        central_pos = {x = player.pos.x , y = player.pos.y},
        radius = player.wrap_circle_radius,
        angle = player.angle,
        speed = math_floor(player.move_speed * (1 + config.bullet_speed_coef / 100) * 100) / 100,
        lifetime = get_bullet_lifetime_by_num(player.small_plane_count),
        all_bullets = {}
    }

    if player.game_clt_id ~= 0 and player.split_last_timer_id ~=0 then
        group.speed = player.split_move_speed * (1 + config.bullet_speed_coef / 100)
        group.lifetime = group.lifetime * player.move_speed / player.split_move_speed
    end

    setmetatable(group, self)
    self.__index = self

    group:generate_bullets()
    -- 增加一个摧毁定时器
    local timerid = player.in_room.timer_queue:insert_single_from_now(group.lifetime * 1000, function()
        group:destroy()
        group=nil
    end)
    return group
end

-- 根据玩家的小飞机的分布,生成对应的子弹位置
function BulletGroup:generate_bullets()
    for i = 1, self.shoot_player.small_plane_count do
        local bulletid = get_plane_id()
        local plane = self.shoot_player.small_plane[i]
        self.all_bullets[bulletid]= {id = bulletid
            , correspond_plane_id = plane.id
            ,local_pos = {x = plane.local_pos.x, y = plane.local_pos.y}
        }
    end
end

function BulletGroup:construct_fire_bc_msg()
    --[[
    local bc_msg = {
        dir_angle = self.angle,
        x = self.central_pos.x,
        y = self.central_pos.y,
        life_seconds = config.bullet_life_seconds,
        speed = self.speed,
        shooter_id = self.shoot_player.id,
        bullet_group_id = self.id,
        all_bullets = {},
    }
    --]]
    local simple_bc_msg = {
        dir_angle = math_floor(self.angle * 100),
        x = math_floor(self.central_pos.x * 100),
        y = math_floor(self.central_pos.y * 100),
        life_seconds = math_floor(self.lifetime * 100),
        speed = math_floor(self.speed * 100),
        shooter_id = self.shoot_player.id,
        bullet_group_id = self.id,
        fire_ms = c_util.get_sys_ms() - self.shoot_player.in_room.createtime,
    }

    --[[
    for k,v in pairs(self.all_bullets) do
        local bullet = {id = v.id, x = v.local_pos.x, y = v.local_pos.y}
        table.insert(bc_msg.all_bullets, bullet)
        -- debug open
        -- log:debug('%d,%s fire, bulletid=%d,x,y=(%f,%f)', self.shoot_player.id, self.shoot_player.nickname, v.id, v.local_pos.x, v.local_pos.y)
    end
    --]]

    return simple_bc_msg
end

function BulletGroup:remove_bullet_by_id(id)
    self.all_bullets[id] = nil
end

function BulletGroup:print_all_bullets()
    for k,v in pairs(self.all_bullets) do
        log:debug('print,bulletgroup id=%d,bullerid=%d', self.id, v.id)
    end
end

function BulletGroup:frame_move(delta_time)
    local angle = math_floor(self.angle)
    local xx = 0 - sinTable[angle]
    local yy = cosTable[angle]
    local movex = self.speed * delta_time * xx
    local movey = self.speed * delta_time * yy
    -- debug open
    --log:info("bulletgroup=%d,xx=%f,yy=%f,move=(%f,%f),delta_time=%f,speed=%f", self.id, movex, movey, self.central_pos.x, self.central_pos.y, delta_time, self.speed)
    self.central_pos.x = self.central_pos.x + self.speed * delta_time * xx
    self.central_pos.y = self.central_pos.y + self.speed * delta_time * yy
end

function BulletGroup:destroy()
    -- 从 room 中移除
    self.shoot_player.in_room.bullet_groups[self.id] = nil
end

return BulletGroup
