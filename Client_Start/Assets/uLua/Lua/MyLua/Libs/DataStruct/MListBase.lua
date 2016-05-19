--[[
    @brief 数组实现，类实现，数组的下标从 0 开始，但是 lua 中数组的下标从 1 开始
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Functor.CmpFuncObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MListBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_funcObj = GlobalNS.new(GlobalNS.CmpFuncObject);
end

function M:dtor()

end

function M:setFuncObject(pThis, func)
    if(self.m_funcObj == nil) then
        self.m_funcObj = GlobalNS.new(GlobalNS.CmpFuncObject);
    else
        self.m_funcObj:clear();
    end
	self.m_funcObj:setPThisAndHandle(pThis, func);
end

function M:clearFuncObject()
	self.m_funcObj = nil;
end

-- 如果 a < b 返回 -1，如果 a == b ，返回 0，如果 a > b ，返回 1
function M:cmpFunc(a, b)
	if (self.m_funcObj ~= nil and self.m_funcObj:isValid()) then
		return self.m_funcObj:callTwoParam(a, b);
    elseif(GlobalNS.UtilApi.isTypeEqual(a, b)) then
        if(GlobalNS.UtilApi.isTable(a) or GlobalNS.UtilApi.isFunction(a) or GlobalNS.UtilApi.isBoolean(a)) then
            -- 这个一定要放在第一行，因为如果是 table 比较，是可以进行 == 操作的，其实比较的是地址，但是不能进行 < 或者 > 比较操作，如果是表只进行 == 比较操作
            -- function 也只能进行 == 比较操作，不能进行 < 或者 > 比较操作
            if (a == b) then
                return 0;
            else
                return -1;
            end
        elseif(GlobalNS.UtilApi.isNumber(a) or GlobalNS.UtilApi.isString(a)) then
            if (a == b) then
                return 0;
            elseif (a < b) then
                return -1;
            else
                return 1;
            end
        else
            return -1
        end
	else
	    return -1;
	end
end

return M;