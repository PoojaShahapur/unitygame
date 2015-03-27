using SDK.Common;

namespace Game.Msg
{
    public class stGameTimeTimerUserCmd : stTimerUserCmd
    {
        public ulong qwGameTime;

        public stGameTimeTimerUserCmd()
        {
            byParam = GAMETIME_TIMER_USERCMD_PARA;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);
            qwGameTime = ba.readUnsignedLong();
        }
    }
}


///// 网关向用户发送游戏时间
//const BYTE GAMETIME_TIMER_USERCMD_PARA = 1;
//struct stGameTimeTimerUserCmd : public stTimerUserCmd 
//{
//  stGameTimeTimerUserCmd()
//  {
//    byParam = GAMETIME_TIMER_USERCMD_PARA;
//  }

//  QWORD qwGameTime;      /**< 游戏时间 */
//};