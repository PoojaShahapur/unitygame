-- 各种需要的 ByteBuffer

package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/unity/unitygame/Client/Assets/Prefabs/Resources")

require('LuaScript/DataStruct/ByteBuffer')
 
NetMsgData = ByteBuffer.new()
--NetMsgData = {}

-- 输出测试
function NetMsgData:TestOut()
	self:log("TestOut")
	self:clear()
    --local _int16 = self:readInt16()
    self:dumpAllBytes()
end 

-- 给 C# 提供接口，因为 C# 中获取 Lua 函数，如果这个函数是通过元表指定的，是获取不到的，必须自己手工添加到表中的才能获取到
function NetMsgData:writeInt8FromCS(oneByte)
	self:writeInt8(oneByte)
end

function NetMsgData:clearFromCS(oneByte)
	self:clear()
end

local int16_ = NetMsgData:readInt16()

local aaa = 10