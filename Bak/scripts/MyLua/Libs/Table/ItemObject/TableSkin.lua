MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableSkin";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mIsParse = false;
	self.mSkinId2BaseIdListMap = GlobalNS.new(GlobalNS.MDictionary);
	self.mBaseTable = nil;
end

function M:dtor()
	
end

function M:init()
	self.mBaseTable = LuaExcelManager.skin_skin;
end

function M:dispose()
	
end

function M:parseTabel()
	local baseIdList = nil;
	
	for key, value in pairs(self.mBaseTable) do
		if(not self.mSkinId2BaseIdListMap:ContainsKey(value.skinid)) then
			baseIdList = GlobalNS.new(GlobalNS.MList);
			baseIdList:setIsSpeedUpFind(true);
			baseIdList:setIsOpKeepSort(true);
			self.mSkinId2BaseIdListMap:add(value.skinid, baseIdList);
		else
			baseIdList = self.mSkinId2BaseIdListMap:value(value.skinid);
		end
		
		if(value.skinid == 4) then
			local aaa = 10;
		end
		baseIdList:add(value);
	end
end

function M:getBaseIdListBySkinId(skinId)
	if(not self.mIsParse) then
		self.mIsParse = true;
		self:parseTabel();
	end
	
	local baseIdList = nil;
	
	if(self.mSkinId2BaseIdListMap:ContainsKey(skinId)) then
		baseIdList = self.mSkinId2BaseIdListMap:value(skinId);
	end
	
	return baseIdList;
end

return M;