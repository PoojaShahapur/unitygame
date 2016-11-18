require "MyLua.Libs.Network.CmdDisp.NetModuleDispHandle"
require "MyLua.Module.Game.GameNetHandle.GameTestCmdHandle"

require "MyLua.Test.TestCmdDisp.TestNetHandleCB"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestCmdDisp";
GlobalNS[M.clsName] = M;

function M:ctor()
    self:init();
end

function M:dtor()

end

function M:init()
	local testNetHandleCB = GlobalNS.new(GlobalNS.TestNetHandleCB);
    GCtx.m_netCmdNotify:addOneDisp(testNetHandleCB);    -- 设置网络模块处理
end

function M:run()
	
end

return M;