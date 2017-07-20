--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "FlyUFO";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.FlyUFOBtn = nil;

    self.mTimer = GlobalNS.new(GlobalNS.TimerItemBase);
    self.mTimer.mInternal = 0.02;
    self.mTimer.mIsContinuous = true;
    self.mTimer.mIsInfineLoop = true;
    self.mTimer:setFuncObject(self, self.onFlyTick);
    self.mIsOnFlying = false;
    self.mOldTime = 0;
    self.mFlyGo = nil;
    self.mFlySpeed = 1800;
    self.mFlyUFO_x = 0;
    self.mFlyUFO_y = 0;
    self.mFlyUFO_Init_x = 0;
    self.mFlyUFO_Init_y = 0;
    self.mPosIndex = 1;--从第二个点开始
    self.mPosList = nil;
    self.mPosCount = 0;

    self.mRandomIndex = 1;
end

function M:dtor()
	self:dispose();
end

function M:dispose()
	if(nil ~= self.FlyUFOBtn) then
		GlobalNS.delete(self.FlyUFOBtn);
		self.FlyUFOBtn = nil;
	end

    if(nil ~= self.mTimer) then
        self.mTimer:Stop();
		GlobalNS.delete(self.mTimer);
		self.mTimer = nil;
	end
end

function M:init()
    self.mFlyUFOBtn = GlobalNS.new(GlobalNS.AuxButton);
    --self.mFlyUFOBtn:hide();--暂时先不用
	self.mFlyUFOBtn:setIsDestroySelf(false);
	self.mFlyUFOBtn:addEventHandle(self, self.onFlyUFOBtnClk);
end

function M:setMovePos()
    local _time = os.clock();
    math.randomseed(_time);
    self.mRandomIndex = math.random(1, 5);
    if 1 == self.mRandomIndex then
        self.mPosList = GCtx.mFlyPos.pos_3rose_1;
        self.mPosCount = GCtx.mFlyPos.pos_3rose_1:Count();
    elseif 2 == self.mRandomIndex then
        self.mPosList = GCtx.mFlyPos.pos_3rose_2;
        self.mPosCount = GCtx.mFlyPos.pos_3rose_2:Count();
    elseif 3 == self.mRandomIndex then
        self.mPosList = GCtx.mFlyPos.pos_4rose_1;
        self.mPosCount = GCtx.mFlyPos.pos_4rose_1:Count();
    elseif 4 == self.mRandomIndex then
        self.mPosList = GCtx.mFlyPos.pos_4rose_2;
        self.mPosCount = GCtx.mFlyPos.pos_4rose_2:Count();
    else
        self.mPosList = GCtx.mFlyPos.pos_arch;
        self.mPosCount = GCtx.mFlyPos.pos_arch:Count();
    end
end

function M:setGo(ufo)
    self.mFlyGo = ufo;
    self.mFlyUFOBtn:setSelfGo(ufo);
    self.mFlyUFOPos = GlobalNS.UtilApi.GetComponent(ufo, "RectTransform").transform.localPosition;
    self.mFlyUFORot = GlobalNS.UtilApi.GetComponent(ufo, "RectTransform").transform.localRotation;
    self.mFlyUFO_x = self.mFlyUFOPos.x;
    self.mFlyUFO_y = self.mFlyUFOPos.y;
    self.mFlyUFO_Init_x = self.mFlyUFO_x;
    self.mFlyUFO_Init_y = self.mFlyUFO_y;
end

function M:onFlyUFOBtnClk(dispObj)
    if not self.mIsOnFlying then
        self.mPosIndex = 1;
        self:setMovePos();
        self.mTimer:reset();
        self.mTimer:Start();
        self.mIsOnFlying = true;
        self.mOldTime = os.clock();
    end
end

function M:onFlyTick()
    if self.mPosIndex >= self.mPosCount then--已经走完所有点，设回原始位置
        self.mFlyUFO_x = self.mFlyUFOPos.x;
        self.mFlyUFO_y = self.mFlyUFOPos.y;
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localRotation = self.mFlyUFORot;
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localPosition = self.mFlyUFOPos;
        self.mTimer:Stop();
        self.mIsOnFlying = false;
        return;
    end

    local curTime = os.clock();
    local delta = curTime - self.mOldTime;--当前计时与上次计时的时间间隔
    self.mOldTime = curTime;
    local deltaDis = self.mFlySpeed * delta;  --计时间隔里实际应该走的距离
    local flyPos = self.mPosList:get(self.mPosIndex);
    local curPos = Vector2.New(self.mFlyUFO_x, self.mFlyUFO_y);--当前位置
    local nextPos = Vector2.New(self.mFlyUFO_Init_x + flyPos.x, self.mFlyUFO_Init_y + flyPos.y);--轨迹中的下一个位置
    local normal = nextPos - curPos;
    normal = normal:Normalize();
    local rotation = Quaternion.FromToRotation(Vector3.up, Vector3.New(normal.x, normal.y, 0));--朝向

    --当前点与下一点的距离平方
    local wantDis_pow = (curPos.x - nextPos.x) * (curPos.x - nextPos.x) + (curPos.y - nextPos.y) * (curPos.y - nextPos.y);
    --实际可以行走的距离平方
    local realDis_pow = deltaDis * deltaDis;
        
    if wantDis_pow > realDis_pow then  --无法走完理想距离，那么目标点为当前点加上实际可以走过的距离，下次计时接着走完
        local delta_x = (normal * deltaDis).x;
        local delta_y = (normal * deltaDis).y;
        self.mFlyUFO_x = self.mFlyUFO_x + delta_x;
        self.mFlyUFO_y = self.mFlyUFO_y + delta_y;
        --Debugger.Log(" " .. deltaDis .. " x " ..  delta_x .. " y " ..  delta_y .. " x " ..  normal.x .. " y " ..  normal.y);
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localPosition = Vector2.New(self.mFlyUFO_x, self.mFlyUFO_y);
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localRotation = rotation;
        --Debugger.Log(" " .. self.mPosIndex .. " x " ..  self.mFlyUFO_x .. " y " ..  self.mFlyUFO_y);
        --Debugger.Log(" true " .. " x " ..  curPos.x .. " y " ..  curPos.y .. " x " ..  nextPos.x .. " y " ..  nextPos.y);
    else --可以走完，直接设置到目标点，index加1
        self.mFlyUFO_x = nextPos.x;
        self.mFlyUFO_y = nextPos.y;
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localPosition = Vector2.New(self.mFlyUFO_x, self.mFlyUFO_y);
        GlobalNS.UtilApi.GetComponent(self.mFlyGo, "RectTransform").transform.localRotation = rotation;
        --Debugger.Log(" " .. self.mPosIndex .. " x " ..  self.mFlyUFO_x .. " y " ..  self.mFlyUFO_y);
        self.mPosIndex = self.mPosIndex + 1;
        --Debugger.Log(" false " .. " x " ..  curPos.x .. " y " ..  curPos.y .. " x " ..  nextPos.x .. " y " ..  nextPos.y);
    end

    --Debugger.Log(" " .. self.mPosIndex .. " x " ..  self.mPosCount);
end

return M;

--endregion
