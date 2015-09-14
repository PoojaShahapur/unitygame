﻿using SDK.Lib;

namespace Game.Msg
{
    public class stGameTimeTimerUserCmd : stTimerUserCmd
    {
        public ulong qwGameTime;

        public stGameTimeTimerUserCmd()
        {
            byParam = GAMETIME_TIMER_USERCMD_PARA;
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            ba.readUnsignedInt64(ref qwGameTime);
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