--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SignData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.daysCount = 0; --数量
    self.items = {};

    self.day = 0;

    self.rangeDay = 3; --签到奖励
end

function M:dtor()
    
end

function M:getSignAwards(awards)
    local index = math.random(28, 31);
    self.daysCount = index;
    for i=1, self.daysCount do
        self.items[i] = 
        {
            m_itemID = 100000;
            m_itemName = "xxx"; 
            m_itemEnable = true;           
        };
    end

    --测试用
    for i=1, 5 do
        self.items[i].m_itemEnable = false;
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUISignPanel);
    if nil ~= form and form.mIsReady then
        form:updateUIData();
    end
end

function M:setBtnState(index)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUISignPanel);
    if nil ~= form and form.mIsReady then
        form:enableBtn(index);
    end
end

return M;
--endregion
