﻿namespace SDK.Common
{
    public class t_MagicPoint
    {
        uint mp;
        uint maxmp;
        uint forbid;

        public void derialize(IByteArray ba)
        {
            mp = ba.readUnsignedInt();
            maxmp = ba.readUnsignedInt();
            forbid = ba.readUnsignedInt();
        }
    }

    //struct t_MagicPoint
    //{
    //DWORD mp;
    //DWORD maxmp;
    //DWORD forbid;
    //t_MagicPoint()
    //{
    //    mp = 0;
    //    maxmp = 0;
    //    forbid = 0;
    //}
    //};
}