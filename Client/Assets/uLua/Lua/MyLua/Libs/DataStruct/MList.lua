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
    if self.m_data ~= nil then
        return table.getn(self.m_data)
    end
    
    return 0
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

function M:add(value)
    self.m_data[self:getLen() + 1] = value
end

function M:Remove(value)
    return self:remove(value)
end 

function M:remove(value)
    local idx = 1
    while( idx < self:getLen() + 1 )
    do
        if self.m_data[idx] == value then
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

function M:at(index)
    if index < self:getLen() then
        return self.m_data[index]
    end
    
    return nil
end

function M:IndexOf(value)
    local idx = 1
    while( idx < self:getLen() + 1 )
    do
        if self.m_data[idx] == value then
            table.remove(self.m_data, idx)
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

return M