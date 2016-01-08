require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.UI.UICore.Form"

local M = GlobalNS.Class(GlobalNS.Form)
M.clsName = "UILua"
GlobalNS[M.clsName] = M

function M:ctor()
    print("M:ctor()")
    print(tostring(self))
end

function M:onInit()
    M.super.onInit(self)
    print("M:onInit()")
end

function M:onReady()
    -- self.super.onReady(self)
    M.super.onReady(self)
    print("M:onReady()")
    --GlobalNS.CSImportToLua.UtilApi.addEventHandle(self.gameObject, self.onBtnClk);
	SDK.Lib.UtilApi.addEventHandle(self.gameObject, "Button", self.onBtnClk);
end

function M:onShow()
    M.super.onShow(self)
    print("M:onShow()")
end

function M:onHide()
    M.super.onHide(self)
    print("M:onHide()")
end

function M:onExit()
    M.super.onExit(self)
    print("M:onExit()")
end

function M:onBtnClk()

end

return M