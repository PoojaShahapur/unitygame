using SDK.Common;

namespace Game.Msg
{
    public class stTimerUserCmd : stNullUserCmd
    {
        public const byte GAMETIME_TIMER_USERCMD_PARA = 1;
        public const byte REQUESTUSERGAMETIME_TIMER_USERCMD_PARA = 2;
        public const byte USERGAMETIME_TIMER_USERCMD_PARA = 3;

        public stTimerUserCmd()
        {
            byCmd = TIME_USERCMD;
        }
    }
}


//struct stTimerUserCmd : public stNullUserCmd
//{
//  stTimerUserCmd()
//  {
//    byCmd = TIME_USERCMD;
//  }
//};