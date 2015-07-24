-- 各种需要的 ByteBuffer

require('LuaScript/DataStruct/ByteBuffer')
 
NetMsgData = ByteBuffer.new()

-- 输出测试
function NetMsgData:TestOut()
    local _int16 = self:readInt16()
    self:dumpAllBytes()
end 