namespace SDK.Common
{
    public class ItemBase
    {
        public uint m_uID;              // 唯一 ID
        public uint m_offset;           // 这一项在文件中的偏移

        virtual public void parseByteArray(ByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
            m_offset = bytes.readUnsignedInt();
            UtilTable.m_prePos = bytes.position;
        }

        virtual public void parseByteArrayTestServer(ByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
        }
    }
}