--[[  
      @brief 这个是错误的直接在 metatable 中添加属性，导致获取不到值
]]--

Window = {}


Window.prototype = {x = 0, y = 0, width = 100, height = 100}
Window.mt = {}
Window.member = 10


function Window.new(o)
    setmetatable(o, Window.mt)
    return o
end


w = Window.new({x=10,y=20}) 
print(w.member)




