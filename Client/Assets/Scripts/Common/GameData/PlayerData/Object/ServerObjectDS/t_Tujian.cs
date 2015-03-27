namespace SDK.Common
{
    public class t_Tujian
    {
        public uint id;
        public byte num;

        public void derialize(ByteArray ba)
        {
            id = ba.readUnsignedInt();
            num = ba.readUnsignedByte();
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