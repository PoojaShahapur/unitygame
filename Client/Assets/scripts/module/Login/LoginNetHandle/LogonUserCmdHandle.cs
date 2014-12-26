using Game.Msg;
using SDK.Common;

namespace Game.Login
{
    public class LogonUserCmdHandle : NetCmdHandleBase
    {
        public LogonUserCmdHandle()
        {
            m_id2HandleDic[stLogonUserCmd.RETURN_CLIENT_IP_PARA] = LoginSys.m_instance.m_loginFlowHandle.receiveMsg2f;
            m_id2HandleDic[stLogonUserCmd.SERVER_RETURN_LOGIN_OK] = LoginSys.m_instance.m_loginFlowHandle.receiveMsg4f;
            m_id2HandleDic[stLogonUserCmd.SERVER_RETURN_LOGIN_FAILED] = LoginSys.m_instance.m_loginFlowHandle.psstServerReturnLoginFailedCmd;
        }
    }
}