using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginSelectUserCmdHandle : NetCmdDispHandle
    {
        public LoginSelectUserCmdHandle()
        {
            this.addParamHandle(stSelectUserCmd.USERINFO_SELECT_USERCMD_PARA, ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.psstUserInfoUserCmd);
            this.addParamHandle(stSelectUserCmd.LOGIN_SELECT_SUCCESS_USERCMD_PARA, ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.psstLoginSelectSuccessUserCmd);
        }
    }
}