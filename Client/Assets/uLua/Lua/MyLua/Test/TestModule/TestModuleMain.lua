--[[  
      @brief 测试 require 实现
]]--

--[[
        有路径的 require ，一定要将返回的 table 赋值给一个局部变量，例如
--]]

package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/Tools/Tools/LuaTest/src")

-- local LocalTestModule_6 = require "TestModule.TestModule_6"
local LocalTestModule = require "TestModule.TestModule_7"

-- retTable = TestModule.TestModule_6.new(1, 2)     -- 这样是不行的
retTable = LocalTestModule.new(1, 2)

print(retTable.r)
print(retTable.i)

--[[
        没有路径的获取是可以的
--]]


-- package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/Tools/Tools/LuaTest/src/TestModule")

-- local LocalTestModule_6 = require "TestModule_6"

-- retTable = LocalTestModule_6.new(1, 2)

-- print(retTable.r)
-- print(retTable.i)


