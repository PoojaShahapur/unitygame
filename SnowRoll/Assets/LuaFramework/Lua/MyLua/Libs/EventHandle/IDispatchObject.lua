--[[
    @brief 可分发的对象
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.EventHandle.EventDispatch");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "IDispatchObject";
GlobalNS[M.clsName] = M;

function M:ctor()

end

return M;