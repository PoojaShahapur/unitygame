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
    -- TODO: 之前使用的是 self.super ，结果如果子类中没有实现这个函数，然后这个函数又被调用了，结果直接调用父类的，而父类中有会再次调用 self 的 super 的函数，结果就死循环了，不断的调用自己
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