using Game.Msg;
using SDK.Lib;
using SDK.Lib;

namespace Game.Game
{
    public class GameTimeCmdHandle : NetCmdHandleBase
    {
        public GameTimeCmdHandle()
        {
            m_id2HandleDic[stTimerUserCmd.GAMETIME_TIMER_USERCMD_PARA] = Ctx.m_instance.m_pTimerMsgHandle.receiveMsg7f;
            m_id2HandleDic[stTimerUserCmd.REQUESTUSERGAMETIME_TIMER_USERCMD_PARA] = Ctx.m_instance.m_pTimerMsgHandle.receiveMsg8f;
        }
    }
}