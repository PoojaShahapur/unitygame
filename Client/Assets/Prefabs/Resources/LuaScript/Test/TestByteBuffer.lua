-- 测试字节缓冲区

local packagePath = package.path
package.path = string.format("%s;%s/?.lua", packagePath, "E:/Self/Self/LuaByteBuffer-git/lua")
require('LuaLib/ByteBuffer')

ByteBuffer.m_buff = {"22", 1, '2', 3, 4, 5, 6, 7, 8, 9}
local retInt8 = 0
local _int8 = ByteBuffer.readInt8(retInt8)
local _int16 = ByteBuffer.readInt16()

print(_int8)
print(_int16)