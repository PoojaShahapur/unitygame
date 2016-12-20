using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginSelectUserCmdHandle : NetCmdDispHandle
    {
        public LoginSelectUserCmdHandle()
        {
            this.addParamHandle(stSelectUserCmd.USERINFO_SELECT_USERCMD_PARA, ((Ctx.mInstance.mLoginSys) as LoginSys).mLoginFlowHandle.psstUserInfoUserCmd);
            this.addParamHandle(stSelectUserCmd.LOGIN_SELECT_SUCCESS_USERCMD_PARA, ((Ctx.mInstance.mLoginSys) as LoginSys).mLoginFlowHandle.psstLoginSelectSuccessUserCmd);
        }
    }
}