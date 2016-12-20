MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxInputField";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    local pntNode, path, styleId = ...;
    if(styleId == nil) then
        styleId = GlobalNS.BtnStyleID.eBSID_None;
    end
    self.mSelfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
    self.mInputField = GlobalNS.UtilApi.getComByPath(pntNode, path, GlobalNS.AuxUITypeId.InputField);
end

function M:setText(value)
    self.mInputField.text = value;
end

function M:getText()
    return self.mInputField.text;
end

return M;