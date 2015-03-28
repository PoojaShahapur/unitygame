namespace SDK.Common
{
    public class t_Tujian
    {
        public uint id;
        public byte num;

        public void derialize(ByteBuffer ba)
        {
            id = ba.readUnsignedInt32();
            num = ba.readUnsignedInt8();
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