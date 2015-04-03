using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace Game.Login
{
    public class LoginLogonUserCmdHandle : NetCmdHandleBase
    {
        public LoginLogonUserCmdHandle()
        {
            m_id2HandleDic[stLogonUserCmd.RETURN_CLIENT_IP_PARA] = ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.receiveMsg2f;
            m_id2HandleDic[stLogonUserCmd.SERVER_RETURN_LOGIN_OK] = ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.receiveMsg4f;
            m_id2HandleDic[stLogonUserCmd.SERVER_RETURN_LOGIN_FAILED] = ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.psstServerReturnLoginFailedCmd;
        }
    }
}