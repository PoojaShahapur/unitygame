MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableStateItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mName = "";           -- 名称
    self.mRes = "";            -- 资源
    self.mEffectId = 0;        -- 特效 Id
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes.position = offset;
    self.mName = GlobalNS.UtilTable.readString(bytes, self.mName);
    self.mRes = GlobalNS.UtilTable.readString(bytes, self.mRes);
    _, self.mEffectId = bytes:readInt32(self.mEffectId);

    self:initDefaultValue();
end

function M:initDefaultValue()
    if(self.mEffectId == 0) then
        self.mEffectId = 0;
    end
end

return M;