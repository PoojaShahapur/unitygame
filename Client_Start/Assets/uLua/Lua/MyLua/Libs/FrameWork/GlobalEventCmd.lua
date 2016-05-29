require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

if(GlobalNS.MacroDef.UNIT_TEST) then
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

-- 场景加载完成
function M.onSceneLoaded()
	if(GlobalNS.MacroDef.UNIT_TEST) then
		pTestMain = GlobalNS.new(GlobalNS.TestMain);
		pTestMain:run();
	end
end

return M;