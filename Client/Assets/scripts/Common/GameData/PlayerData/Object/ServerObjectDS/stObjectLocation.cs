namespace SDK.Common
{
    /**
     * @brief 场景卡牌移动的时候， dwLocation 表示区域， y 表示位置， x 和 dwTableID 没有使用
     */
    public class stObjectLocation
    {
        public uint dwLocation;
        public uint dwTableID;
        public ushort x;
        public ushort y;

        public void derialize(ByteArray ba)
        {
            dwLocation = ba.readUnsignedInt();
            dwTableID = ba.readUnsignedInt();
            x = ba.readUnsignedShort();
            y = ba.readUnsignedShort();
        }

        public void serialize(ByteArray ba)
        {
            ba.writeUnsignedInt(dwLocation);
            ba.writeUnsignedInt(dwTableID);
            ba.writeUnsignedShort(x);
            ba.writeUnsignedShort(y);
        }

        public void copyFrom(stObjectLocation rhv)
        {
            dwLocation = rhv.dwLocation;
            dwTableID = rhv.dwTableID;
            x = rhv.x;
            y = rhv.y;
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