MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "WidgetStyleMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_style2Dic = GlobalNS.new(GlobalNS.MDictionary);
    self:registerStype();
end

function M:addWidgetStype(widgetId, comId, style)
    if(not self.m_style2Dic:ContainsKey(widgetId)) then
        local styleDic = GlobalNS.new(GlobalNS.MDictionary);
        self.m_style2Dic:Add(widgetId, styleDic);
    end
    self.m_style2Dic:value(widgetId):Add(comId, style);
end

function M:GetWidgetStyle(widgetId, comId)
    return self.m_style2Dic:value(widgetId):value(comId);
end

function M:registerStype()
    local lblStyle = GlobalNS.new(GlobalNS.LabelStyleBase);
    self:addWidgetStype(GlobalNS.WidgetStyleID.eWSID_Text, GlobalNS.LabelStyleID.eLSID_None, lblStyle);

    local btnStyle = GlobalNS.new(GlobalNS.ButtonStyleBase);
    self:addWidgetStype(GlobalNS.WidgetStyleID.eWSID_Button, GlobalNS.BtnStyleID.eBSID_None, btnStyle);
end

return M;