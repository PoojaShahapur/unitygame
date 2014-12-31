namespace SDK.Common
{
    public class TableItemBase
    {
        public TableItemHeader m_itemHeader;
        public TableItemBodyBase m_itemBody;

        virtual public void parseHeaderByteArray(IByteArray bytes)
        {
            if (null == m_itemHeader)
            {
                m_itemHeader = new TableItemHeader();
            }
            m_itemHeader.parseHeaderByteArray(bytes);
        }

        virtual public void parseBodyByteArray<T>(IByteArray bytes, uint offset) where T : TableItemBodyBase, new()
        {
            if (null == m_itemBody)
            {
                m_itemBody = new T();
            }

            m_itemBody.parseBodyByteArray(bytes, offset);
        }

        virtual public void parseAllByteArray<T>(IByteArray bytes) where T : TableItemBodyBase, new()
        {
            // 解析头
            parseHeaderByteArray(bytes);
            // 保存下一个 Item 的头位置
            UtilTable.m_prePos = (bytes as ByteArray).position;
            // 解析内容
            parseBodyByteArray<T>(bytes, m_itemHeader.m_offset);
            // 移动到下一个 Item 头位置
            bytes.setPos(UtilTable.m_prePos);
        }
    }
}