MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle");

local M = GlobalNS.Class(GlobalNS.NetCmdDispHandle_KBE);
M.clsName = "GameEventHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()
	GCtx.mNetCmdNotify_KBE:removeParamHandle("Client_onHelloCB", self, self.handleTest);
end

function M:init()
	GCtx.mNetCmdNotify_KBE:addParamHandle("Client_onHelloCB", self, self.handleTest);
end

function M:dtor()
    
end

function M:handleTest(cmd)
    
end

return M;