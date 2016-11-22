using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginTimerUserCmdHandle : NetCmdDispHandle
    {
        public LoginTimerUserCmdHandle()
        {
            this.addParamHandle(stTimerUserCmd.GAMETIME_TIMER_USERCMD_PARA, Ctx.mInstance.mTimerMsgHandle.receiveMsg7f);
            this.addParamHandle(stTimerUserCmd.REQUESTUSERGAMETIME_TIMER_USERCMD_PARA, Ctx.mInstance.mTimerMsgHandle.receiveMsg8f);
        }
    }
}