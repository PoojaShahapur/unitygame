MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableRaceItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_raceName = "";
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes.position = offset;
    self.m_raceName = GlobalNS.UtilTable.readString(bytes, self.m_raceName);
end

return M;