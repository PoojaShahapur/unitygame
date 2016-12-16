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
    self.mData = {};
end

function M:dtor()

end

function M:getData()
    return self.mData;
end

function M:getCount()
    local ret = 0;
    if (self.mData ~= nil) then
        for _, value in pairs(self.mData) do
            ret = ret + 1;
        end
    end
    
    return ret;
end

function M:value(key)
    --[[
    for key_, value_ in pairs(self.mData) do
        if key_ == key then
            return value_;
        end
    end
    
    return nil;
    ]]
    
    return self.mData[key];
end

function M:key(value)
    for key_, value_ in pairs(self.mData) do
        if value_ == value then
            return key_;
        end
    end
    
    return nil;
end

function M:Add(key, value)
    self.mData[key] = value;
end

function M:Remove(key)
    -- table.remove 只能移除数组
    -- table.remove(self.mData, key);
    self.mData[key] = nil;
end

function M:Clear()
    self.mData = {};
end

function M:ContainsKey(key)
    --[[
    for key_, value_ in pairs(self.mData) do
        if key_ == key then
            return true;
        end
    end
    
    return false;
    ]]
    
    return self.mData[key] ~= nil;
end

function M:ContainsValue(value)
    for _, value_ in pairs(self.mData) do
        if value_ == value then
            return true;
        end
    end
    
    return false;
end

return M;