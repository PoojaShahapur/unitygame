--[[
    @brief 数组实现，类实现，数组的下标从 0 开始，但是 lua 中数组的下标从 1 开始
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Common.CmpFuncObject"
require "MyLua.Libs.DataStruct.MListBase"

local M = GlobalNS.Class(GlobalNS.MListBase);
M.clsName = "MList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_data = {};
end

function M:dtor()

end

function M:getLen()
	local ret = 0;
    if self.m_data ~= nil then
        ret = table.getn(self.m_data);
    end
    
    return ret;
end

function M:Count()
    return self:getLen();
end

function M:list()
    return self.m_data;
end

function M:Add(value)
    self:add(value);
end

-- 表添加是从索引 1 开始的， ipairs 遍历也是从下表 1 开始的，因此，如果是 0 可能有问题，第 0 个元素不能遍历
function M:add(value)
	table.insert(self.m_data, value);
    -- self.m_data[self:getLen() + 1] = value;
end

function M:Remove(value)
    return self:remove(value);
end 

function M:remove(value)
    local idx = 1;
    local bFind = false
    while( idx < self:getLen() + 1 ) do
        if self:cmpFunc(self.m_data[idx], value) == 0 then
            table.remove(self.m_data, idx);
            bFind = true
            break;
        end
        idx = idx + 1;
    end
    
    return bFind
end

function M:removeAt(index)
	if index < self.Count() then
		table.remove(self.m_data, index + 1);  	-- 需要添加 1 ，作为删除的索引
		return true;
	end
	
	return false;
end

function M:removeAtAndRet(index)
    local ret;
    if index < self.Count() then
        ret = table.remove(self.m_data, index + 1);    -- 需要添加 1 ，作为删除的索引
    end
    
    return ret;
end

function M:at(index)
    if index < self:getLen() then
        return self.m_data[index + 1];
    end
    
    return nil;
end

function M:IndexOf(value)
    local idx = 1;
    local bFind = false
    while( idx < self:getLen() + 1 ) do
        if self:cmpFunc(self.m_data[idx], value) == 0 then
            bFind = true
            break;
        end
        idx = idx + 1;
    end
    
    if bFind then
        return idx - 1;      -- 返回的是从 0 开始的下表
    else
        return -1;
    end
end

function M:find(value, pThis, func)
    self:setFuncObject(pThis, func);
    local index = 1;
    local bFind = false;
    while(index < self:getLen() + 1) do
        if self:cmpFunc(self.m_data[index], value) == 0 then
            bFind = true;
            break;
        end
        index = index + 1;
    end
    
    if bFind then
        return self.m_data[index];
    else
        return nil;
    end
end

-- 通过一个类型添加一个变量
function M:addByCls(cls)
    local item = GlobalNS.new(cls);
    self:add(item);
end

-- 获取并且创建一个 Item 
function M:getOrCreate(value, cls, pThis, func)
    local item = self:find(value, pThis, func);
    if item == nil then
        item = self:addByCls(cls);
    end
    
    return item;
end

function M:Clear()
    self.m_data = {};
end

-- 排序
function M:sort(pThis, func)
    self:setFuncObject(pThis, func);
    -- 目前采用插入排序
    local len = self:getLen();
    local temp;
    local jIndex = 0;
    for index = 2, len, 1 do
        if self:cmpFunc(self.m_data[index - 1], self.m_data[index]) == 1 then
            temp = self.m_data[index];
            jIndex = index;
            while jIndex > 1 and self.cmpFunc(self.m_data[jIndex - 1], temp) == 1 do
                self.m_data[jIndex] = self.m_data[jIndex - 1];
                jIndex = jIndex - 1;
            end
            self.m_data[jIndex] = temp;
        end
    end
end

return M;