MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief 通过 Key 索引的列表
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MKeyIndexList";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mList = GlobalNS.new(GlobalNS.MList);		-- 数据列表
	self.mList:setIsSpeedUpFind(true);
	self.mList:setIsOpKeepSort(true);

	self.mIndexDic = GlobalNS.new(GlobalNS.MDictionary);	-- Key 字典
end

function M:setIsSpeedUpFind(value)
	self.mList:setIsSpeedUpFind(value);
end

function M:setIsOpKeepSort(value)
	self.mList:setIsOpKeepSort(value);
end

function M:Clear()
	self:clear()
end

function M:clear()
	self.mList:clear();
	self.mIndexDic:clear();
end

function M:getList()
	return self.mList;
end

function M:get(index)
	return self.mList:get(index);
end

function M:getIndexDic()
	return self.mIndexDic;
end

function M:add(key, value)
	if(not self.mIndexDic:ContainsKey(key)) then
		self.mList:add(value);
		self.mIndexDic:add(key, value);
	end
end

function M:Remove(key)
	self:remove(key);
end

function M:remove(key)
	if(self.mIndexDic:ContainsKey(key)) then
		self.mList:remove(self.mIndexDic:value(key));
		self.mIndexDic:remove(key);
	end
end

function M:getAndRemoveByKey(key)
	local item = self:value(key);
	self:remove(key);
	return item;
end

function M:value(key)
	local ret = nil;

	if(self.mIndexDic:ContainsKey(key)) then
		ret = self.mIndexDic:value(key);
	end

	return ret;
end

function M:Count()
	return self:count();
end

function M:count()
	return self.mList:Count();
end

function M:ContainsKey(key)
	return self.mIndexDic:ContainsKey(key);
end

return M;