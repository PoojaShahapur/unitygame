MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

if(MacroDef.UNIT_TEST) then
	require "MyLua.Test.TestMain"
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


-- 场景加载完成
function M.onSceneLoaded()
	if(MacroDef.UNIT_TEST) then
		pTestMain = GlobalNS.new(GlobalNS.TestMain);
		pTestMain:run();
	end
end

-- 帧循环
function M.onAdvance(delta)
	GCtx.mProcessSys:advance(delta);
end

return M;