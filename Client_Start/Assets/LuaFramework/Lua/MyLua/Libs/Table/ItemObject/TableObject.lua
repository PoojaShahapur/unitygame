--[[
    @brief 道具基本表   
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableObjectItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_name = "";
    self.m_maxNum = 0;
    self.m_type = 0;
    self.m_color = 0;
    self.m_objResName = "";
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes:setPos(offset);  -- 从偏移处继续读取真正的内容
    GlobalNS.UtilTable.readString(bytes, self.m_name);
    _, self.m_maxNum = bytes:readInt32(self.m_maxNum);
    _, self.m_type = bytes:readInt32(self.m_type);
    _, self.m_color = bytes:readInt32(self.m_color);
    GlobalNS.UtilTable.readString(bytes, self.m_objResName);
end

return M;