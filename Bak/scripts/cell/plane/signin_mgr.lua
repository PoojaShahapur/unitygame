MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local pb = require("protobuf");
local log = require('log'):new('plane.signin_mgr');
local mdb = require("database");

--[[
@brief 签到系统
]]
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "signin_mgr";
GlobalNS[M.clsName] = M;

M.DB_KEY = "signinStr";

function M:ctor()
	self.mDailySigninList = GlobalNS.new(GlobalNS.MList); 	-- 每日签到列表
	self.mDailySigninList:setIsSpeedUpFind(true);
	self.mDailySigninList:setIsOpKeepSort(true);
	self.mReceiveCumulaRewardList = GlobalNS.new(GlobalNS.MList); 		-- 领取的每日奖励列表
	self.mReceiveCumulaRewardList:setIsSpeedUpFind(true);
	self.mReceiveCumulaRewardList:setIsOpKeepSort(true);
	
	-- TODO: Test
	--self.mDailySigninList:add(15);
	--self.mDailySigninList:add(16);
	--self.mDailySigninList:add(17);
	
	self.mPlayer = nil; 	-- 保存的玩家指针
end

function M:dtor()
	self:dispose();
end

function M:init()
	
end

function M:dispose()
	self.mPlayer = nil;
	
	if(nil ~= self.mDailySigninList) then
		self.mDailySigninList:clear();
		self.mDailySigninList = nil;
	end
	
	if(nil ~= self.mReceiveCumulaRewardList) then
		self.mReceiveCumulaRewardList:clear();
		self.mReceiveCumulaRewardList = nil;
	end
end

-- 序列化
function M:serialize()
	log:info('signin_mgr::serialize');
	
	local ret = nil;
	
	local pbTable = {
        dayList = {},
		idList = {}
    };
	
	pbTable.dayList = self.mDailySigninList:list();
	pbTable.idList = self.mReceiveCumulaRewardList:list();
	
	ret = assert(pb.encode("svr.MonthSigninInfo", pbTable));
	
	return ret;
end

-- 反序列化
function M:unserialize(pbBytes)
	log:info('signin_mgr::unserialize');

	local pbTable = assert(pb.decode("svr.MonthSigninInfo", pbBytes));
	
	local index = 0;
	local listLen = 0;
	local element = 0;
	
	index = 0;
	listLen = GlobalNS.UtilApi.getTableLen(pbTable.dayList);
	
	while(index < listLen) do
		element = GlobalNS.UtilApi.getTableElementByIndex(pbTable.dayList, index);
		self.mDailySigninList:add(element);
		index = index + 1;
	end
	
	index = 0;
	listLen = GlobalNS.UtilApi.getTableLen(pbTable.idList);
	
	while(index < listLen) do
		element = GlobalNS.UtilApi.getTableElementByIndex(pbTable.idList, index);
		self.mReceiveCumulaRewardList:add(element);
		index = index + 1;
	end
end

-- 保存
function M:save()
	log:info('signin_mgr::save');
	
    local saveBytes = self:serialize();

    if(0 ~= string.len(saveBytes)) then
        mdb:update_b(mdb.db, mdb.collection, {_id=self.mPlayer.openid}, nil, {signinStr=saveBytes});
    end
end

-- 月初或者月底之前停止服务器，然后月初重启服务器
function M:onNewMonthStart()
	self.mDailySigninList:clear();
	self.mReceiveCumulaRewardList:clear();
end

function M:setPlayer(player)
	self.mPlayer = player;
end

function M:online()
	-- 通知客户端签到信息
	self:notifyMonthSigninInfo();
end

-- 请求每日签到
function M:reqDaySignin()
	local day = GlobalNS.UtilApi.getDay();

	local sendMsg = {
        ret = 1,
    };
	
	if(self.mDailySigninList:Contains(day)) then
		sendMsg.ret = 2;
	else
		self.mDailySigninList:add(day);
	end
	
	self.mPlayer:rpc_request("plane.SignInS2C", "NotifyDaySigninResult", pb.encode("plane.DaySigninResultMsg", sendMsg));
end

-- 请求领取累积奖励
function M:reqReceiveCumulaReward(retMsg)
	log:debug('reqReceiveCumulaReward, retMsg.id = %d', retMsg.id);
	
	local sendMsg = {
		id = retMsg.id,
        ret = 1
    };
	
	if(self.mReceiveCumulaRewardList:Contains(retMsg.id)) then
		sendMsg.ret = 2;
	else
		self.mReceiveCumulaRewardList:add(retMsg.id);
	end

	self.mPlayer:rpc_request("plane.SignInS2C", "NotifyReceiveCumulaRewardResult", pb.encode("plane.ReceiveCumulaRewardResultMsg", sendMsg));
end

-- 上线通知每日签到信息
function M:notifyMonthSigninInfo()
	local sendMsg = {
		signincount = self.mDailySigninList:count(),
		signinlist = {},
        receivecount = self.mReceiveCumulaRewardList:count(),
		cumularewardlist = {}
    };

	local index = 0;
	local listLen = 0;
	
	if(sendMsg.signincount > 0) then
		index = 0;
		listLen = sendMsg.signincount;
		
		while(index < listLen) do
			GlobalNS.UtilApi.insertTable(sendMsg.signinlist, self.mDailySigninList:get(index));
			index = index + 1;
		end
	end
	
	if(sendMsg.receivecount > 0) then
		index = 0;
		listLen = sendMsg.receivecount;
		
		while(index < listLen) do
			GlobalNS.UtilApi.insertTable(sendMsg.cumularewardlist, self.mReceiveCumulaRewardList:get(index));
			index = index + 1;
		end
	end

	self.mPlayer:rpc_request("plane.SignInS2C", "NotifyMonthSigninInfo", pb.encode("plane.MonthSigninInfoMsg", sendMsg));
end

return M;