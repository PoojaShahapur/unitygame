using Game.Msg;
using SDK.Common;
using SDK.Lib;
namespace Game.Login
{
    public class LoginSelectUserCmdHandle : NetCmdHandleBase
    {
        public LoginSelectUserCmdHandle()
        {
            m_id2HandleDic[stSelectUserCmd.USERINFO_SELECT_USERCMD_PARA] = LoginSys.m_instance.m_loginFlowHandle.psstUserInfoUserCmd;
            m_id2HandleDic[stSelectUserCmd.LOGIN_SELECT_SUCCESS_USERCMD_PARA] = LoginSys.m_instance.m_loginFlowHandle.psstLoginSelectSuccessUserCmd;
        }
    }
}