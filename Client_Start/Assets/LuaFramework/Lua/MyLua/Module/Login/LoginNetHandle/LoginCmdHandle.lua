MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle");

local M = GlobalNS.Class(GlobalNS.NetCmdDispHandle);
M.clsName = "LoginCmdHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
	self:addParamHandle(LoginResponse, self, self.handleLoginResponse);
end

function M:dtor()
    
end

function M:handleLoginResponse(cmd)
	GCtx.mLogSys:log(cmd.result, GlobalNS.LogTypeId.eLogCommon);
end

return M;