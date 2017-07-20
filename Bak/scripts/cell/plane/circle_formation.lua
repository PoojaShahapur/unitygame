-- 根据飞机数量和阵型,计算各个飞机的位置
-- 圆形阵型计算
local M = {}
local config = require("config")
local sinTable= require('math_sin')
local cosTable = require('math_cos')
local math_floor = math.floor

function M:get_formation_id()
    return 1
end

-- 根据圈数获得该圈应该放多少个飞机
function M:get_plane_num_by_circle_num(which_circle)
    if 1 == which_circle then
        return 1
    end
    -- 最中间的是第0圈
    return (which_circle - 1) * 7 - 1
end

-- 获取包裹阵型的圆半径
function M:get_wrap_radius(plane_num)
    local circle = self:get_circle_num_by_plane_num(plane_num)
    return 2 * config.circle_modle_radius * (circle - 1) + config.circle_modle_radius
end

-- 根据飞机数量,获得一共有多少圈
function M:get_circle_num_by_plane_num(plane_num)
    if 1 == plane_num then
        return 1
    end
    local circle_num = 1
    local current_num = 1
    while current_num < plane_num do
        circle_num = circle_num + 1
        current_num = current_num + self:get_plane_num_by_circle_num(circle_num)
    end

    return circle_num
end

-- 获得从第2圈开始,.每个飞机之间相隔的角度
function M:get_angle_between_plane(which_circle)
    local all_angles = {}
    all_angles[1] = 0
    local circle = 2
    while circle <= which_circle do
        local num = self:get_plane_num_by_circle_num(circle)
        all_angles[circle] = 360 / num
        circle = circle + 1
    end
    return all_angles
end

-- 根据每个小飞机的圆形模型半径的大小,以及飞机的数量,算出每个飞机应该在六边形阵型的哪个坐标
function M:calculate_pos(plane_num)
    local all_pos = {}
    all_pos[1] = {x = 0, y = 0}
    if 1 == plane_num then
        return all_pos
    end

    local total_circle_num = self:get_circle_num_by_plane_num(plane_num)
    local already_calculate_num = 2--已经计算过的小飞机个数+1,代表下一个放入table的下标
    local all_angles = self:get_angle_between_plane(total_circle_num)
    local sqrt3 = math.sqrt(3)
    for circle = 2, total_circle_num do
        local this_circle_num = self:get_plane_num_by_circle_num(circle)
        if already_calculate_num > plane_num then
            return
        else
            local cur_index = 1
            while cur_index <= this_circle_num do
                -- 计算 x,y 值
                local angle = math_floor(all_angles[circle] * (cur_index - 1))
                local tempx = cosTable[angle] * (circle-1) * 2 * config.circle_modle_radius
                local tempy = sinTable[angle] * (circle-1) * 2 * config.circle_modle_radius
                all_pos[already_calculate_num] = {x = tempx, y = tempy}
                already_calculate_num = already_calculate_num + 1
                cur_index = cur_index + 1
                if already_calculate_num > plane_num then
                    break
                end
            end
        end
    end
    return all_pos
end

function M:print_cpp_map()
    for i = 1,500 do
        self:print_this_number_map(i)
    end
end

function M:print_this_number_map(number)
    print('{')
    print('\t' .. number .. ',\n\t{')
    -- 2个tab缩进,打印出number行
    local all_pos = self:calculate_pos(number)
    for i = 1,number do
        print('\t\t{' .. all_pos[i].x .. ', ' .. all_pos[i].y .. '},')
    end
    print('\t}')
    print('},')
end

function M:print_wrap_radius_map()
    for i = 1,500 do
        print('{' .. i .. ',' .. self:get_wrap_radius(i) .. '},')
    end
end

return M
