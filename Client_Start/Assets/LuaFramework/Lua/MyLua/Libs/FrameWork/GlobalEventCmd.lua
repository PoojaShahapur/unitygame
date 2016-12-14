MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.Test.TestMain");

if(MacroDef.UNIT_TEST) then
	MLoader("MyLua.Test.TestMain");
end

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

function M.onReceiveToLuaRpc(buffer, length)
    GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLuaRpc", GlobalNS.LogTypeId.eLogCommon);
    GCtx.mNetMgr:receiveCmdRpc(buffer, length);
end

-- 接收消息, KBE
function M.onReceiveToLua_KBE(msgName, param)
    GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLua_KBE", GlobalNS.LogTypeId.eLogCommon);
	GCtx.mNetCmdNotify_KBE:handleMsg(msgName, param);
end

-- 场景加载完成
function M.onSceneLoaded()
	if(MacroDef.UNIT_TEST) then
		pTestMain = GlobalNS.new(GlobalNS.TestMain);
		pTestMain:run();
	end

    --加载场景上的UI组件
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormID.eUIPlayerDataPanel);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormID.eUIForwardForce);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormID.eUIOptionPanel);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormID.eUITopXRankPanel);
end

-- 帧循环
function M.onAdvance(delta)
	GCtx.mProcessSys:advance(delta);
end

function M.openForm(formId)
	GCtx.mUiMgr:loadAndShow(formId);
end

function M.exitForm(formId)
	GCtx.mUiMgr:exitForm(formId);
end

function M.requireFile(filePath)
	return MLoader(filePath);
end

return M;