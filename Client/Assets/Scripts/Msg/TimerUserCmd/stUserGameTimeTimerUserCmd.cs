using SDK.Lib;

namespace Game.Msg
{
    public class stUserGameTimeTimerUserCmd : stTimerUserCmd
    {
        public ulong dwUserTempID;
        public ulong qwGameTime;

        public stUserGameTimeTimerUserCmd()
        {
            byParam = USERGAMETIME_TIMER_USERCMD_PARA;
        }

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt64(dwUserTempID);
            ba.writeUnsignedInt64(qwGameTime);
        }

        //public override void derialize(ByteBuffer ba)
        //{
        //    base.derialize(ba);
        //    dwUserTempID = ba.readULong();
        //    qwGameTime = ba.readULong();
        //}
    }
}


/// 用户向网关发送当前游戏时间
//const BYTE USERGAMETIME_TIMER_USERCMD_PARA  = 3;
//struct stUserGameTimeTimerUserCmd : public stTimerUserCmd
//{
//  stUserGameTimeTimerUserCmd()
//  {
//    byParam = USERGAMETIME_TIMER_USERCMD_PARA;
//  }

//  DWORD dwUserTempID;      /**< 用户临时ID */
//  QWORD qwGameTime;      /**< 用户游戏时间 */
//};