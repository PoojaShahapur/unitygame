namespace SDK.Lib
{
    public class t_MagicPoint
    {
        public uint mp;
        public uint maxmp;
        public uint forbid;

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt32(ref mp);
            bu.readUnsignedInt32(ref maxmp);
            bu.readUnsignedInt32(ref forbid);
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