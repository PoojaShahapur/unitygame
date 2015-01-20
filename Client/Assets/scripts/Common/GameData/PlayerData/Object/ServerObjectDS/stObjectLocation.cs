namespace SDK.Common
{
    public class stObjectLocation
    {
        public uint dwLocation;
        public uint dwTableID;
        public ushort x;
        public ushort y;

        public void derialize(IByteArray ba)
        {
            dwLocation = ba.readUnsignedInt();
            dwTableID = ba.readUnsignedInt();
            x = ba.readUnsignedShort();
            y = ba.readUnsignedShort();
        }

        public void serialize(IByteArray ba)
        {
            ba.writeUnsignedInt(dwLocation);
            ba.writeUnsignedInt(dwTableID);
            ba.writeUnsignedShort(x);
            ba.writeUnsignedShort(y);
        }
    }
}


//struct stObjectLocation
//{
//    DWORD dwLocation;   // 格子类型  1
//    DWORD dwTableID;    // 包袱ID    ->0
//    WORD x;		//行
//    WORD y;		//列
//}