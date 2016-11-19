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
    self.m_name = "";        -- 名称
    self.m_type = 0;           -- 类型
    self.m_career = 0;         -- 职业
    self.m_race = 0;           -- 种族
    self.m_quality = 0;        -- 品质
    
    self.m_magicConsume = 0;   -- 魔法消耗
    self.m_attack = 0;         -- 攻击力
    self.m_hp = 0;             -- 血量
    self.m_Durable = 0;        -- 耐久
    
    self.m_chaoFeng = 0;      -- 嘲讽
    self.m_chongFeng = 0;     -- 冲锋
    self.m_fengNu = 0;        -- 风怒
    self.m_qianXing = 0;      -- 潜行
    self.m_shengDun = 0;      -- 圣盾
    
    self.m_mpAdded = 0;       -- 魔法伤害增加
    self.m_guoZai = 0;        -- 过载
    
    self.m_faShu = 0;         -- 法术
    self.m_zhanHou = 0;       -- 战吼
    self.m_bNeedFaShuTarget = 0;     -- 是否需要法术目标，这个是出牌后是否需要选择目录，这个技能是否需要在目标位置释放，需要看技能表
    self.m_bNeedZhanHouTarget = 0;    -- 战吼需要目标
    self.m_cardDesc = "";           -- 卡牌描述
    self.m_cardHeader = "";         -- 卡牌头像贴图路径，卡牌模型中头像
    
    self.m_cardSetCardHeader = "";    -- 卡牌头像贴图路径，卡组中卡牌资源
    self.m_dzCardHeader = "";         -- 卡牌头像贴图路径，对战中卡牌图像
    self.m_skillPrepareEffect = 0;     -- 技能攻击准备特效
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes:setPos(offset);
    self.m_name = GlobalNS.UtilTable.readString(bytes, self.m_name);

    _, self.m_type = bytes:readInt32(self.m_type);
    _, self.m_career = bytes:readInt32(self.m_career);
    _, self.m_race = bytes:readInt32(self.m_race);
    _, self.m_quality = bytes:readInt32(self.m_quality);
    _, self.m_magicConsume = bytes:readInt32(self.m_magicConsume);

    _, self.m_attack = bytes:readInt32(self.m_attack);
    _, self.m_hp = bytes:readInt32(self.m_hp);
    _, self.m_Durable = bytes:readInt32(self.m_Durable);

    _, self.m_chaoFeng = bytes:readInt32(self.m_chaoFeng);
    _, self.m_chongFeng = bytes:readInt32(self.m_chongFeng);
    _, self.m_fengNu = bytes:readInt32(self.m_fengNu);
    _, self.m_qianXing = bytes:readInt32(self.m_qianXing);
    _, self.m_shengDun = bytes:readInt32(self.m_shengDun);
    _, self.m_mpAdded = bytes:readInt32(self.m_mpAdded);
    _, self.m_guoZai = bytes:readInt32(self.m_guoZai);
    _, self.m_faShu = bytes:readInt32(self.m_faShu);
    _, self.m_zhanHou = bytes:readInt32(self.m_zhanHou);
    _, self.m_bNeedFaShuTarget = bytes:readUnsignedInt8(self.m_bNeedFaShuTarget);
    _, self.m_bNeedZhanHouTarget = bytes:readInt32(self.m_bNeedZhanHouTarget);
    self.m_cardDesc = GlobalNS.UtilTable.readString(bytes, self.m_cardDesc);
    self.m_cardHeader = GlobalNS.UtilTable.readString(bytes, self.m_cardHeader);
    _, self.m_skillPrepareEffect = bytes:readUnsignedInt32(self.m_skillPrepareEffect);

    self:initDefaultValue();
end

function M:initDefaultValue()
    if (self.m_cardHeader == nil) then
        self.m_cardHeader = "gaibangzhutu_kapai";
    end

    self.m_cardSetCardHeader = string.format("%s_2", self.m_cardHeader);
    self.m_dzCardHeader = string.format("%s_3", self.m_cardHeader);
    self.m_cardHeader = string.format("%s_1", self.m_cardHeader);

    if (self.m_skillPrepareEffect == 0) then
        self.m_skillPrepareEffect = 4;
    end
end

return M;