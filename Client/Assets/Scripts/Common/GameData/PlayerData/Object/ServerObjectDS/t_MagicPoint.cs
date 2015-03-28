namespace SDK.Common
{
    public class t_MagicPoint
    {
        public uint mp;
        public uint maxmp;
        public uint forbid;

        public void derialize(ByteBuffer ba)
        {
            mp = ba.readUnsignedInt32();
            maxmp = ba.readUnsignedInt32();
            forbid = ba.readUnsignedInt32();
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