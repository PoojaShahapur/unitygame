namespace SDK.Common
{
    public class ItemBase
    {
        public uint m_uID;              // 唯一 ID
        public uint m_headerOffset;     // 头这一项在文件中的偏移
        public uint m_bodyOffset;       // 体这一项在文件中的偏移

        virtual public void parseAllByteArray(ByteArray bytes)
        {

        }

        virtual public void parseHeaderByteArray(ByteArray bytes)
        {
            m_headerOffset = bytes.position;
            m_uID = bytes.readUnsignedInt();
            m_bodyOffset = bytes.readUnsignedInt();
        }

        virtual public void parseBodyByteArray(ByteArray bytes)
        {

        }

        virtual public void parseByteArrayTestServer(ByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
        }
    }
}