local Plane = {}

local log = require("log"):new("plane.player")
local config = require("config")

function Plane:new(planeid, player)
    assert(player)
    local plane = {
        id = planeid,
        owner_player = player,
        arrive_dest = false,
        local_pos = {x = 0, y = 0},
        -- 该方向是单位化后的单位向量
        local_direction = {x = 0, y = 0},
        dest_pos = {x = 0, y = 0},
    }

    setmetatable(plane, self)
    self.__index = self
    return plane
end

-- 设置目标点位置
function Plane:set_dest_pos(dest_pos)
    if self.local_pos.x == dest_pos.x and self.local_pos.y == dest_pos.y then
        self.arrive_dest = true
    else    
        self.arrive_dest = false
        local direction_len = math.sqrt(math.pow(self.local_pos.x-dest_pos.x, 2) + math.pow(self.local_pos.y-dest_pos.y, 2))
        self.local_direction.x = (dest_pos.x - self.local_pos.x) / direction_len
        self.local_direction.y = (dest_pos.y - self.local_pos.y) / direction_len
        self.dest_pos = dest_pos
    end
    -- debug open
    --log:debug('planeid=%d,localpos=(%f,%f),moveto=(%f,%f)', self.id, self.local_pos.x, self.local_pos.y, self.dest_pos.x, self.dest_pos.y)
end

function Plane:get_world_pos()
    local pos = {x = self.owner_player.pos.x + self.local_pos.x, y = self.owner_player.pos.y + self.local_pos.y}
    return pos
end

-- 更新小飞机位置
function Plane:frame_move(delta_time)
    if self.arrive_dest == false then
        -- 判断按当前速度移动,本帧后是否大于等于 dest_pos,若超过的话,直接设置到目标点,移动结束;没超过的话,移动,更新local_pos
        local old_x = self.local_pos.x
        local old_y = self.local_pos.y
        local new_x = old_x + self.owner_player.move_speed * delta_time * self.local_direction.x
        local new_y = old_y + self.owner_player.move_speed * delta_time * self.local_direction.y
        local multiplex = (self.dest_pos.x - old_x) * (self.dest_pos.x - new_x)
        local multipley = (self.dest_pos.y - old_y) * (self.dest_pos.y - new_y)
        if multiplex <= 0 and multipley <=0.000001 then
            self.local_pos = self.dest_pos
            self.arrive_dest = true
        else
            self.local_pos.x = new_x
            self.local_pos.y = new_y
        end
        -- debug open
        --log:debug('planeid=%d,dir=(%f,%f),(%f,%f)-->(%f,%f),disxy=(%f,%f),disx=%s,disy=%s', self.id, self.local_direction.x, self.local_direction.y, old_x, old_y, self.local_pos.x, self.local_pos.y
        --    ,multiplex, multipley, tostring(multiplex<=0), tostring(multipley<=0.000001))
    end
end


return Plane
