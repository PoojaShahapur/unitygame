namespace SDK.Lib
{
    public class ItemBase
    {
        public uint m_uID;            // 唯一 ID

        virtual public void parseByteArray(ByteArray bytes)
        {
            m_uID = bytes.readUnsignedInt();
        }
    }
}