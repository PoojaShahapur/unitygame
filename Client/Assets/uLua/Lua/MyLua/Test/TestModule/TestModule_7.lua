--[[  
      @brief 测试 require 实现，测试 select
]]--

-- ... 一定要放在 module 的前面，因为 module 后环境就改变了， select 是访问不到的 
-- 打印参数
for i = 1, select('#', ...) do
     print(select(i, ...))
end


module(...)


function new(r, i) return {r = r, i = i} end


-- 定义一个常量i
i = new(0, 1)

 
function add(c1, c2)
    return new(c1.r + c2.r, c1.i + c2.i)
end

 
function sub(c1, c2)
    return new(c1.r - c2.r, c1.i - c2.i)
end






