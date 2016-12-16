namespace SDK.Lib
{
    public class TableItemBase
    {
        public TableItemHeader mItemHeader;
        public TableItemBodyBase mItemBody;

        virtual public void parseHeaderByteBuffer(ByteBuffer bytes)
        {
            if (null == mItemHeader)
            {
                mItemHeader = new TableItemHeader();
            }
            mItemHeader.parseHeaderByteBuffer(bytes);
        }

        virtual public void parseBodyByteBuffer<T>(ByteBuffer bytes, uint offset) where T : TableItemBodyBase, new()
        {
            if (null == mItemBody)
            {
                mItemBody = new T();
            }

            mItemBody.parseBodyByteBuffer(bytes, offset);
        }

        virtual public void parseAllByteBuffer<T>(ByteBuffer bytes) where T : TableItemBodyBase, new()
        {
            // 解析头
            parseHeaderByteBuffer(bytes);
            // 保存下一个 Item 的头位置
            UtilTable.m_prePos = bytes.position;
            // 解析内容
            parseBodyByteBuffer<T>(bytes, mItemHeader.mOffset);
            // 移动到下一个 Item 头位置
            bytes.setPos(UtilTable.m_prePos);
        }
    }
}