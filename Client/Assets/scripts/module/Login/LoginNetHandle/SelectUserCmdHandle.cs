using Game.Msg;
using SDK.Common;
namespace Game.Login
{
    public class SelectUserCmdHandle : NetCmdHandleBase
    {
        public SelectUserCmdHandle()
        {
            m_id2HandleDic[stSelectUserCmd.USERINFO_SELECT_USERCMD_PARA] = LoginSys.m_instance.m_loginFlowHandle.psstUserInfoUserCmd;
            m_id2HandleDic[stSelectUserCmd.LOGIN_SELECT_SUCCESS_USERCMD_PARA] = LoginSys.m_instance.m_loginFlowHandle.psstLoginSelectSuccessUserCmd;
        }
    }
}