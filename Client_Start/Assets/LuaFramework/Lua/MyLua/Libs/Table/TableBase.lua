MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableBase";
GlobalNS[M.clsName] = M;

function M:ctor(resName, tablename)
    self.m_resName = resName;
    self.m_tableName = tablename;                -- 表的名字

    self.m_List = GlobalNS.new(GlobalNS.TableItemBase);
    self.m_byteBuffer = nil;      -- 整个表格所有的原始数据
end

return M;