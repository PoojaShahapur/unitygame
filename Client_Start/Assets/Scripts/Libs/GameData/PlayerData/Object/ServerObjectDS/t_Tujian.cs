﻿namespace SDK.Lib
{
    public class t_Tujian
    {
        public uint id;
        public byte num;

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt32(ref id);
            bu.readUnsignedInt8(ref num);
        }
    }

    //struct t_Tujian
    //{
    //    DWORD id;   //baseID
    //    BYTE num;   //个数
    //    t_Tujian()
    //    {
    //        id = 0;
    //        num = 0;
    //    }
    //};
}