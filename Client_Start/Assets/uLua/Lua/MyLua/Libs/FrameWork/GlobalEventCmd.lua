require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

--[[
    处理 CS 到 Lua 的全局事件
]]
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "GlobalEventCmd";
GlobalNS[M.clsName] = M;

-- 接收消息
function M.onReceiveToLua(id, buffer)
    GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLua", GlobalNS.LogTypeId.eLogCommon);
    GCtx.mNetMgr:receiveCmd(id, buffer);
end

-- 场景加载完成
function M.onSceneLoaded()
	GCtx.mUIMgr:loadAndShow(GlobalNS.UIFormID.eUITest);
	M.testSendMsg();
end

-- 测试发送消息
function M.testSendMsg()
	local form = GCtx.mUIMgr:getForm(GlobalNS.UIFormID.eUITest);
	if(form ~= nil) then
		form:testSendMsg();
	end
end

return M;