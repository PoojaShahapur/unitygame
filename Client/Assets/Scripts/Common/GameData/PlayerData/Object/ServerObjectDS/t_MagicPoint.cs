namespace SDK.Common
{
    public class t_MagicPoint
    {
        public uint mp;
        public uint maxmp;
        public uint forbid;

        public void derialize(ByteBuffer ba)
        {
            ba.readUnsignedInt32(ref mp);
            ba.readUnsignedInt32(ref maxmp);
            ba.readUnsignedInt32(ref forbid);
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