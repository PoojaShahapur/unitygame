namespace SDK.Common
{
    public class UtilTable
    {
        static public uint m_prePos;        // 记录之前的位置
        static public ushort m_sCnt;

        static public void readString(ByteBuffer bytes, ref string tmpStr)
        {
            bytes.readUnsignedInt16(ref m_sCnt);
            bytes.readMultiByte(ref tmpStr, m_sCnt, GkEncode.UTF8);
        }
    }
}