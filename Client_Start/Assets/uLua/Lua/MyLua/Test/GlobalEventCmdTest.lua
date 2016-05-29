require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

--[[
    处理 CS 到 Lua 的全局事件
]]
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "GlobalEventCmdTest";
GlobalNS[M.clsName] = M;

-- 测试发送消息
function M.testSendMsg()
	local form = GCtx.mUIMgr:getForm(GlobalNS.UIFormID.eUITest);
	if(form ~= nil) then
		form:testSendMsg();
	end
end

return M;