require "MyLua/Libs/Core/Prequisites"

-- 全局变量表，自己定义的所有的变量都放在 GCtx 表中，不放在 GlobalNS 表中
GCtx = {};
local M = GCtx;
local this = GCtx;

function M.ctor()
	this.aaa = 1000;
end

function M.dtor()
	
end

return M;