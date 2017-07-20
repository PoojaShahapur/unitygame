--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

--[[使用示例
    local FlyItem = GlobalNS.new(GlobalNS.FlyItem);
    FlyItem:init();
    local destPosItemTran = GlobalNS.UtilApi.getComByPath(bg_image, "Account_BtnTouch", "RectTransform").transform;
    FlyItem:setGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "FlyUFO"), destPosItemTran);
    FlyItem:doFlyItem();
]]--

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "FlyItem";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.FlyItem = nil;

    self.mTimer = GlobalNS.new(GlobalNS.TimerItemBase);
    self.mTimer.mInternal = 0.02;
    self.mTimer.mIsContinuous = true;
    self.mTimer.mIsInfineLoop = true;
    self.mTimer:setFuncObject(self, self.onFlyTick);
    self.mIsOnFlying = false;
    self.mOldTime = 0;
    self.mFlyGo = nil;
    self.mFlySpeed = 1200;
    self.mFly_x = 0;
    self.mFly_y = 0;
end

function M:dtor()
	self:dispose();
end

function M:dispose()
	if(nil ~= self.FlyItem) then
		GlobalNS.delete(self.FlyItem);
		self.FlyItem = nil;
	end

    if(nil ~= self.mTimer) then
        self.mTimer:Stop();
		GlobalNS.delete(self.mTimer);
		self.mTimer = nil;
	end
end

function M:init()
    self.mFlyItem = GlobalNS.new(GlobalNS.AuxComponent);
	self.mFlyItem:setIsDestroySelf(false);
end

--item: 要移动的物体
--destPosItemTran: 目标位置的Transform
function M:setGo(item, destPosItemTran)
    self.mFlyGo = item;
    self.mFlyItem:setSelfGo(item);
    self.mFlyItemPos = GlobalNS.UtilApi.GetComponent(item, "RectTransform").transform.localPosition;
    self.mFly_x = self.mFlyItemPos.x;
    self.mFly_y = self.mFlyItemPos.y;

    --将终点位置从他的局部坐标转到item父节点坐标下，保证两个位置在统一的坐标系
    local parentTran = item.transform.parent.transform;
    local destPosTrans = GlobalNS.UtilApi.convPtFromLocal2Local(destPosItemTran, parentTran); 
    self.mFly_Dest_x = destPosTrans.x;
    self.mFly_Dest_y = destPosTrans.y;

    self.curPos = Vector2.New(self.mFly_x, self.mFly_y);--当前位置
    self.nextPos = Vector2.New(self.mFly_Dest_x, self.mFly_Dest_y);--目标位置
    self.normal = self.nextPos - self.curPos;
    self.normal = self.normal:Normalize();
end

function M:doFlyItem()
    if not self.mIsOnFlying then
        self.mIsOnFlying = true;
        self.mTimer:reset();
        self.mTimer:Start();
        self.mOldTime = os.clock();
    end
end

function M:onFlyTick()
    if not self.mIsOnFlying then
        return;
    end
    --界面已关闭，对象已被删除掉，直接关掉定时器
    if GlobalNS.UtilApi.IsUObjNil(self.mFlyGo) then
        self.mTimer:Stop();
        self.mIsOnFlying = false;
        return;
    end

    local curTime = os.clock();
    local delta = curTime - self.mOldTime;
    self.mOldTime = curTime;
    local deltaDis = self.mFlySpeed * delta;  --这次计时与上次计时间隔里实际应该走的距离

    --当前点与下一点的距离平方
    local wantDis_pow = (self.curPos.x - self.nextPos.x) * (self.curPos.x - self.nextPos.x) + (self.curPos.y - self.nextPos.y) * (self.curPos.y - self.nextPos.y);
    --实际可以行走的距离平方
    local realDis_pow = deltaDis * deltaDis;
        
    if wantDis_pow > realDis_pow then  --无法走完理想距离，那么目标点为当前点加上实际可以走过的距离，下次计时接着走完
        self.curPos = self.curPos + (self.normal * deltaDis);
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localPosition = self.curPos;
    else --可以走完，直接设置到目标
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localPosition = self.nextPos;
        self.mTimer:Stop();
        self.mIsOnFlying = false;
    end
end

return M;

--endregion
