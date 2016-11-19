﻿namespace SDK.Lib
{
    public class t_MainUserData
    {
        public uint m_dwUserTempID;             // 用户临时 id
        public string m_name = "";                   //玩家名字
        public uint m_gold;                     //玩家的金币

        public void derialize(ByteBuffer bu)
        {
            bu.readMultiByte(ref m_name, ProtoCV.MAX_NAMESIZE + 1, GkEncode.eUTF8);
            bu.readUnsignedInt32(ref m_gold);
        }
    }
}