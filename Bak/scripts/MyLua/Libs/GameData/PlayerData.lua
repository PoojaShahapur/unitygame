MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "PlayerData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mShopData = GlobalNS.new(GlobalNS.ShopData);
	self.mHeroData = GlobalNS.new(GlobalNS.HeroData);
	self.mPackData = GlobalNS.new(GlobalNS.PackData);
	self.mSkinData = GlobalNS.new(GlobalNS.SkinData);
	self.mDailyTaskData = GlobalNS.new(GlobalNS.DailyTaskData);
end

function M:dtor()
	
end

function M:init()
	self.mShopData:init();
	self.mHeroData:init();
	self.mPackData:init();
	self.mSkinData:init();
	self.mDailyTaskData:init();
end

function M:dispose()
	if(nil ~= self.mShopData) then
		self.mShopData:dispose();
		self.mShopData = nil;
	end
	if(nil ~= self.mHeroData) then
		self.mHeroData:dispose();
		self.mHeroData = nil;
	end
	if(nil ~= self.mPackData) then
		self.mPackData:dispose();
		self.mPackData = nil;
	end
	if(nil ~= self.mSkinData) then
		self.mSkinData:dispose();
		self.mSkinData = nil;
	end
	if(nil ~= self.mDailyTaskData) then
		self.mDailyTaskData:dispose();
		self.mDailyTaskData = nil;
	end
end

return M;