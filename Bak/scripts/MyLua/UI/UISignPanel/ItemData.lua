--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

MLoader("MyLua.UI.UISignPanel.SignPanelNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ItemData";
GlobalNS.SignPanelNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.day = 1;
    self.m_Id = 0;
    self.m_Name = "";
    self.m_Enable = false;
    self.ItemBtn = false;
	
	self.mDayConfig = nil;
end

function M:dtor()
	
end

function M:dispose()
	if(nil ~= self.ItemBtn) then
		self.ItemBtn:dispose();
		self.ItemBtn = nil;
	end
	if(nil ~= self.mDayText) then
		self.mDayText:dispose();
		self.mDayText = nil;
	end
end

function M:init(_Prefab, _Content, _day)
	self.day = _day;
	
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
    --self.m_go.transform.parent = _Content;
	GlobalNS.CSSystem.SetParent(self.m_go, _Content);
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "Item" .. _day .. "BtnTouch";

    self.ItemBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.ItemBtn:setIsDestroySelf(false);
    self.ItemBtn:setSelfGo(self.m_go);
    self.ItemBtn:addEventHandle(self, self.onItemBtnClk);
	
    self.mDayText = GlobalNS.new(GlobalNS.AuxLabel);
    self.mDayText:setIsDestroySelf(false);
	self.mDayText:setSelfGoByPath(self.m_go, "Text");
	self.mDayText:setText("" .. self.day + 1);
	
	self.mAuxObjectImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mAuxObjectImage:setIsDestroySelf(false);
	self.mAuxObjectImage:setSelfGoByPath(self.m_go, "Item_Image");
end

function M:onItemBtnClk()
	--[[
    if GCtx.mSignData.day ~= 0 then
        GCtx.mSignData:setBtnState(GCtx.mSignData.day);
    end
    
    GlobalNS.UtilApi.disableBtn(self.m_go);
	GCtx.mSignData.day = self.day;
	]]
end

function M:setDayConfig(value)
	self.mDayConfig = value;
	self.mAuxObjectImage:setObjectBaseId(GlobalNS.UtilApi.tonumber(self.mDayConfig["ObjectId"]));
end

function M:show()
	GlobalNS.UtilApi.SetActive(self.m_go, true);
end

function M:hide()
	GlobalNS.UtilApi.SetActive(self.m_go, false);
end

return M;

--endregion
