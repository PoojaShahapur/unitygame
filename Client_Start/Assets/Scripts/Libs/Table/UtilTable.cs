namespace SDK.Lib
{
    public class UtilTable
    {
        static public uint m_prePos;        // 记录之前的位置
        static public ushort mSCnt;

        static public void readString(ByteBuffer bytes, ref string tmpStr)
        {
            bytes.readUnsignedInt16(ref mSCnt);
            //bytes.readMultiByte(ref tmpStr, mSCnt, GkEncode.UTF8);
            bytes.readMultiByte(ref tmpStr, mSCnt, GkEncode.eUTF8);
        }
    }
}