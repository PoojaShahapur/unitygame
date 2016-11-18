require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableItemHeader";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_uID = 0;              -- 唯一 ID
    self.m_offset = 0;           -- 这一项在文件中的偏移
end

-- 解析头部
function M:parseHeaderByteBuffer(bytes)
    bytes.readUnsignedInt32(self.m_uID);
    bytes.readUnsignedInt32(self.m_offset);
end

return M;