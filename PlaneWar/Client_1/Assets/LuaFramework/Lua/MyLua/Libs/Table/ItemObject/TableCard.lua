--[[
    @brief 卡表中的属性名字
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableCardAttrName";
GlobalNS[M.clsName] = M;

M.ChaoFeng = "嘲讽";
M.ChongFeng = "冲锋";
M.FengNu = "风怒";
M.QianXing = "潜行";
M.ShengDun = "圣盾";

M.MoFaXiaoHao = "魔法消耗";
M.GongJiLi = "攻击力";
M.Xueliang = "血量";
M.NaiJiu = "耐久";
M.FaShuShangHai = "法术伤害增加";
M.GuoZai = "过载";


--[[
    @brief 卡牌基本表
]]
M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableCardItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mName = "";        -- 名称
    self.mType = 0;           -- 类型
    self.mCareer = 0;         -- 职业
    self.mRace = 0;           -- 种族
    self.mQuality = 0;        -- 品质
    
    self.mMagicConsume = 0;   -- 魔法消耗
    self.mAttack = 0;         -- 攻击力
    self.mHp = 0;             -- 血量
    self.mDurable = 0;        -- 耐久
    
    self.mChaoFeng = 0;      -- 嘲讽
    self.mChongFeng = 0;     -- 冲锋
    self.mFengNu = 0;        -- 风怒
    self.mQianXing = 0;      -- 潜行
    self.mShengDun = 0;      -- 圣盾
    
    self.mMpAdded = 0;       -- 魔法伤害增加
    self.mGuoZai = 0;        -- 过载
    
    self.mFaShu = 0;         -- 法术
    self.mZhanHou = 0;       -- 战吼
    self.mIsNeedFaShuTarget = 0;     -- 是否需要法术目标，这个是出牌后是否需要选择目录，这个技能是否需要在目标位置释放，需要看技能表
    self.mIsNeedZhanHouTarget = 0;    -- 战吼需要目标
    self.mCardDesc = "";           -- 卡牌描述
    self.mCardHeader = "";         -- 卡牌头像贴图路径，卡牌模型中头像
    
    self.mCardSetCardHeader = "";    -- 卡牌头像贴图路径，卡组中卡牌资源
    self.mDzCardHeader = "";         -- 卡牌头像贴图路径，对战中卡牌图像
    self.mSkillPrepareEffect = 0;     -- 技能攻击准备特效
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes:setPos(offset);
    self.mName = GlobalNS.UtilTable.readString(bytes, self.mName);

    _, self.mType = bytes:readInt32(self.mType);
    _, self.mCareer = bytes:readInt32(self.mCareer);
    _, self.mRace = bytes:readInt32(self.mRace);
    _, self.mQuality = bytes:readInt32(self.mQuality);
    _, self.mMagicConsume = bytes:readInt32(self.mMagicConsume);

    _, self.mAttack = bytes:readInt32(self.mAttack);
    _, self.mHp = bytes:readInt32(self.mHp);
    _, self.mDurable = bytes:readInt32(self.mDurable);

    _, self.mChaoFeng = bytes:readInt32(self.mChaoFeng);
    _, self.mChongFeng = bytes:readInt32(self.mChongFeng);
    _, self.mFengNu = bytes:readInt32(self.mFengNu);
    _, self.mQianXing = bytes:readInt32(self.mQianXing);
    _, self.mShengDun = bytes:readInt32(self.mShengDun);
    _, self.mMpAdded = bytes:readInt32(self.mMpAdded);
    _, self.mGuoZai = bytes:readInt32(self.mGuoZai);
    _, self.mFaShu = bytes:readInt32(self.mFaShu);
    _, self.mZhanHou = bytes:readInt32(self.mZhanHou);
    _, self.mIsNeedFaShuTarget = bytes:readUnsignedInt8(self.mIsNeedFaShuTarget);
    _, self.mIsNeedZhanHouTarget = bytes:readInt32(self.mIsNeedZhanHouTarget);
    self.mCardDesc = GlobalNS.UtilTable.readString(bytes, self.mCardDesc);
    self.mCardHeader = GlobalNS.UtilTable.readString(bytes, self.mCardHeader);
    _, self.mSkillPrepareEffect = bytes:readUnsignedInt32(self.mSkillPrepareEffect);

    self:initDefaultValue();
end

function M:initDefaultValue()
    if (self.mCardHeader == nil) then
        self.mCardHeader = "gaibangzhutu_kapai";
    end

    self.mCardSetCardHeader = string.format("%s_2", self.mCardHeader);
    self.mDzCardHeader = string.format("%s_3", self.mCardHeader);
    self.mCardHeader = string.format("%s_1", self.mCardHeader);

    if (self.mSkillPrepareEffect == 0) then
        self.mSkillPrepareEffect = 4;
    end
end

return M;