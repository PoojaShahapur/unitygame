namespace SDK.Common
{
    public class ItemBase
    {
        public uint m_uID;              // 唯一 ID
        public uint m_offset;           // 这一项在文件中的偏移
        public bool m_bLoadAll = false;        // 是否整个内容全部加载

        // 解析头部
        virtual public void parseHeaderByteArray(IByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
            m_offset = bytes.readUnsignedInt();
            UtilTable.m_prePos = (bytes as ByteArray).position;
        }

        // 解析主要数据部分
        virtual public void parseBodyByteArray(IByteArray bytes)
        {
            
        }

        virtual public void parseAllByteArray(IByteArray bytes)
        {

        }

        virtual public void parseByteArrayTestServer(IByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
        }
    }
}