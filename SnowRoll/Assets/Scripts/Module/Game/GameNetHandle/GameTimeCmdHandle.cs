using Game.Msg;
using SDK.Lib;

namespace Game.Game
{
    public class GameTimeCmdHandle : NetCmdDispHandle
    {
        public GameTimeCmdHandle()
        {
            this.addParamHandle(stTimerUserCmd.GAMETIME_TIMER_USERCMD_PARA, Ctx.mInstance.mTimerMsgHandle.receiveMsg7f);
            this.addParamHandle(stTimerUserCmd.REQUESTUSERGAMETIME_TIMER_USERCMD_PARA, Ctx.mInstance.mTimerMsgHandle.receiveMsg8f);
        }
    }
}