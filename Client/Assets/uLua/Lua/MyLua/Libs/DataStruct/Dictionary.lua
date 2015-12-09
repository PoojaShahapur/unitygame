--[[
    @brief 字典实现
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject:new())
M.clsName = "Dictionary"
GlobalNS[M.clsName] = M

function M:ctor()
    self.m_data = {}
end

function M:dtor()

end

function M:getData()
    return self.m_data
end

function M:value(key)
    for key_, value_ in self.m_data do
        if key_ == key then
            return value_
        end
    end
    
    return nil
end

function M:key(value)
    for key_, value_ in self.m_data do
        if value_ == value then
            return key_
        end
    end
    
    return nil
end

function M:Add(key, value)
    self.m_data[key] = value
end

function M:Remove(key)
    table.remove(self.m_data, key)
end

function M:Clear()
    self.m_data = {}
end

function M:ContainsKey(key)
    for key_, value_ in self.m_data do
        if key_ == key then
            return true
        end
    end
    
    return false
end

function M:ContainsValue(value)
    for key_, value_ in self.m_data do
        if value_ == value then
            return true
        end
    end
    
    return false
end

return M