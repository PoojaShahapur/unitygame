using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginTimerUserCmdHandle : NetCmdHandleBase
    {
        public LoginTimerUserCmdHandle()
        {
            this.addParamHandle(stTimerUserCmd.GAMETIME_TIMER_USERCMD_PARA, Ctx.m_instance.m_pTimerMsgHandle.receiveMsg7f);
            this.addParamHandle(stTimerUserCmd.REQUESTUSERGAMETIME_TIMER_USERCMD_PARA, Ctx.m_instance.m_pTimerMsgHandle.receiveMsg8f);
        }
    }
}