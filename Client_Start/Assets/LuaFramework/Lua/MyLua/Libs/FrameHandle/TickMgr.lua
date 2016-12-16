MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayHandleMgrBase");
MLoader("MyLua.Libs.FrameHandle.TickProcessObject");

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase);
M.clsName = "TickMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mTickList = GlobalNS.new(GlobalNS.MList);
end

function M:addTick(tickObj, priority)
    self:addObject(tickObj, priority);
end

function M:addObject(delayObject, priority)
    if self:bInDepth() then
        M.super.addObject(self, delayObject, priority);
    else
        local position = -1;
        local i = 0;
        for i = 0, i < self.mTickList:Count(), 1 do
            while true do
                if self.mTickList:at(i) == nil then
                    break;
                end
    
                if self.mTickList:at(i).mTickObject == delayObject then
                    return;
                end
    
                if self.mTickList:at(i).mPriority < priority then
                    position = i;
                    break;
                end
                
                break;
            end
        end

        local processObject = GlobalNS.new(GlobalNS.TickProcessObject);
        processObject.mTickObject = delayObject;
        processObject.mPriority = priority;

        if position < 0 or position >= self.mTickList:Count() then
            self.mTickList:Add(processObject);
        else
            self.mTickList:Insert(position, processObject);
        end
    end
end

function M:removeObject(delayObject)
    if self:bInDepth() then
        M.super.removeObject(self, delayObject);
    else
        for key, item in ipairs(self.mTickList:list()) do
            if item.mTickObject == delayObject then
                self.mTickList:Remove(item);
                break;
            end
        end
    end
end

function M:Advance(delta)
    self:incDepth();

    for key, tk in ipairs(self.mTickList:list()) do
        if not tk.mTickObject:getClientDispose() then
            tk.mTickObject:onTick(delta);
        end
    end

    self:decDepth();
end

return M;