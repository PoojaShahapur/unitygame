namespace SDK.Common
{
    public class UtilTable
    {
        static public uint m_prePos;        // 记录之前的位置
        static public uint m_sCnt;
        static public string readString(ByteBuffer bytes)
        {
            m_sCnt = bytes.readUnsignedInt16();
            return bytes.readMultiByte(m_sCnt, GkEncode.UTF8);
        }
    }
}