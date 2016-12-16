MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.DataStruct.MDictionary");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "EventDispatchGroup";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mGroupID2DispatchDic = GlobalNS.new(GlobalNS.MDictionary);
    self.mIsInLoop = false;
end

-- 添加分发器
function M:addEventDispatch(groupID, disp)
    if (not self.mGroupID2DispatchDic:ContainsKey(groupID)) then
        self.mGroupID2DispatchDic:Add(groupID, disp);
    end
end

function M:addEventHandle(groupID, pThis, handle)
    if(not self.mGroupID2DispatchDic:ContainsKey(groupID)) then
        local disp = GlobalNS.new(GlobalNS.EventDispatch);
        self:addEventDispatch(groupID, disp);
    end
    self.mGroupID2DispatchDic:value(groupID):addEventHandle(pThis, handle);
end

function M:removeEventHandle(groupID, pThis, handle)
    if (self.mGroupID2DispatchDic:ContainsKey(groupID)) then
        self.mGroupID2DispatchDic:value(groupID):removeEventHandle(pThis, handle);
    else
        -- 日志
    end
end

function M:dispatchEvent(groupID, dispatchObject)
    self.mIsInLoop = true;
    if (self.mGroupID2DispatchDic:ContainsKey(groupID)) then
        self.mGroupID2DispatchDic:value(groupID):dispatchEvent(dispatchObject);
    else
        -- 日志
    end
    self.mIsInLoop = false;
end

function M:clearAllEventHandle()
    if (not self.mIsInLoop) then
        for _, dispatch in pairs(self.mGroupID2DispatchDic:getData()) do
            dispatch:clearEventHandle();
        end

        self.mGroupID2DispatchDic:Clear();
    else
        -- 日志
    end
end

function M:clearGroupEventHandle(groupID)
    if (not self.mIsInLoop) then
        if self.mGroupID2DispatchDic:ContainsKey(groupID) then
            self.mGroupID2DispatchDic:value(groupID):clearEventHandle();
            self.mGroupID2DispatchDic:Remove(groupID);
        else
            -- 日志
        end
    else
        -- 日志
    end
end

return M;