require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject:new())
M.clsName = "TableBase"
GlobalNS[M.clsName] = M

function M:ctor(resName, tablename)
    self.m_resName = resName;
    self.m_tableName = tablename;                -- 表的名字

    self.m_List = GlobalNS.TableItemBase:new();
    self.m_byteBuffer = nil;      -- 整个表格所有的原始数据
end

return M