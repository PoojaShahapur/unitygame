MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableItemHeader";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mId = 0;              -- 唯一 ID
    self.mOffset = 0;           -- 这一项在文件中的偏移
end

-- 解析头部
function M:parseHeaderByteBuffer(bytes)
    _, self.mId = bytes:readUnsignedInt32(self.mId);
    _, self.mOffset = bytes:readUnsignedInt32(self.mOffset);
end

return M;