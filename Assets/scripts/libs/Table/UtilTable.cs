using SDK.Common;
namespace SDK.Lib
{
    public class UtilTable
    {
        static public uint m_sCnt;
        static public string readString(ByteArray bytes)
        {
            m_sCnt = bytes.readUnsignedShort();
            return bytes.readMultiByte(m_sCnt, GkEncode.UTF8);
        }
    }
}