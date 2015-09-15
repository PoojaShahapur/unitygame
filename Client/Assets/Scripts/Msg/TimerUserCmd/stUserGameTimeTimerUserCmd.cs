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

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeUnsignedInt64(dwUserTempID);
            bu.writeUnsignedInt64(qwGameTime);
        }

        //public override void derialize(ByteBuffer bu)
        //{
        //    base.derialize(bu);
        //    dwUserTempID = bu.readULong();
        //    qwGameTime = bu.readULong();
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