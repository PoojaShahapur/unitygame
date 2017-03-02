using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginLogonUserCmdHandle : NetCmdDispHandle
    {
        public LoginLogonUserCmdHandle()
        {
            this.addParamHandle(stLogonUserCmd.RETURN_CLIENT_IP_PARA, ((Ctx.mInstance.mLoginSys) as LoginSys).mLoginFlowHandle.receiveMsg2f);
            this.addParamHandle(stLogonUserCmd.SERVER_RETURN_LOGIN_OK, ((Ctx.mInstance.mLoginSys) as LoginSys).mLoginFlowHandle.receiveMsg4f);
            this.addParamHandle(stLogonUserCmd.SERVER_RETURN_LOGIN_FAILED, ((Ctx.mInstance.mLoginSys) as LoginSys).mLoginFlowHandle.psstServerReturnLoginFailedCmd);
        }
    }
}