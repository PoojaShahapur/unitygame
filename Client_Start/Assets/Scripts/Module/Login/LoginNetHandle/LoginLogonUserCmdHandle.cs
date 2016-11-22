using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginLogonUserCmdHandle : NetCmdDispHandle
    {
        public LoginLogonUserCmdHandle()
        {
            this.addParamHandle(stLogonUserCmd.RETURN_CLIENT_IP_PARA, ((Ctx.m_instance.m_loginSys) as LoginSys).mLoginFlowHandle.receiveMsg2f);
            this.addParamHandle(stLogonUserCmd.SERVER_RETURN_LOGIN_OK, ((Ctx.m_instance.m_loginSys) as LoginSys).mLoginFlowHandle.receiveMsg4f);
            this.addParamHandle(stLogonUserCmd.SERVER_RETURN_LOGIN_FAILED, ((Ctx.m_instance.m_loginSys) as LoginSys).mLoginFlowHandle.psstServerReturnLoginFailedCmd);
        }
    }
}