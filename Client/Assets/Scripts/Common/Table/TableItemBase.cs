namespace SDK.Common
{
    public class TableItemBase
    {
        public TableItemHeader m_itemHeader;
        public TableItemBodyBase m_itemBody;

        virtual public void parseHeaderByteBuffer(ByteBuffer bytes)
        {
            if (null == m_itemHeader)
            {
                m_itemHeader = new TableItemHeader();
            }
            m_itemHeader.parseHeaderByteBuffer(bytes);
        }

        virtual public void parseBodyByteBuffer<T>(ByteBuffer bytes, uint offset) where T : TableItemBodyBase, new()
        {
            if (null == m_itemBody)
            {
                m_itemBody = new T();
            }

            m_itemBody.parseBodyByteBuffer(bytes, offset);
        }

        virtual public void parseAllByteBuffer<T>(ByteBuffer bytes) where T : TableItemBodyBase, new()
        {
            // 解析头
            parseHeaderByteBuffer(bytes);
            // 保存下一个 Item 的头位置
            UtilTable.m_prePos = bytes.position;
            // 解析内容
            parseBodyByteBuffer<T>(bytes, m_itemHeader.m_offset);
            // 移动到下一个 Item 头位置
            bytes.setPos(UtilTable.m_prePos);
        }
    }
}