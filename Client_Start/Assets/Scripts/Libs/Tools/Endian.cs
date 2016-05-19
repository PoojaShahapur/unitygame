namespace SDK.Lib
{
    public enum EEndian
    {
        eBIG_ENDIAN,         // 大端
        eLITTLE_ENDIAN,      // 小端
    }

    public class SystemEndian
    {
        static public EEndian m_sEndian = EEndian.eLITTLE_ENDIAN;     // 当前机器的编码
    }
}