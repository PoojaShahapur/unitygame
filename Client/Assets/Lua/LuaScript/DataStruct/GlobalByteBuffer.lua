-- 各种需要的 ByteBuffer

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