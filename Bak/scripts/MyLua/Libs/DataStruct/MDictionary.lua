MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 字典实现
]]

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
	local ret = nil;
	
    for key_, value_ in pairs(self.mData) do
        if value_ == value then
            ret = key_;
			break;
        end
    end
    
    return ret;
end

function M:Add(key, value)
	if(nil ~= key) then
		self.mData[key] = value;
	end
end

function M:add(key, value)
	if(nil ~= key) then
		self.mData[key] = value;
	end
end

function M:remove(key)
	self:Remove(key);
end

function M:Remove(key)
    -- table.remove 只能移除数组
    -- table.remove(self.mData, key);
    self.mData[key] = nil;
end

function M:getAndRemoveByKey(key)
	local value = self:value(key);
	self:remove(key);
	return value;
end

function M:clear()
	self:Clear();
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
	local ret = false;
    for _, value_ in pairs(self.mData) do
        if(value_ == value) then
            ret = true;
			break;
        end
    end
    
    return ret;
end

return M;