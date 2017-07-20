--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "GoodsItemData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.m_Id = 0;
    self.m_Name = "";
    self.m_NeedId = 0;
    self.m_NeedNum = 0;
    self.buyItemBtn = nil;
end

function M:dtor()
    self.m_go:dispose();
    self.buyItemBtn:dispose();
end

function M:init(_Prefab, _Content, _index, isOwn)
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
    --self.m_go.transform.parent = _Content;
	GlobalNS.CSSystem.SetParent(self.m_go.transform, _Content);
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "GoodsItem" .. _index;

    self.buyItemBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.buyItemBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "BuyItem_BtnTouch"));
    if isOwn then
        self.buyItemBtn:addEventHandle(self, self.onUseBtnClk);
    else
        self.buyItemBtn:addEventHandle(self, self.onBuyBtnClk);
    end
end

function M:onUseBtnClk()
	
end

function M:onBuyBtnClk()
	local haixing = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt(GCtx.mGoodsData.HaiXingId);
    local zhenzhu = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt(GCtx.mGoodsData.ZhenZhuId);

    if self.m_NeedId == tonumber(GCtx.mGoodsData.HaiXingId) then
        if haixing >= self.m_NeedNum then
            haixing = haixing - self.m_NeedNum;
        else
            GCtx.mLogSys:log("海星不足", GlobalNS.LogTypeId.eLogCommon);
        end        
    end

    if self.m_NeedId == tonumber(GCtx.mGoodsData.ZhenZhuId) then
        if zhenzhu >= self.m_NeedNum then
            zhenzhu = zhenzhu - self.m_NeedNum;
        else
            GCtx.mLogSys:log("珍珠不足", GlobalNS.LogTypeId.eLogCommon);
        end
    end

    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIShop_SkinPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIShop_SkinPanel);
        if nil ~= form and form.mIsReady then
            form:refreshGoldNum(haixing, zhenzhu);
        end
    end
end

return M;

--endregion
