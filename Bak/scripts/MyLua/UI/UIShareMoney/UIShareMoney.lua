MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIShareMoney.ShareMoneyNS");
MLoader("MyLua.UI.UIShareMoney.ShareMoneyData");
MLoader("MyLua.UI.UIShareMoney.ShareMoneyCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIShareMoney";
GlobalNS.ShareMoneyNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIShareMoney;
	self.mData = GlobalNS.new(GlobalNS.ShareMoneyNS.ShareMoneyData);

    self.cur_open = 0;
    self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
    self.mTabPanel.mTabClickEventDispatch:addEventHandle(self, self.onTabClick);

    self.mUrl = [[https://yf.ztgame.com?acc=]];
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

    --获取
    self.mShareFriend_Btn = GlobalNS.new(GlobalNS.AuxButton);
	self.mShareFriend_Btn:addEventHandle(self, self.onShareFriendBtnClk);
    self.mSavecode_Btn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSavecode_Btn:addEventHandle(self, self.onSavecodeBtnClk);
    self.mCopy_Btn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCopy_Btn:addEventHandle(self, self.onCopyBtnClk);
    self.mGet_Btn = GlobalNS.new(GlobalNS.AuxButton);
	self.mGet_Btn:addEventHandle(self, self.onGetClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local TopPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "TopPanel");
    self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TopPanel, "Esc_Button"));

    self:InitMainPanel(TopPanel, BG);
    self:InitChangePanel(TopPanel, BG);

    --默认打开
    self.mTabPanel:openPage(0);
end

function M:InitMainPanel(TopPanel, BG)
    local MainPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "MainPanel");
    self.mShareFriend_Btn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "ShareFriend_Btn"));
    self.mSavecode_Btn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "Savecode_Btn"));
    self.mCopy_Btn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "Copy_Btn"));
    self.mGet_Btn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "Get_Btn"));

    local ShareGetnum_text = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "ShareGetnum_text")
    self.ShareGetnum = GlobalNS.UtilApi.getComByPath(ShareGetnum_text, "Text", "Text");
    local GameGetnum_text  = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "GameGetnum_text ")
    self.GameGetnum  = GlobalNS.UtilApi.getComByPath(GameGetnum_text , "Text", "Text");
    local GetnumSum_text = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "GetnumSum_text")
    self.GetnumSum = GlobalNS.UtilApi.getComByPath(GetnumSum_text, "Text", "Text");
    local MyLink = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "MyLink");
    self.MyLinkText = GlobalNS.UtilApi.getComByPath(MyLink, "http_Text", "Text");

    local myurl = self.mUrl .. GCtx.mPlayerData.mHeroData.mMyselfAccount;
    local short_url = GlobalNS.CSSystem.Ctx.mInstance.mShareData:getShortLink();
    if not GlobalNS.UtilStr.IsNullOrEmpty(short_url) then
        myurl = short_url;
    end
    self.MyLinkText.text = myurl;
    GlobalNS.CSSystem.Ctx.mInstance.mQrCodeMgr:createQrCode(myurl, GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "QR_code"));

    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TopPanel, "GetSuger_text"), MainPanel);
    page:setTag(0);
end

function M:updateData(args)
    if 0 == self.cur_open then
        self.ShareGetnum.text = args[0] .. "";
        self.GameGetnum.text = args[1] .. "";
        self.GetnumSum.text = args[2] .. "";
    end
end

function M:InitChangePanel(BG)
    
end

function M:onTabClick(dispObj)
	local tag = dispObj:getCurPageTag();
    self.cur_open = tag;
    if 0 == tag then --获取
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:GetShareData();
    elseif 1 == tag then--兑换

    else
        
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    self.mCloseBtn:dispose();
    self.mSavecode_Btn:dispose();
    self.mCopy_Btn:dispose();
    self.mGet_Btn:dispose();
    self.mShareFriend_Btn:dispose();

    if(nil ~= self.mTabPanel) then
        self.mTabPanel.mTabClickEventDispatch:removeEventHandle(self, self.onTabClick);
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end

    M.super.onExit(self);
end

function M:onCloseBtnClk()
	self:exit();
end

function M:onShareFriendBtnClk()
	GlobalNS.CSSystem.Ctx.mInstance.mCamSys:ShareTo3Party(GCtx.mPlayerData.mHeroData.mMyselfAccount);
end

function M:onSavecodeBtnClk()
	GlobalNS.CSSystem.Ctx.mInstance.mShareUtil:save_img_to_album();
    GCtx.mGameData:ShowRollMessage("保存成功");
end

function M:onCopyBtnClk()
	--复制到剪切板
    local text = self.MyLinkText.text;
    GlobalNS.CSSystem.Ctx.mInstance.mShareUtil:copy_text_to_clipboard(text);
    GCtx.mGameData:ShowRollMessage("复制成功");
end

function M:onGetClk()
	
end

return M;