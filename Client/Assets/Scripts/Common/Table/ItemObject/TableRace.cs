namespace SDK.Common
{
    public class TableRaceItemBody : TableItemBodyBase
    {
        public string m_raceName;                // 种族名称

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref m_raceName);
        }
    }
}
