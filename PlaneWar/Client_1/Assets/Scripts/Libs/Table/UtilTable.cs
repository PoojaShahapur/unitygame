namespace SDK.Lib
{
    public class UtilTable
    {
        static public uint mPrePos;        // 记录之前的位置
        static public ushort msCnt;

        static public void readString(ByteBuffer bytes, ref string tmpStr)
        {
            bytes.readUnsignedInt16(ref msCnt);
            //bytes.readMultiByte(ref tmpStr, msCnt, GkEncode.UTF8);
            bytes.readMultiByte(ref tmpStr, msCnt, GkEncode.eUTF8);
        }
    }
}