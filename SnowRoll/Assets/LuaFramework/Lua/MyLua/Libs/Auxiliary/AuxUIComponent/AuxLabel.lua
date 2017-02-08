MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxLabel";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    local params = {...};
    if(GlobalNS.UtilApi.isString(params[2])) then
        self:AuxLabel_1(...);
    elseif(GlobalNS.UtilApi.isType(params[2], 'LabelStyleID')) then
        self:AuxLabel_2(...);
    else
        self:AuxLabel_3(...);
    end
end

function M:AuxLabel_1(...)
    local pntNode, path, styleId = ...;
    if(styleId == nil) then
        styleId = GlobalNS.LabelStyleID.eLSID_None;
    end
    
    self.mSelfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
    self.mText = GlobalNS.UtilApi.getComByPath(pntNode, path, GlobalNS.AuxUITypeId.Label);
    self.mLabelStyle = GCtx.mWidgetStyleMgr:GetWidgetStyle(GlobalNS.WidgetStyleID.eWSID_Text, styleId);
    if(self.mLabelStyle:needClearText()) then
        self.mText.text = "";
    end
end

function M:AuxLabel_2(...)
    local selfNode, styleId = ...;
    if(styleId == nil) then
        styleId = GlobalNS.LabelStyleID.eLSID_None;
    end
    
    self.mSelfGo = selfNode;
    self.mText = GlobalNS.UtilApi.getComByPath(selfNode, GlobalNS.AuxUITypeId.Label);
end

function M:AuxLabel_3(...)
    local styleId = ...;
    if(styleId == nil) then
        styleId = GlobalNS.LabelStyleID.eLSID_None;
    end
end

function M:setSelfGo(pntNode, path)
    self.mSelfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
    self.mText = GlobalNS.UtilApi.getComByP(pntNode, path, GlobalNS.AuxUITypeId.Label);
end

function M:setText(value)
    if (self.mText ~= nil) then
        self.mText.text = value;
    end
end
    
function M:getText()
    if (self.mText ~= nil) then
        return self.mText.text;
    end
    return "";
end

function M:changeSize()
    self.mText.rectTransform.sizeDelta = Vector2.New(self.mText.rectTransform.sizeDelta.x, self.mText.preferredHeight);
end

return M;