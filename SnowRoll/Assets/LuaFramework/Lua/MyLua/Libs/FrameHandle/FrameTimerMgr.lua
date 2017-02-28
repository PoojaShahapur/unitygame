MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayHandleMgrBase");

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase);
M.clsName = "FrameTimerMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mTimerList = GlobalNS.new(GlobalNS.MList);
    self.mDelList = GlobalNS.new(GlobalNS.MList);
end

function M:addObject(delayObject, priority)
    -- 检查当前是否已经在队列中
    if(not self.mTimerList:Contains(delayObject)) then
        if(self:isInDepth()) then
            M.super.addObject(self, delayObject, priority);
        else
            self.mTimerList:Add(delayObject);
        end
    end
end

function M:removeObject(delayObject)
    -- 检查当前是否在队列中
    if(self.mTimerList:Contains(delayObject)) then
        delayObject.mIsDisposed = true;
        if(self:isInDepth()) then
            M.super.addObject(self, delayObject);
        else
            for key, item in ipairs(self.mTimerList.list()) do
                if item == delayObject then
                    self.mTimerList:Remove(item);
                    break;
                end
            end
        end
    end
end

function M:Advance(delta)
    self.incDepth();

    for key, timerItem in ipairs(self.mTimerList.list()) do
        if not timerItem:getClientDispose() then
            timerItem:OnFrameTimer();
        end
        if timerItem.mIsDisposed then
            self:removeObject(timerItem);
        end
    end

    self:decDepth();
end

return M;