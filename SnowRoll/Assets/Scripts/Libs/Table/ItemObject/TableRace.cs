namespace SDK.Lib
{
    public class TableRaceItemBody : TableItemBodyBase
    {
        public string mRaceName;                // 种族名称

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref mRaceName);
        }
    }
}
