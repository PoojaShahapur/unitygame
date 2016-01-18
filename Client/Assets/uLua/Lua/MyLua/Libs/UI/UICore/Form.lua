require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "Form";
GlobalNS[M.clsName] = M;

function M:ctor()

end

-- 界面代码创建后就调用
function M:onInit()
    
end

-- 第一次显示之前会调用一次
function M:onReady()
    
end

-- 每一次显示都会调用一次
function M:onShow()
    
end

-- 每一次隐藏都会调用一次
function M:onHide()

end

-- 每一次关闭都会调用一次
function M:onExit()

end

return M;