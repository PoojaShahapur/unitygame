MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayHandleMgrBase");

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase);
M.clsName = "TimerMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mTimerList = GlobalNS.new(GlobalNS.MList);     -- 当前所有的定时器列表
end

function M:getCount()
    return self.mTimerList:Count();
end

function M:addObject(delayObject, priority)
    if(nil == priority) then
        priority = 0;
    end
    
    -- 检查当前是否已经在队列中
    if (self.mTimerList:IndexOf(delayObject) == -1) then
        if (self:isInDepth()) then
            M.super.addObject(self, delayObject, priority);
        else
            self.mTimerList:Add(delayObject);
        end
		
		GCtx.mFrameUpdateStatistics:incNum();
    end
end

function M:removeObject(delayObject)
    -- 检查当前是否在队列中
    if (self.mTimerList:IndexOf(delayObject) ~= -1) then
        delayObject.mIsDisposed = true;
        if (self:isInDepth()) then
            M.super.removeObject(self, delayObject);
        else
            for key, item in ipairs(self.mTimerList:list()) do
                if (item == delayObject) then
                    self.mTimerList:Remove(item);
                    break;
                end
            end
        end
		
		GCtx.mFrameUpdateStatistics:decNum();
    end
end

function M:addTimer(delayObject, priority)
    self:addObject(delayObject, priority);
end

function M:Advance(delta)
    self:incDepth();

    for key, timerItem in ipairs(self.mTimerList:list()) do
        if (not timerItem:getClientDispose()) then
            timerItem:OnTimer(delta);
        end

        if (timerItem.mIsDisposed) then       -- 如果已经结束
            self:removeObject(timerItem);
        end
    end

    self:decDepth();
end

return M;