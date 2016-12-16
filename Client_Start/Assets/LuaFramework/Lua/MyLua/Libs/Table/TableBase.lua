MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableBase";
GlobalNS[M.clsName] = M;

function M:ctor(resName, tablename)
    self.mResName = resName;
    self.mTableName = tablename;                -- 表的名字

    self.mList = GlobalNS.new(GlobalNS.MList);
    self.mByteBuffer = nil;      -- 整个表格所有的原始数据
end

return M;