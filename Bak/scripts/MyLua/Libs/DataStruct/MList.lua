MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Functor.CmpFuncObject");
MLoader("MyLua.Libs.DataStruct.MListBase");
MLoader("MyLua.Libs.DataStruct.MDictionary");

--[[
    @brief 数组实现，类实现，数组的下标从 0 开始，但是 lua 中数组的下标从 1 开始
]]

-- bug 提示，如果 require "MyLua.Libs.DataStruct.MListBase" 导入后，如果 clsName 不是 MListBase ，就会导致 GlobalNS.MListBase 为空，就会导致基类为空  
local M = GlobalNS.Class(GlobalNS.MListBase);
M.clsName = "MList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mData = {};
	self.mIsSpeedUpFind = false; 	-- 是否加快查询，这个只适用于元素在列表中是唯一的，例如引用之类的，如果有相同的，就会有问题，注意了
	self.mIsOpKeepSort = false;
end

function M:dtor()

end

function M:setIsSpeedUpFind(value)
	self.mIsSpeedUpFind = value;

	if(self.mIsSpeedUpFind) then
		self.mDic = GlobalNS.new(GlobalNS.MDictionary); 	-- 保存的索引是从 0 ，开始的，不是从 1 开始的
	end
end

function M:setIsOpKeepSort(value)
	self.mIsOpKeepSort = value;
end

-- 获取数组中元素的个数
function M:getLen()
	local ret = 0;
	
    if (self.mData ~= nil) then
        ret = table.getn(self.mData);
    end
    
    return ret;
end

-- 获取数组和哈希表中元素的个数
function M:getAllLen()
    local ret = 0;
	
    if (self.mData ~= nil) then
        for _, value in pairs(self.mData) do
            ret = ret + 1;
        end
    end
    
    return ret;
end

function M:length()
	return self:Count();
end

function M:count()
	return self:Count();
end

function M:Count()
    return self:getLen();
end

function M:list()
    return self.mData;
end

function M:Add(value)
    self:add(value);
end

-- 表添加是从索引 1 开始的， ipairs 遍历也是从下表 1 开始的，因此，如果是 0 可能有问题，第 0 个元素不能遍历
function M:add(value)
	if(nil ~= value) then
		table.insert(self.mData, value);
		-- self.mData[self:getLen() + 1] = value;

		if (self.mIsSpeedUpFind) then
			self.mDic:Add(value, self:Count() - 1);
		end
	else
		--table.insert 插入 nil 是不能插入的
		if(MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("MList::add, add nil value", GlobalNS.LogTypeId.eLogCommon);
		end
	end
end

-- 向列表中插入一个值
function M:insert(index, value)
    if(index < self:Count()) then
        table.insert(self.mData, index + 1, value);
		
		if (self.mIsSpeedUpFind) then
			self.mDic:Add(value, index);
			
			self:updateIndex(index + 1);
		end
    else
        self:add(value);
    end
end

function M:replace(index, value)
	if(index < self:Count()) then
        self.mData[index + 1] = value;
		
		if (self.mIsSpeedUpFind) then
			self.mDic:Add(value, index);
		end
    end
end

function M:Remove(value)
    return self:remove(value);
end 

-- 移除列表中第一个相等的值
function M:remove(value)
	if (self.mIsSpeedUpFind) then
		self:effectiveRemove(value);
	else
		local idx = 1;
		local isFind = false;
		
		while(idx <= self:getLen()) do
			if (self:cmpFunc(self.mData[idx], value) == 0) then
				table.remove(self.mData, idx);
				isFind = true;
				break;
			end
			
			idx = idx + 1;
		end
		
		return isFind;
	end
end

-- 移除所有相等的值
function M:removeAllEqual(value)
	if (self.mIsSpeedUpFind) then
		return self:effectiveRemove(value);
	else
		local idx = self:getLen();
		local isFind = false;
		
		while(idx > 0) do
			if (self:cmpFunc(self.mData[idx], value) == 0) then
				table.remove(self.mData, idx);
				isFind = true
			end
			
			idx = idx - 1;
		end
		
		return isFind;
	end
end

function M:RemoveAt(index)
	self:removeAt(index);
end

function M:removeAt(index)
	local ret = false;
	
	if (index < self:Count()) then
		if (self.mIsSpeedUpFind) then
			ret = self:effectiveRemove(self:get(index));
		else
			table.remove(self.mData, index + 1);  	-- 需要添加 1 ，作为删除的索引
			ret =  true;
		end
	end
	
	return ret;
end

function M:getAndRemoveByIndex(index)
    local ret = nil;
	
    ret = self:get(index);
	self:removeAt(index);
    
    return ret;
end

function M:get(index)
	local item = nil;
	
	if(GlobalNS.UtilApi.isNumber(index)) then
		if (index < self:getLen()) then
			item = self.mData[index + 1];
		end
	end

	return item;
end

function M:set(index, value)
	if(GlobalNS.UtilApi.isNumber(index) and nil ~= value) then
		if (index < self:getLen()) then
			self.mData[index + 1] = value;
		end
	end
end

function M:IndexOf(value)
	local ret = -1;
	
	if (self.mIsSpeedUpFind) then
		ret = self.mDic:value(value);
	else
		local idx = 1;
		local isFind = false;
		
		while (idx <= self:getLen()) do
			if (self:cmpFunc(self.mData[idx], value) == 0) then
				isFind = true;
				break;
			end
			
			idx = idx + 1;
		end
		
		if (isFind) then
			ret = idx - 1;      -- 返回的是从 0 开始的下表
		else
			ret = -1;
		end
	end
	
	return ret;
end

-- 仅仅判断是否包含某个元素
function M:Contains(item)
	local ret = false;
	
	if (self.mIsSpeedUpFind) then
		ret = (self.mDic:value(item) ~= nil);
	else
		ret = (self:IndexOf(item) ~= -1);
	end
	
	return ret;
end

function M:find(value, pThis, func)
    -- 如果指定比较函数
    if(nil ~= pThis or nil ~= func) then
        self:setFuncObject(pThis, func);
    end
	
    local index = 1;
    local isFind = false;
	
    while(index <= self:getLen()) do
        if (self:cmpFunc(self.mData[index], value) == 0) then
            isFind = true;
            break;
        end
		
        index = index + 1;
    end
    
    if (isFind) then
        return self.mData[index];
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

function M:clear()
	self:Clear();
end

function M:Clear()
    self.mData = {};
end

-- 排序
function M:sort(pThis, func)
    -- 如果指定比较函数
    if(nil ~= pThis or nil ~= func) then
        self:setFuncObject(pThis, func);
    end
	
    -- 目前采用插入排序
    local len = self:getLen();
    local temp;
    local jIndex = 0;
	
    for index = 2, len, 1 do
        if (self:cmpFunc(self.mData[index - 1], self.mData[index]) == 1) then
            temp = self.mData[index];
            jIndex = index;
			
            while (jIndex > 1 and self.cmpFunc(self.mData[jIndex - 1], temp) == 1) do
                self.mData[jIndex] = self.mData[jIndex - 1];
                jIndex = jIndex - 1;
            end
			
            self.mData[jIndex] = temp;
        end
    end
end

-- 快速移除元素
function M:effectiveRemove(item)
	local ret = false;

	if (self.mDic:ContainsKey(item)) then
		ret = true;

		local idx = self.mDic:value(item);
		self.mDic:Remove(item);

		if (idx == self:Count() - 1) then    -- 如果是最后一个元素，直接移除
			table.remove(self.mData, idx + 1);
		else
			if (not self.mIsOpKeepSort) then
				self.mData[idx + 1] = self:get(self:Count() - 1);
				table.remove(self.mData, self:Count());
				self.mDic:Add(self:get(idx), idx);
			else
				table.remove(self.mData, idx + 1);
				self:updateIndex(idx);
			end
		end
	end

	return ret;
end

function M:updateIndex(idx)
	local listLen = self:Count();

	while(idx < listLen) do
		self.mDic:Add(self:get(idx), idx);

		idx = idx + 1;
	end
end

return M;