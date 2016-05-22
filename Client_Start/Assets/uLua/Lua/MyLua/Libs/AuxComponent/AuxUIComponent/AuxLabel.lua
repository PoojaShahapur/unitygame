require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxWindow"

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxLabel";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    local params = {...};
    if(GlobalNS.UtilApi.isString(params[2])) then
        self:AuxLabel_1(...);
    elseif(type(params[2]) == 'LabelStyleID') then
        self:AuxLabel_2(...);
    else
        self:AuxLabel_3(...);
    end
end

function M:AuxLabel_1(...)
    local pntNode, path, styleId = ...;
    if(styleId == nil) then
        styleId = LabelStyleID.eLSID_None;
    end
    
    self.m_selfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
    self.m_text = GlobalNS.UtilApi.getComByP(pntNode, path, 'Text');
    --self.m_labelStyle = Ctx.m_instance.m_widgetStyleMgr.GetWidgetStyle<LabelStyleBase>(WidgetStyleID.eWSID_Text, (int)styleId);
    if(self.m_labelStyle:needClearText()) then
        self.m_text.text = "";
    end
end

function M:AuxLabel_2(...)
    local selfNode, styleId = ...;
    if(styleId == nil) then
        styleId = LabelStyleID.eLSID_None;
    end
    
    self.m_selfGo = selfNode;
    self.m_text = GlobalNS.UtilApi.getComByP(selfNode, 'Text');
end

function M:AuxLabel_3(...)
    local styleId = ...;
    if(styleId == nil) then
        styleId = LabelStyleID.eLSID_None;
    end
end

function M:setSelfGo(pntNode, path)
    m_selfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
    m_text = GlobalNS.UtilApi.getComByP(pntNode, path, 'Text');
end

function M:setText(value)
    if (self.m_text ~= nil) then
        self.m_text.text = value;
    end
end
    
function M:getText()
    if (self.m_text ~= nil) then
        return self.m_text.text;
    end
    return "";
end

function M:changeSize()
    self.m_text.rectTransform.sizeDelta = Vector2.New(self.m_text.rectTransform.sizeDelta.x, self.m_text.preferredHeight);
end

return M;