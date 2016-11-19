--[[
    @brief 字典实现
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MDictionary";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_data = {};
end

function M:dtor()

end

function M:getData()
    return self.m_data;
end

function M:getCount()
    local ret = 0;
    if (self.m_data ~= nil) then
        for _, value in pairs(self.m_data) do
            ret = ret + 1;
        end
    end
    
    return ret;
end

function M:value(key)
    --[[
    for key_, value_ in pairs(self.m_data) do
        if key_ == key then
            return value_;
        end
    end
    
    return nil;
    ]]
    
    return self.m_data[key];
end

function M:key(value)
    for key_, value_ in pairs(self.m_data) do
        if value_ == value then
            return key_;
        end
    end
    
    return nil;
end

function M:Add(key, value)
    self.m_data[key] = value;
end

function M:Remove(key)
    -- table.remove 只能移除数组
    -- table.remove(self.m_data, key);
    self.m_data[key] = nil;
end

function M:Clear()
    self.m_data = {};
end

function M:ContainsKey(key)
    --[[
    for key_, value_ in pairs(self.m_data) do
        if key_ == key then
            return true;
        end
    end
    
    return false;
    ]]
    
    return self.m_data[key] ~= nil;
end

function M:ContainsValue(value)
    for _, value_ in pairs(self.m_data) do
        if value_ == value then
            return true;
        end
    end
    
    return false;
end

return M;