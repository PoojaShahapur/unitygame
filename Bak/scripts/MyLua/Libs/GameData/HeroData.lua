MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "HeroData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mMoney = 0;		-- 游戏币
	self.mTicket = 0;		-- 点券
	self.mPlastic = 0;		-- 塑料块

    self.mViewAccount = "";     -- 当前查看的玩家account
    self.mViewLevel = 0;     -- 当前查看的玩家段位
    self.mViewUid = 0;     -- 当前查看的玩家uid
    self.mMyselfAccount = "";   -- 自己的account
    self.mMyselfUid = 0;   -- 自己的uid
    self.mMyselfNickName = "";   -- 自己的NickName
    self.mMyselfLevel = 0;   -- 自己的段位
    self.mAvatarIndex = 0;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:updateMoney()
	self.mMoney = GlobalNS.CSSystem.mHeroData:getMoney();
	self.mTicket = GlobalNS.CSSystem.mHeroData:getTicket();
	self.mPlastic = GlobalNS.CSSystem.mHeroData:getPlastic();
end

function M:getMoney()
	return self.mMoney;
end

function M:getTicket()
	return self.mTicket;
end

function M:getPlastic()
	return self.mPlastic
end

function M:isMyself()
    return self.mViewAccount == self.mMyselfAccount;
end

function M:isMyselfbyUid(uid)
    return uid == self.mMyselfUid;
end

return M;