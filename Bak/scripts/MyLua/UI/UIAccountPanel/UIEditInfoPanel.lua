MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelData");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIEditInfoPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIEditInfoPanel;
	self.mData = GlobalNS.new(GlobalNS.AccountPanelNS.AccountPanelData);

    self.index = 1;
    self.EditDone = true;

    self.old_username = "";
    self.old_age = 1;
    self.old_sex = 1;
    self.old_areacode = 1;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    self.mNameEditBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNameEditBtn:addEventHandle(self, self.onEditBtnClk);

	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

    self.mAvatarImage = GlobalNS.new(GlobalNS.AuxImage);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Close_BtnTouch"));

    local PersonalInfo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "PersonalInfo");
    local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(PersonalInfo, "Avatar");
    local Info = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Info");
    self.mAvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Avatar_Img"));
    self.mName = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Name");
    self.mNameEditBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "NameEdit_Btn"));
    self.mNameInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "NameInput");
    self.mAge = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Age");
    self.nameinputText = GlobalNS.UtilApi.GetComponent(self.mNameInput, "InputField");
    self.ageText = GlobalNS.UtilApi.GetComponent(self.mAge, "InputField");
    self.nametext = GlobalNS.UtilApi.GetComponent(self.mName, "Text");
    GlobalNS.UtilApi.addInputEndHandle(self.mAge, self, self.onAgeInputEnd);

    local Sex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Sex");
    self.Man = GlobalNS.UtilApi.getComByPath(Sex, "Man", "Toggle");
    self.Woman = GlobalNS.UtilApi.getComByPath(Sex, "WoMan", "Toggle");

    self.Country = GlobalNS.UtilApi.getComByPath(Info, "Country", "Dropdown");

    self:setInfo();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    self.mNameEditBtn:dispose();
    self.mCloseBtn:dispose();
    self.mAvatarImage:dispose();
    M.super.onExit(self);
end

function M:onAgeInputEnd(text)
    local num = GlobalNS.UtilApi.GetTextWordNum(text);
    local age = tonumber(text);
    local age_r = age < 6 or age > 80;
    if num > 2 or string.find(text,"0") == 1 or age_r then
         GCtx.mGameData:ShowRollMessage("我相信你的年龄不正确，0.0");
         self.ageText.text = self.old_age;
    end
end

function M:onEditBtnClk()
    if self.EditDone then
        self.mName:SetActive(false);
        self.mNameInput:SetActive(true);
        self.EditDone = false;
    else
        local newname = self.nameinputText.text;
        newname = string.gsub(newname, "^%s*(.-)%s*$", "%1"); --去除两端的空格
        if newname == '' then
            newname = "";
        end

        if string.len(newname) == 0 then
            GCtx.mGameData:ShowMessageBox("用户名不能为空");
            return;
        end

        local isnospaceorper = false;
        isnospaceorper = GlobalNS.UtilApi.GetIsContainKeyword(newname);
        if isnospaceorper then
            GCtx.mGameData:ShowRollMessage("用户名中不能包含<color=#00FF00FF>空格</color>和<color=#00FF00FF>%</color>，请重新设置");
            return;
        end

        local isFilter = GlobalNS.CSSystem.Ctx.mInstance.mWordFilterManager:IsMatch(newname);
        if isFilter then
            GCtx.mGameData:ShowRollMessage("用户名中含有敏感词，请重新设置");
            return;
        end

        self.nametext.text = self.nameinputText.text;
        self.mName:SetActive(true);
        self.mNameInput:SetActive(false);
        self.EditDone = true;
        if self.old_username ~= newname then
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ChangeAccount(newname);
        end
    end
end

function M:onCloseBtnClk()
    local newage = tonumber(self.ageText.text);
    local newsex = 1;
    if self.Man.isOn then
        newsex = 0;
    else
        newsex = 1;
    end
    local area_code = self.Country.value + 1; --value从0开始

    local ischanged = false; --是否有修改
    ischanged = (self.old_age ~= newage) or (self.old_sex ~= newsex) or (self.old_areacode ~= area_code);
    if not ischanged then
        self:exit();
        return;
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:updatePersonalInfo(newsex, newage, area_code);
    end
    
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ChangePersonalInfo(newsex, tonumber(newage), area_code);
    --Debugger.Log(newname.. " " .. newage .. " " .. newsex .. " " .. newcountry .. " " .. area_code);
	self:exit();
end

function M:setInfo()
    local info = {};
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        info = {form:getPersonalInfo()};
    end

	self.mAvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(GCtx.mPlayerData.mHeroData.mAvatarIndex));

    self.name = info[2];
    self.nametext.text = self.name;
    self.nameinputText.text = self.name;

    self.ageText.text = info[4];

    local sex = info[3];
    if 0 == sex then
        self.Man.isOn = true;
        self.Woman.isOn = false;
    else
        self.Woman.isOn = true;
        self.Man.isOn = false;
    end

    local area_code = info[5];
    self.Country.captionText.text = GCtx.mSocialData.citys[area_code];

    self.old_username = self.name;
    self.old_age = info[4];
    self.old_sex = info[3];
    self.old_areacode = area_code;
end

return M;