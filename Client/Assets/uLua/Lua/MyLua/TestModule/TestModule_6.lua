--[[  
      @brief 测试 require 实现，测试 select
]]--



local moduleName = ...
 
-- 打印参数
for i = 1, select('#', ...) do
     print(select(i, ...))
end
 
local M = {}    -- 局部的变量
_G[moduleName] = M     -- 将这个局部变量最终赋值给模块名
complex = M

function M.new(r, i) return {r = r, i = i} end
 
-- 定义一个常量i
M.i = M.new(0, 1)
 
function M.add(c1, c2)
    return M.new(c1.r + c2.r, c1.i + c2.i)
end
 
function M.sub(c1, c2)
    return M.new(c1.r - c2.r, c1.i - c2.i)
end
 
return complex  -- 返回模块的table





