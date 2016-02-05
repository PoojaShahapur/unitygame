--[[
    @brief 数组实现，类实现，数组的下标从 0 开始，但是 lua 中数组的下标从 1 开始
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Common.CmpFuncObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MListBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_funcObj = GlobalNS.new(GlobalNS.CmpFuncObject);
end

function M:dtor()

end

function M:setFuncObject(pThis, func)
    self.m_funcObj:clear();
	self.m_funcObj:setPThisAndHandle(pThis, func);
end

function M:clearFuncObject()
	self.m_funcObj = nil;
end

-- 如果 a < b 返回 -1，如果 a == b ，返回 0，如果 a > b ，返回 1
function M:cmpFunc(a, b)
	if self.m_funcObj ~= nil and self.m_funcObj:isValid() then
		return self.m_funcObj:callTwoParam(a, b);
	else
		if a < b then
			return -1;
		elseif a == b then
			return 0;
		else
			return 1;
		end
	end
end

return M;