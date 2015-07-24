-- 测试字节缓冲区

local packagePath = package.path
package.path = string.format("%s;%s/?.lua", packagePath, "E:/Self/Self/unity/unitygame/Client/Assets/Prefabs/Resources/LuaScript")

require('LuaScript/DataStruct/ByteBuffer')
require('LuaScript/DataStruct/Class')

ByteBuffer.m_buff = {3, 1, '2', 3, 4, 5, 6, 7, 8, 9}

local tbl = {}
tbl[0] = 1
tbl[1] = 2
tbl[2] = 3

-- local _int8 = ByteBuffer.readInt8()
-- local _int16 = ByteBuffer.readInt16()

-- print(_int8)
-- print(_int16)

--local buIns = class(ByteBuffer)
--a = buIns.new(1)
local a = ByteBuffer.new()
a:writeMultiByte("aaaa")

local b = ByteBuffer.new()
b:writeMultiByte("bbb")

local c = 10