namespace Game.Msg
{
    public class stRequestUserGameTimeTimerUserCmd : stTimerUserCmd
    {
        public stRequestUserGameTimeTimerUserCmd()
        {
            byParam = REQUESTUSERGAMETIME_TIMER_USERCMD_PARA;
        }
    }
}


/// 网关向用户请求时间
//const BYTE REQUESTUSERGAMETIME_TIMER_USERCMD_PARA = 2;
//struct stRequestUserGameTimeTimerUserCmd : public stTimerUserCmd
//{
//  stRequestUserGameTimeTimerUserCmd()
//  {
//    byParam = REQUESTUSERGAMETIME_TIMER_USERCMD_PARA;
//  }

//};