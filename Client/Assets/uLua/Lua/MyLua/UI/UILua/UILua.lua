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
    print("M:onInit()")
end

function M:onReady()
    print("M:onReady()")
end

function M:onShow()
    print("M:onShow()")
end

function M:onHide()
    print("M:onHide()")
end

function M:onExit()
    print("M:onExit()")
end

return M