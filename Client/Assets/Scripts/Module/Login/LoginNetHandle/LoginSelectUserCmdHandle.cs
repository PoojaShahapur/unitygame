using Game.Msg;
using SDK.Lib;
using SDK.Lib;
namespace Game.Login
{
    public class LoginSelectUserCmdHandle : NetCmdHandleBase
    {
        public LoginSelectUserCmdHandle()
        {
            m_id2HandleDic[stSelectUserCmd.USERINFO_SELECT_USERCMD_PARA] = ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.psstUserInfoUserCmd;
            m_id2HandleDic[stSelectUserCmd.LOGIN_SELECT_SUCCESS_USERCMD_PARA] = ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.psstLoginSelectSuccessUserCmd;
        }
    }
}