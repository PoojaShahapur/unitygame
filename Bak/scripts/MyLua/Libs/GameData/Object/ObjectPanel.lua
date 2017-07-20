MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief ObjectPanel 一个 Panel 数据
]]

local M = GlobalNS.Class(GlobalNS.ObjectPanelBase);
M.clsName = "ObjectPanel";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end



return M;