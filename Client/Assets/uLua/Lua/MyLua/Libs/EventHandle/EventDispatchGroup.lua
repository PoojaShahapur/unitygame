require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.DataStruct.Dictionary"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "EventDispatchGroup";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_groupID2DispatchDic = GlobalNS.new(GlobalNS.Dictionary);
    self.m_bInLoop = false;
end

-- 添加分发器
function M:addEventDispatch(groupID, disp)
    if (not self.m_groupID2DispatchDic:ContainsKey(groupID)) then
        self.m_groupID2DispatchDic:Add(groupID, disp);
    end
end

function M:addEventHandle(groupID, pThis, handle)
    if(not self.m_groupID2DispatchDic:ContainsKey(groupID)) then
        local disp = GlobalNS.new(GlobalNS.EventDispatch);
        self:addEventDispatch(groupID, disp);
    end
    self.m_groupID2DispatchDic:value(groupID):addEventHandle(pThis, handle);
end

function M:removeEventHandle(groupID, pThis, handle)
    if (self.m_groupID2DispatchDic:ContainsKey(groupID)) then
        self.m_groupID2DispatchDic:value(groupID):removeEventHandle(pThis, handle);
    else
        -- 日志
    end
end

function M:dispatchEvent(groupID, dispatchObject)
    self.m_bInLoop = true;
    if (self.m_groupID2DispatchDic:ContainsKey(groupID)) then
        self.m_groupID2DispatchDic:value(groupID):dispatchEvent(dispatchObject);
    else
        -- 日志
    end
    self.m_bInLoop = false;
end

function M:clearAllEventHandle()
    if (not self.m_bInLoop) then
        for _, dispatch in pairs(self.m_groupID2DispatchDic:getData()) do
            dispatch:clearEventHandle();
        end

        self.m_groupID2DispatchDic:Clear();
    else
        -- 日志
    end
end

function M:clearGroupEventHandle(groupID)
    if (not self.m_bInLoop) then
        if self.m_groupID2DispatchDic:ContainsKey(groupID) then
            self.m_groupID2DispatchDic:value(groupID):clearEventHandle();
            self.m_groupID2DispatchDic:Remove(groupID);
        else
            -- 日志
        end
    else
        -- 日志
    end
end

return M;