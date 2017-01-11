--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ItemData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.day = 1;
    self.m_Id = 0;
    self.m_Name = "";
    self.m_Enable = false;
end

function M:dtor()

end

function M:init(_Prefab, _Content, _day)
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
    self.m_go.transform.parent = _Content;
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "Item" .. _day .. "BtnTouch";

    self.ItemBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.ItemBtn:setSelfGo(self.m_go);
    self.ItemBtn:addEventHandle(self, self.onItemBtnClk);

    self.day = _day;
end

function M:onItemBtnClk()
    if GCtx.mSignData.day ~= 0 then
        GCtx.mSignData:setBtnState(GCtx.mSignData.day);
    end
    
    GlobalNS.UtilApi.disableBtn(self.m_go);
	GCtx.mSignData.day = self.day;
end

return M;

--endregion
