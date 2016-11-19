--[[
    @brief 职业表
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableJobItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_jobName = "";                -- 职业名称
    self.m_jobDesc = "";                -- 职业描述
    self.m_frameImage = "";             -- 门派底图资源(这个是场景卡牌需要的资源)
    self.m_yaoDaiImage = "";            -- 卡牌名字腰带资源(这个是场景卡牌需要的资源)
    self.m_jobRes = "";                 -- 门派选择资源(门派名字资源是这个资源名字加上 __name 组成，例如这个名字是 aaa ，那么名字的资源名字就是 aaa_name)
    self.m_cardSetRes = "";             -- 门派卡组资源
    self.m_skillName = "";              -- 技能名称
    self.m_skillDesc = "";              -- 技能描述
    self.m_skillRes = "";               -- 技能图标资源
    
    self.m_jobNameRes = "";             -- 这个字段表中没有配置
    self.m_jobBtnRes = "";              -- 职业按钮资源
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes.position = offset;
    GlobalNS.UtilTable.readString(bytes, self.m_jobName);
    GlobalNS.UtilTable.readString(bytes, self.m_jobDesc);
    GlobalNS.UtilTable.readString(bytes, self.m_frameImage);
    GlobalNS.UtilTable.readString(bytes, self.m_yaoDaiImage);

    GlobalNS.UtilTable.readString(bytes, self.m_jobRes);
    GlobalNS.UtilTable.readString(bytes, self.m_cardSetRes);
    GlobalNS.UtilTable.readString(bytes, self.m_skillName);
    GlobalNS.UtilTable.readString(bytes, self.m_skillDesc);
    GlobalNS.UtilTable.readString(bytes, self.m_skillRes);

    self:initDefaultValue();
end

function M:initDefaultValue()
    if (self.m_frameImage == nil) then
        self.m_frameImage = "paidi_kapai";
    end
    if (self.m_yaoDaiImage == nil) then
        self.m_yaoDaiImage = "mingzidi_kapai";
    end
    if (self.m_cardSetRes == nil) then
        self.m_cardSetRes = "emei_taopai";
    end
    if (self.m_skillRes == nil) then
        self.m_skillRes = "emeibiao_zhiye";
    end
    if (self.m_jobRes == nil) then
        self.m_jobNameRes = "emei_zhiye";
        self.m_jobBtnRes = "gaibang_paizu";
    else
        self.m_jobNameRes = string.format("%s_name", self.m_jobRes);
        self.m_jobBtnRes = string.format("%s_btn", self.m_jobRes);
    end
    if (string.IsNullOrEmpty(self.m_jobRes)) then
        self.m_jobRes = "emei_zhiyepai";
    end
end

return M;