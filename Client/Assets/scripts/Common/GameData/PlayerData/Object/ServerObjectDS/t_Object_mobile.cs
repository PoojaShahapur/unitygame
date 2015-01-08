﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDK.Common
{
    public class t_Object_mobile
    {
        public uint dwThisID;
        public uint dwObjectID;
        public stObjectLocation pos;
        public uint dwNum;

        public void derialize(IByteArray ba)
        {
            dwThisID = ba.readUnsignedInt();
            dwObjectID = ba.readUnsignedInt();
            pos = new stObjectLocation();
            pos.derialize(ba);
            dwNum = ba.readUnsignedInt();
        }
    }
}


/**
 * \brief   手游 服务器和客户端通信道具数据
*/
//typedef struct t_Object_mobile
//{
//    DWORD dwThisID;   //物品唯一id
//    DWORD dwObjectID;  //物品表中的编号
//    stObjectLocation pos;	// 位置
//    DWORD dwNum;	// 数量
//};