--[[
    @brief 数组实现，类实现，数组的下标从 0 开始，但是 lua 中数组的下标从 1 开始
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject:new())
M.clsName = "MList"
GlobalNS[M.clsName] = M

function M:ctor()
    self.m_data = {}
end

function M:dtor()

end

function M:getLen()
	local ret = 0
    if self.m_data ~= nil then
        ret = table.getn(self.m_data)
    end
    
    return ret
end

function M:Count()
    return self:getLen()
end

function M:list()
    return self.m_data
end

function M:Add(value)
    self:add(value)
end

-- 表添加是从索引 1 开始的， ipairs 遍历也是从下表 1 开始的，因此，如果是 0 可能有问题，第 0 个元素不能遍历
function M:add(value)
	table.insert(self.m_data, value)
    -- self.m_data[self:getLen() + 1] = value
end

function M:Remove(value)
    return self:remove(value)
end 

function M:remove(value)
    local idx = 1
    while( idx < self:getLen() + 1 )
    do
        if self:cmpFunc(self.m_data[idx], value) == 0 then
            table.remove(self.m_data, idx)
            break;
        end
        idx = idx + 1
    end
    
    if idx < self:getLen() + 1 then
        return true
    else
        return false
    end
end

function removeAt(index)
	if index < self.Count() then
		table.remove(self.m_data, index + 1)  	-- 需要添加 1 ，作为删除的索引
		return true
	end
	
	return false
end

function M:at(index)
    if index < self:getLen() then
        return self.m_data[index + 1]
    end
    
    return nil
end

function M:IndexOf(value)
    local idx = 1
    while( idx < self:getLen() + 1 )
    do
        if self:cmpFunc(self.m_data[idx], value) == 0 then
            break;
        end
        idx = idx + 1
    end
    
    if idx < self:getLen() + 1 then
        return idx - 1      -- 返回的是从 0 开始的下表
    else
        return -1
    end
end

function M:Clear()
    self.m_data = {}
end

function M:setFuncObject(caller, func)
	self.m_funcObj = GlobalNS.CmpFuncObject:new()
	self.m_funcObj:setPThisAndHandle(caller, func)
end

function M:clearFuncObject()
	self.m_funcObj = nil
end

-- 如果 a < b 返回 -1，如果 a == b ，返回 0，如果 a > b ，返回 1
function M:cmpFunc(a, b)
	if self.m_funcObj ~= nil then
		return self.m_funcObj:callTwoParam(a, b)
	else
		if a < b then
			return -1
		elseif a == b then
			return 0
		else
			return 1
		end
	end
return M