--[[  
      @brief 这个是通过 __index 方法实现的表获取值
]]--

Window = {}

Window.prototype = {x = 0, y = 0, width = 100, height = 100}
Window.mt = {}

function Window.new(o)
    setmetatable(o, Window.mt)
    return o
end

--现在定义一个元方法
Window.mt.__index = function(table, key)
    return Window.prototype[key]
end


-- w = Window.new{x=10,y=20}   -- 调用的时候不用这么写吗 Window.new({x=10,y=20})
w = Window.new({x=10,y=20}) 
print(w.width)  -- 100 window实际上没有width这个字段



