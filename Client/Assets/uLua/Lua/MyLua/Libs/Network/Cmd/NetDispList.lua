require "MyLua.Libs.Network.Cmd.NetDispHandle"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetDispList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_revMsgCnt = 0;
    self.m_handleMsgCnt = 0;
    self.m_netDispList = GlobalNS.new(GlobalNS.MList);
    self.m_bStopNetHandle = false;
end

function M:getStopNetHandle()
    return self.m_bStopNetHandle;
end

function M:setStopNetHandle(value)
    self.m_bStopNetHandle = value;
end

function M:addOneDisp(disp)
    if(self.m_netDispList:IndexOf(disp) == -1) then
        self.m_netDispList:Add(disp);
    end
end

function M:removeOneDisp(disp)
    if(self.m_netDispList:IndexOf(disp) ~= -1) then
        self.m_netDispList:Remove(disp);
    end
end

function M:handleMsg(msg)
    --if (false == m_bStopNetHandle) then -- 如果没有停止网络处理
        for _, item in pairs(self.m_netDispList:list()) do
            item:handleMsg(msg);
        end
    --end
end

function M:addOneRevMsg()
    self.m_revMsgCnt = self.m_revMsgCnt + 1;
end

function M:addOneHandleMsg()
    self.m_handleMsgCnt = self.m_handleMsgCnt + 1;
end

function M:clearOneRevMsg()
    self.m_revMsgCnt = 0;
end

function M:clearOneHandleMsg()
    self.m_handleMsgCnt = 0;
end