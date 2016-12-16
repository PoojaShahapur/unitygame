--[[
    @brief 技能基本表
    // 添加一个表的步骤一
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableSkillItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mName = "";               -- 名称
    self.mEffect = "";             -- 效果
    self.mSkillAttackEffect = 0;       -- 技能攻击特效
    self.mEffectMoveTime = 0;      -- 移动
    self.mIsNeedMove = 0;             -- 是否弹道特效, 0 不需要 1 需要
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes:setPos(offset);
    self.mName = GlobalNS.UtilTable.readString(bytes, self.mName);
    self.mEffect = GlobalNS.UtilTable.readString(bytes, self.mEffect);
    _, self.mSkillAttackEffect = bytes:readUnsignedInt32(self.mSkillAttackEffect);
    _, self.mIsNeedMove = bytes:readInt32(self.mIsNeedMove);

    self:initDefaultValue();
end

function M:initDefaultValue()
    if (self.mSkillAttackEffect == 0) then
        self.mSkillAttackEffect = 8;
    end

    self.mEffectMoveTime = 1;
end

return M;