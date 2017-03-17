MLoader("MyLua.Libs.Network.CmdDisp.NetModuleDispHandle");
MLoader("MyLua.Module.Game.GameNetHandle.GameTestCmdHandle");

local M = GlobalNS.Class(GlobalNS.NetModuleDispHandle);
M.clsName = "GameNetHandleCB_KBE";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mGameEventHandle = GlobalNS.new(GlobalNS.GameEventHandle);
end

function M:dtor()

end

function M:init()
	self.mGameEventHandle:init();
end

return M;