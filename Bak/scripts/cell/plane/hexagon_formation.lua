-- 根据飞机数量和阵型,计算各个飞机的位置
-- 目前支持 六边形,圆形 一共2种阵型
local M = {}
local config = require("config")

function M:get_formation_id()
    return 2
end

-- 根据圈数获得该圈应该放多少个飞机
function M:get_plane_num_by_circle_num(which_circle)
    if which_circle > 1 then
        return 6 * (which_circle - 1)
    else--传进来一个负数?暂时负数或0也返回1
        return 1
    end
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

-- 获取包裹阵型的圆半径
function M:get_wrap_radius(plane_num)
    local circle = self:get_circle_num_by_plane_num(plane_num)
    return 2 * config.modle_radius * (circle - 1) + config.modle_radius
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
    local sqrt3 = math.sqrt(3)
    for circle = 2, total_circle_num do
        --对于每圈飞机,先定位出6个特殊顶点的坐标和序号,最右边的顶点算序号0
        local index_of_node = {}
        for i = 1,6 do
            index_of_node[i] = (i-1) * (circle - 1) + 1
        end
        local pos_of_node = {}--6个顶点的坐标
        pos_of_node[1] = {x = 2 * config.modle_radius * (circle - 1), y = 0}
        pos_of_node[2] = {x = config.modle_radius * (circle - 1), y = sqrt3 * config.modle_radius * (circle - 1)}
        pos_of_node[3] = {x = 0 - config.modle_radius * (circle - 1), y = sqrt3 * config.modle_radius * (circle - 1)}
        pos_of_node[4] = {x = 0 - 2 * config.modle_radius * (circle - 1), y = 0}
        pos_of_node[5] = {x = 0 - config.modle_radius * (circle - 1), y = 0 - sqrt3 * config.modle_radius * (circle - 1)}
        pos_of_node[6] = {x = config.modle_radius * (circle - 1), y = 0 - sqrt3 * config.modle_radius * (circle - 1)}

        if already_calculate_num > plane_num then
            break
        else
            local cur_index = 1--代表是当前圈的第几个飞机
            local this_circle_num = self:get_plane_num_by_circle_num(circle)
            while cur_index <= this_circle_num do
                local this_index_x = 0
                local this_index_y = 0
                if cur_index >= index_of_node[1] and cur_index < index_of_node[2] then
                    local my_index_in_this_side = cur_index - index_of_node[1]--代表我是这条边的第几个点
                    this_index_x = pos_of_node[1].x - config.modle_radius * my_index_in_this_side
                    this_index_y = pos_of_node[1].y + sqrt3 * config.modle_radius * my_index_in_this_side
                elseif cur_index >= index_of_node[2] and cur_index < index_of_node[3] then
                    local my_index_in_this_side = cur_index - index_of_node[2]--代表我是这条边的第几个点
                    this_index_x = pos_of_node[2].x - 2 * config.modle_radius * my_index_in_this_side
                    this_index_y = pos_of_node[2].y
                elseif cur_index >= index_of_node[3] and cur_index < index_of_node[4] then
                    local my_index_in_this_side = cur_index - index_of_node[3]--代表我是这条边的第几个点
                    this_index_x = pos_of_node[3].x - config.modle_radius * my_index_in_this_side
                    this_index_y = pos_of_node[3].y - sqrt3 * config.modle_radius * my_index_in_this_side
                elseif cur_index >= index_of_node[4] and cur_index < index_of_node[5] then
                    local my_index_in_this_side = cur_index - index_of_node[4]--代表我是这条边的第几个点
                    this_index_x = pos_of_node[4].x + config.modle_radius * my_index_in_this_side
                    this_index_y = pos_of_node[4].y - sqrt3 * config.modle_radius * my_index_in_this_side
                elseif cur_index >= index_of_node[5] and cur_index < index_of_node[6] then
                    local my_index_in_this_side = cur_index - index_of_node[5]--代表我是这条边的第几个点
                    this_index_x = pos_of_node[5].x + 2 * config.modle_radius * my_index_in_this_side
                    this_index_y = pos_of_node[5].y
                else
                    local my_index_in_this_side = cur_index - index_of_node[6]--代表我是这条边的第几个点
                    this_index_x = pos_of_node[6].x + config.modle_radius * my_index_in_this_side
                    this_index_y = pos_of_node[6].y + sqrt3 * config.modle_radius * my_index_in_this_side
                end
                all_pos[already_calculate_num] = {x = this_index_x, y = this_index_y}
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

-- 生成c++构造函数初始化列表
--[[
{
   1, 
   {
      {0.1, 0.1},
   }
},
{
   2,
   {
      {0.1, 0.1},
      {0.1, 0.1},
   }
},

--]]
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
