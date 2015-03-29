using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace Game.Login
{
    public class LoginLogonUserCmdHandle : NetCmdHandleBase
    {
        public LoginLogonUserCmdHandle()
        {
            m_id2HandleDic[stLogonUserCmd.RETURN_CLIENT_IP_PARA] = LoginSys.m_instance.m_loginFlowHandle.receiveMsg2f;
            m_id2HandleDic[stLogonUserCmd.SERVER_RETURN_LOGIN_OK] = LoginSys.m_instance.m_loginFlowHandle.receiveMsg4f;
            m_id2HandleDic[stLogonUserCmd.SERVER_RETURN_LOGIN_FAILED] = LoginSys.m_instance.m_loginFlowHandle.psstServerReturnLoginFailedCmd;
        }
    }
}