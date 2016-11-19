MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableItemHeader";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_uID = 0;              -- 唯一 ID
    self.m_offset = 0;           -- 这一项在文件中的偏移
end

-- 解析头部
function M:parseHeaderByteBuffer(bytes)
    _, self.m_uID = bytes:readUnsignedInt32(self.m_uID);
    _, self.m_offset = bytes:readUnsignedInt32(self.m_offset);
end

return M;