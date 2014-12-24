namespace SDK.Common
{
    public class ItemBase
    {
        public ItemHeader m_itemHeader;
        public ItemBodyBase m_itemBody;

        virtual public void parseHeaderByteArray(IByteArray bytes)
        {
            if (null == m_itemHeader)
            {
                m_itemHeader = new ItemHeader();
            }
            m_itemHeader.parseHeaderByteArray(bytes);
        }

        virtual public void parseBodyByteArray(IByteArray bytes, uint offset)
        {
            
        }

        virtual public void parseAllByteArray(IByteArray bytes)
        {
            // 解析头
            parseHeaderByteArray(bytes);
            // 保存下一个 Item 的头位置
            UtilTable.m_prePos = (bytes as ByteArray).position;
            // 解析内容
            parseBodyByteArray(bytes, m_itemHeader.m_offset);
            // 移动到下一个 Item 头位置
            bytes.setPos(UtilTable.m_prePos);
        }
    }
}