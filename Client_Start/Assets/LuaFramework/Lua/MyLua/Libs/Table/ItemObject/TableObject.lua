--[[
    @brief 道具基本表   
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableObjectItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mName = "";
    self.mMaxNum = 0;
    self.mType = 0;
    self.mColor = 0;
    self.mObjResName = "";
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes:setPos(offset);  -- 从偏移处继续读取真正的内容
    self.mName = GlobalNS.UtilTable.readString(bytes, self.mName);
    _, self.mMaxNum = bytes:readInt32(self.mMaxNum);
    _, self.mType = bytes:readInt32(self.mType);
    _, self.mColor = bytes:readInt32(self.mColor);
    self.mObjResName = GlobalNS.UtilTable.readString(bytes, self.mObjResName);
end

return M;