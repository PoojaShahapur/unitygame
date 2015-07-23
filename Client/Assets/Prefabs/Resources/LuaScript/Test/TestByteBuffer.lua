-- 测试字节缓冲区

local packagePath = package.path
package.path = string.format("%s;%s/?.lua", packagePath, "E:/Self/Self/unity/unitygame/Client/Assets/Prefabs/Resources/LuaScript")

require('DataStruct/ByteBuffer')
require('DataStruct/Class')

ByteBuffer.m_buff = {3, 1, '2', 3, 4, 5, 6, 7, 8, 9}

-- local _int8 = ByteBuffer.readInt8()
-- local _int16 = ByteBuffer.readInt16()

-- print(_int8)
-- print(_int16)

--local buIns = class(ByteBuffer)
--a = buIns.new(1)
a = ByteBuffer.new()
a:writeMultiByte("aaaa")

b = ByteBuffer.new()
b:writeMultiByte("bbb")