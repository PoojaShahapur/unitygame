--[[
    @brief 职业表
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.TableItemBodyBase);
M.clsName = "TableJobItemBody";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mJobName = "";                -- 职业名称
    self.mJobDesc = "";                -- 职业描述
    self.mFrameImage = "";             -- 门派底图资源(这个是场景卡牌需要的资源)
    self.mYaoDaiImage = "";            -- 卡牌名字腰带资源(这个是场景卡牌需要的资源)
    self.mJobRes = "";                 -- 门派选择资源(门派名字资源是这个资源名字加上 __name 组成，例如这个名字是 aaa ，那么名字的资源名字就是 aaa_name)
    self.mCardSetRes = "";             -- 门派卡组资源
    self.mSkillName = "";              -- 技能名称
    self.mSkillDesc = "";              -- 技能描述
    self.mSkillRes = "";               -- 技能图标资源
    
    self.mJobNameRes = "";             -- 这个字段表中没有配置
    self.mJobBtnRes = "";              -- 职业按钮资源
end

function M:parseBodyByteBuffer(bytes, offset)
    bytes.position = offset;
    self.mJobName = GlobalNS.UtilTable.readString(bytes, self.mJobName);
    self.mJobDesc = GlobalNS.UtilTable.readString(bytes, self.mJobDesc);
    self.mFrameImage = GlobalNS.UtilTable.readString(bytes, self.mFrameImage);
    self.mYaoDaiImage = GlobalNS.UtilTable.readString(bytes, self.mYaoDaiImage);

    self.mJobRes = GlobalNS.UtilTable.readString(bytes, self.mJobRes);
    self.mCardSetRes = GlobalNS.UtilTable.readString(bytes, self.mCardSetRes);
    self.mSkillName = GlobalNS.UtilTable.readString(bytes, self.mSkillName);
    self.mSkillDesc = GlobalNS.UtilTable.readString(bytes, self.mSkillDesc);
    self.mSkillRes = GlobalNS.UtilTable.readString(bytes, self.mSkillRes);

    self:initDefaultValue();
end

function M:initDefaultValue()
    if (self.mFrameImage == nil) then
        self.mFrameImage = "paidi_kapai";
    end
    if (self.mYaoDaiImage == nil) then
        self.mYaoDaiImage = "mingzidi_kapai";
    end
    if (self.mCardSetRes == nil) then
        self.mCardSetRes = "emei_taopai";
    end
    if (self.mSkillRes == nil) then
        self.mSkillRes = "emeibiao_zhiye";
    end
    if (self.mJobRes == nil) then
        self.mJobNameRes = "emei_zhiye";
        self.mJobBtnRes = "gaibang_paizu";
    else
        self.mJobNameRes = string.format("%s_name", self.mJobRes);
        self.mJobBtnRes = string.format("%s_btn", self.mJobRes);
    end
    if (GlobalNS.UtilStr.IsNullOrEmpty(self.mJobRes)) then
        self.mJobRes = "emei_zhiyepai";
    end
end

return M;