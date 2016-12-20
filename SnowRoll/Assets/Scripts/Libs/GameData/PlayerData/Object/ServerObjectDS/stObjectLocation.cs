namespace SDK.Lib
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

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt32(ref dwLocation);
            bu.readUnsignedInt32(ref dwTableID);
            bu.readUnsignedInt16(ref x);
            bu.readUnsignedInt16(ref y);
        }

        public void serialize(ByteBuffer bu)
        {
            bu.writeUnsignedInt32(dwLocation);
            bu.writeUnsignedInt32(dwTableID);
            bu.writeUnsignedInt16(x);
            bu.writeUnsignedInt16(y);
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