MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableRaceItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mRaceName = "";
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes.position = offset;
    self.mRaceName = GlobalNS.UtilTable.readString(bytes, self.mRaceName);
end

return M;