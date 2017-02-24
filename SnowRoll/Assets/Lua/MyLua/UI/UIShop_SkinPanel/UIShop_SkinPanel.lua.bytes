MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIShop_SkinPanel.Shop_SkinPanelNS");
MLoader("MyLua.UI.UIShop_SkinPanel.Shop_SkinPanelData");
MLoader("MyLua.UI.UIShop_SkinPanel.Shop_SkinPanelCV");
MLoader("MyLua.UI.UIShop_SkinPanel.GoodsItemData");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIShop_SkinPanel";
GlobalNS.Shop_SkinPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIShop_SkinPanel;
	self.mData = GlobalNS.new(GlobalNS.Shop_SkinPanelNS.Shop_SkinPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    --商店类别
    self.mSkinBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSkinBtn:addEventHandle(self, self.onSkinBtnClk);
    --[[待扩充
    self.mSkinBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSkinBtn:addEventHandle(self, self.onSkinBtnClk);
    self.mSkinBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSkinBtn:addEventHandle(self, self.onSkinBtnClk);
	]]--
    self.mHaiXingBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mHaiXingBtn:addEventHandle(self, self.onHaiXingBtnClk);
    self.mZhenZhuBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mZhenZhuBtn:addEventHandle(self, self.onZhenZhuBtnClk);
    
    --物品类别
    self.mShapeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mShapeBtn:addEventHandle(self, self.onShapeBtnClk);
    self.mChildBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mChildBtn:addEventHandle(self, self.onChildBtnClk);
    self.mFilterToggle = nil;
    --goodsitem prefab
    self.mGoodsitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mGoodsitem_prefab:setIsNeedInsPrefab(false);
    self.isPrefabLoaded = false;
    --goodsitems gameobject数组
    self.goodsitems = { };

    --返回
	self.mBackBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackBtn:addEventHandle(self, self.onBackBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    --顶部
    local TitleBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "TitleBGImage");
	self.mSkinBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitleBG, "Skin_BtnTouch"));
    --[[待扩充
    self.mSkinBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitleBG, "Skin_BtnTouch"));
    self.mSkinBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitleBG, "Skin_BtnTouch"));
    ]]--
    local HaiXingBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitleBG, "HaiXingBG");
    self.HaiXingNumText = GlobalNS.UtilApi.getComByPath(HaiXingBG, "Num", "Text");
    self.mHaiXingBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(HaiXingBG, "HaiXing_BtnTouch"));

    local ZhenZhuBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitleBG, "ZhenZhuBG");
    self.ZhenZhuNumText = GlobalNS.UtilApi.getComByPath(ZhenZhuBG, "Num", "Text");
    self.mZhenZhuBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(ZhenZhuBG, "ZhenZhu_BtnTouch"));

    --商品区
    local MiddlePanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "MiddlePanel");
    local Middle_TopPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MiddlePanel, "TopPanel");
    self.mShapeBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Middle_TopPanel, "Shape_BtnTouch"));
    self.mChildBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Middle_TopPanel, "Child_BtnTouch"));
    self.StateText = GlobalNS.UtilApi.getComByPath(Middle_TopPanel, "StateText", "Text");

    self.mFilterToggle = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Middle_TopPanel, "FilterToggle")
    GlobalNS.UtilApi.addToggleHandle(self.mFilterToggle, self, self.onValueChanged);
    --[[下拉列表待补充]]--

    self.scrollrect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MiddlePanel, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.scrollrect, "Viewport");
    self.GoodsContentRect = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");
    self.Tip = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MiddlePanel, "Tip");
    self.Tip:SetActive(false);
    --加载goodsitems
	self.mGoodsitem_prefab:asyncLoad("UI/UIShop_SkinPanel/GoodsItem.prefab", self, self.onPrefabLoaded, nil);    

    --底部
    local BottomBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "BottomBGImage");
	self.mBackBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BottomBG, "Back_BtnTouch"));

    --货币
    if not GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey(GCtx.mGoodsData.HaiXingId) then
        self:refreshGoldNum(10000, 10000);
    else
        local haixing = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt(GCtx.mGoodsData.HaiXingId);
        local zhenzhu = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt(GCtx.mGoodsData.ZhenZhuId);
        self:refreshGoldNum(haixing, zhenzhu);
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
end

function M:onSkinBtnClk()
	GCtx.mGoodsData.CurrentShopType = GlobalNS.UIFormId.eUIShop_SkinPanel;
end
--[[待扩充
function M:onSkinBtnClk()
	GCtx.mLogSys:log("Shop Skin Clk", GlobalNS.LogTypeId.eLogCommon);
end

function M:onSkinBtnClk()
	GCtx.mLogSys:log("Shop Skin Clk", GlobalNS.LogTypeId.eLogCommon);
end
]]--

function M:onHaiXingBtnClk()
	GCtx.mLogSys:log("海星： " .. self.HaiXingNumText.text, GlobalNS.LogTypeId.eLogCommon);
end

function M:onZhenZhuBtnClk()
	GCtx.mLogSys:log("珍珠： " .. self.ZhenZhuNumText.text, GlobalNS.LogTypeId.eLogCommon);
end

function M:onShapeBtnClk()
    self.StateText.text = "正在出售";
    GCtx.mGoodsData.CurrentGoodsType = 1;
    GCtx.mGoodsData:getGoodsItems(GCtx.mGoodsData.CurrentShopType, GCtx.mGoodsData.CurrentGoodsType);
end

function M:onChildBtnClk()
    self.StateText.text = "正在出售";
    GCtx.mGoodsData.CurrentGoodsType = 2;
    GCtx.mGoodsData:getGoodsItems(GCtx.mGoodsData.CurrentShopType, GCtx.mGoodsData.CurrentGoodsType);
end

function M:onPrefabLoaded(dispObj)
    --获取goodsitem prefab对象
    self.mGoodsItemPrefab = self.mGoodsitem_prefab:getPrefabTmpl();
    self.isPrefabLoaded = true;
    GCtx.mGoodsData.CurrentGoodsType = 1;
    GCtx.mGoodsData:getGoodsItems(GCtx.mGoodsData.CurrentShopType, GCtx.mGoodsData.CurrentGoodsType); --初始打开
end

function M:CreateGoodsItem(isOwn)
    if not self.isPrefabLoaded then
        return;
    end

    --清空
    for i=1, #self.goodsitems do
        local goodsitem = self.goodsitems[i];
        GlobalNS.UtilApi.Destroy(goodsitem.m_go);
    end
    self.goodsitems = {};

    --重新生成
    for i=1, GCtx.mGoodsData.goodsCount do
        local goodsitem = GlobalNS.new(GlobalNS.GoodsItemData);
        
        --用goodsitem prefab生成GameObject对象
        goodsitem:init(self.mGoodsItemPrefab, self.GoodsContentRect, i, isOwn);
        self.goodsitems[i] = goodsitem;
    end

    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.GetComponent(self.scrollrect, "ScrollRect").verticalNormalizedPosition = 1;
end

function M:updateUIData(isOwn)
    if GCtx.mGoodsData.goodsCount == 0 then
        self.scrollrect:SetActive(false);
        self.Tip:SetActive(true);
        return;
    else
        self.scrollrect:SetActive(true);
        self.Tip:SetActive(false);
    end

    self:CreateGoodsItem(isOwn);

    if #self.goodsitems == GCtx.mGoodsData.goodsCount then        
        self:SetGoodsItems(isOwn);
    end
end

--物品数据
function M:SetGoodsItems(isOwn)
    for i=1, GCtx.mGoodsData.goodsCount do
        local goodsitem = self.goodsitems[i].m_go.transform;
        
        --热卖标识
        local HotImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.goodsitems[i].m_go, "HotImage");
        local Hot = GlobalNS.UtilApi.getComByPath(HotImage, "HotText", "Text");
        Hot.text = GCtx.mGoodsData.goodsitems[i].m_hot;

        --形状
        local Avatar = GlobalNS.UtilApi.getComByPath(goodsitem, "GoodsImage", "Image");
        --Sprite avatarSprite = Resources.Load("Avatar/DefaultAvatar", typeof(Sprite)) as Sprite;
        --Avatar.overrideSprite = avatarSprite;

        --物品名
        local Name = GlobalNS.UtilApi.getComByPath(goodsitem, "GoodsName", "Text");
        Name.text = GCtx.mGoodsData.goodsitems[i].m_goodsName;        

        --材料数量
        local BuyItemBtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.goodsitems[i].m_go, "BuyItem_BtnTouch");
        if isOwn then --已拥有物品
            local usingStr = "使用";
            if GCtx.mGoodsData.goodsitems[i].m_needNum == 0 then
                usingStr = "使用";
            else
                usingStr = "已使用";
            end
            GlobalNS.UtilApi.getComByPath(BuyItemBtn, "Num", "Text").text = usingStr;
        else
            GlobalNS.UtilApi.getComByPath(BuyItemBtn, "Num", "Text").text = GCtx.mGoodsData.goodsitems[i].m_needNum;
            --GlobalNS.UtilApi.getComByPath(BuyItemBtn, "Kind", "Image");
        end

        --专属标识
        local OnlyImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.goodsitems[i].m_go, "OnlyImage");
        local Only = GlobalNS.UtilApi.getComByPath(OnlyImage, "OnlyText", "Text");
        Only.text = GCtx.mGoodsData.goodsitems[i].m_only;

        --商品数据
        self.goodsitems[i].m_Id = GCtx.mGoodsData.goodsitems[i].m_goodsID;
        self.goodsitems[i].m_Name = GCtx.mGoodsData.goodsitems[i].m_goodsName;
        self.goodsitems[i].m_NeedId = GCtx.mGoodsData.goodsitems[i].m_needID;
        self.goodsitems[i].m_NeedNum = GCtx.mGoodsData.goodsitems[i].m_needNum;
    end
end

function M:refreshGoldNum(HaiXingNum, ZhenZhuNum)
	GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt(GCtx.mGoodsData.HaiXingId, HaiXingNum);
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt(GCtx.mGoodsData.ZhenZhuId, ZhenZhuNum);

    self.HaiXingNumText.text = HaiXingNum;
    self.ZhenZhuNumText.text = ZhenZhuNum;
end

function M:onBackBtnClk()
    --清空
    for i=1, #self.goodsitems do
        local goodsitem = self.goodsitems[i];
        GlobalNS.UtilApi.Destroy(goodsitem.m_go);
    end
    self.goodsitems = {};
    self.mGoodsitem_prefab:dispose();
	self:exit();
end

function M:onValueChanged(check)
    if check then
        GCtx.mLogSys:log("true", GlobalNS.LogTypeId.eLogCommon);
    else
        GCtx.mLogSys:log("false", GlobalNS.LogTypeId.eLogCommon);
    end
end

return M;