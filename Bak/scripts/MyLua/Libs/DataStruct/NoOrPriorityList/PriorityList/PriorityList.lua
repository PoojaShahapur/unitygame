MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DataStruct.MDictionary");
MLoader("MyLua.Libs.DataStruct.NoOrPriorityList.INoOrPriorityList");
MLoader("MyLua.Libs.DataStruct.NoOrPriorityList.PriorityList.PrioritySort");

-- 优先级队列
local M = GlobalNS.Class(GlobalNS.INoOrPriorityList);
M.clsName = "PriorityList";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mPriorityProcessObjectList = GlobalNS.new(GlobalNS.MList);
	self.mPrioritySort = GlobalNS.PrioritySort.ePS_Great;
	self.mIsSpeedUpFind = false;
	self.mIsOpKeepSort = false;
end

function M:setIsSpeedUpFind(value)
	self.mIsSpeedUpFind = value;

	if (self.mIsSpeedUpFind) then
		self.mDic = GlobalNS.new(GlobalNS.MDictionary);
	end
end

function M:setIsOpKeepSort(value)
	self.mIsOpKeepSort = value;
end

function M:Clear()
	self.mPriorityProcessObjectList:Clear();

	if(self.mIsSpeedUpFind) then
		self.mDic:Clear();
	end
end

function M:Count()
	return self.mPriorityProcessObjectList:Count();
end

function M:get(index)
	local ret = nil;

	if(index < self:Count()) then
		ret = self.mPriorityProcessObjectList:get(index).mPriorityObject;
	end

	return ret;
end

function M:getPriority(index)
	local ret = 0;

	if (index < self:Count()) then
		ret = self.mPriorityProcessObjectList:get(index).mPriority;
	end

	return ret;
end

function M:Contains(item)
	local ret = false;

	if (nil ~= item) then
		if (self.mIsSpeedUpFind) then
			ret = self.mDic:ContainsKey(item);
		else
			local index = 0;
			local listLen = self.mPriorityProcessObjectList:Count();

			while (index < listLen) do
				if (item == self.mPriorityProcessObjectList:get(index).mPriorityObject) then
					ret = true;
					break;
				end

				index = index + 1;
			end
		end
	else
		if (MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("PriorityList::Contains, failed", GlobalNS.LogTypeId.eLogPriorityListCheck);
		end
	end

	return ret;
end

function M:RemoveAt(index)
	if (self.mIsSpeedUpFind) then
		self:effectiveRemove(self.mPriorityProcessObjectList:get(index).mPriorityObject);
	else
		self.mPriorityProcessObjectList:RemoveAt(index);
	end
end

function M:getIndexByPriority(priority)
	local retIndex = -1;

	local index = 0;
	local listLen = self.mPriorityProcessObjectList:Count();

	while (index < listLen) do
		if (GlobalNS.PrioritySort.ePS_Less == self.mPrioritySort) then
			if (self.mPriorityProcessObjectList:get(index).mPriority >= priority) then
				retIndex = index;
				break;
			end
		elseif (GlobalNS.PrioritySort.ePS_Great == self.mPrioritySort) then
			if (self.mPriorityProcessObjectList:get(index).mPriority <= priority) then
				retIndex = index;
				break;
			end
		end

		index = index + 1;
	end

	return retIndex;
end

function M:getIndexByPriorityObject(priorityObject)
	local retIndex = -1;

	local index = 0;
	local listLen = self.mPriorityProcessObjectList:Count();

	while (index < listLen) do
		if (self.mPriorityProcessObjectList:get(index).mPriorityObject == priorityObject) then
			retIndex = index;
			break;
		end

		index = index + 1;
	end

	return retIndex;
end

function M:getIndexByNoOrPriorityObject(priorityObject)
	return self:getIndexByPriorityObject(priorityObject);
end

function M:addPriorityObject(priorityObject, priority)
	if (nil ~= priorityObject) then
		if (not self:Contains(priorityObject)) then
			local priorityProcessObject = nil;
			priorityProcessObject = GlobalNS.new(GlobalNS.PriorityProcessObject);

			priorityProcessObject.mPriorityObject = priorityObject;
			priorityProcessObject.mPriority = priority;

			if (not self.mIsOpKeepSort) then
				self.mPriorityProcessObjectList:Add(priorityProcessObject);

				if (self.mIsSpeedUpFind) then
					self.mDic:Add(priorityObject, self.mPriorityProcessObjectList:Count() - 1);
				end
			else
				local index = self:getIndexByPriority(priority);

				if (-1 == index) then
					self.mPriorityProcessObjectList:Add(priorityProcessObject);

					if (self.mIsSpeedUpFind) then
						self.mDic:Add(priorityObject, self.mPriorityProcessObjectList:Count() - 1);
					end
				else
					self.mPriorityProcessObjectList:Insert(index, priorityProcessObject);

					if (self.mIsSpeedUpFind) then
						self.mDic:Add(priorityObject, index);
						self:updateIndex(index + 1);
					end
				end
			end
		end
	else
		if (MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("PriorityList::addPriorityObject, failed", GlobalNS.LogTypeId.eLogPriorityListCheck);
		end
	end
end

function M:removePriorityObject(priorityObject)
	if (self:Contains(priorityObject)) then
		if (self.mIsSpeedUpFind) then
			self:effectiveRemove(priorityObject);
		else
			local index = self:getIndexByPriorityObject(priorityObject);

			if(-1 ~= index) then
				self.mPriorityProcessObjectList:RemoveAt(index);
			end
		end
	end
end

function M:addNoOrPriorityObject(noPriorityObject, priority)
	self:addPriorityObject(noPriorityObject);
end

function M:removeNoOrPriorityObject(noPriorityObject)
	self:removePriorityObject(noPriorityObject);
end

-- 快速移除元素
function M:effectiveRemove(item)
	local ret = false;

	if (self.mDic:ContainsKey(item)) then
		ret = true;

		local index = self.mDic:value(item);
		self.mDic:Remove(item);

		if (index == self.mPriorityProcessObjectList:Count() - 1) then    -- 如果是最后一个元素，直接移除
			self.mPriorityProcessObjectList:RemoveAt(index);
		else
			-- 这样移除会使优先级顺序改变
			if (not self.mIsOpKeepSort) then
				self.mPriorityProcessObjectList:set(index, self.mPriorityProcessObjectList:get(self.mPriorityProcessObjectList:Count() - 1));
				self.mPriorityProcessObjectList:RemoveAt(self.mPriorityProcessObjectList:Count() - 1);
				self.mDic:Add(self.mPriorityProcessObjectList:get(index).mPriorityObject, index);
			else
				self.mPriorityProcessObjectList:RemoveAt(index);
				self:updateIndex(index);
			end
		end
	end

	return ret;
end

function M:updateIndex(index)
	local listLen = self.mPriorityProcessObjectList:Count();

	while (index < listLen) do
		self.mDic:Add(self.mPriorityProcessObjectList:get(index).mPriorityObject, index);

		index = index + 1;
	end
end

return M;