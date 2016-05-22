require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxWindow"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxUITypeId"

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxBasicButton";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self:AuxBasicButton_1(...);
end

function M:dtor()
	
end

function M:dispose()
	if (self.m_eventDisp ~= nil) then
        GlobalNS.UtilApi.RemoveListener(self.m_btn, self.onBtnClk);
    end
    M.super.dispose(self);
end

function M:AuxBasicButton_1(...)
    local pntNode, path, styleId = ...;
    if(path == nil) then
        path = '';
    end
    if(styleId == nil) then
        styleId = GlobalNS.BtnStyleID.eBSID_None;
    end
    
    self.m_eventDisp = GlobalNS.new(GlobalNS.EventDispatch);
    if (pntNode ~= nil) then
        self.m_selfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        self:updateBtnCom(nil);
    end
end

function M:updateBtnCom(dispObj)
    self.m_btn = GlobalNS.UtilApi.getComFromSelf(self.m_selfGo, GlobalNS.AuxUITypeId.Button);
    GlobalNS.UtilApi.addEventHandle(self.m_btn, self.onBtnClk);
end

function M:enable()
    self.m_btn.interactable = true;
end

function M:disable()
    self.m_btn.interactable = false;
end

-- 点击回调
function M:onBtnClk()
    self.m_eventDisp.dispatchEvent(self);
end

function M:addEventHandle(btnClk)
    self.m_eventDisp.addEventHandle(btnClk);
end

function M:syncUpdateCom()

end

return M;