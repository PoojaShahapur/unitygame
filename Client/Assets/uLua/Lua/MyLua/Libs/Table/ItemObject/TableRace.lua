require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableRaceItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_raceName = "";
end

function M:parseBodyByteBuffer(bytes, offset)
    local UtilTable = nil;
    bytes.position = offset;
    UtilTable.readString(bytes, self.m_raceName);
end

return M;