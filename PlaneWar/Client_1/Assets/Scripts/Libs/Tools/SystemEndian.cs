namespace SDK.Lib
{
    public class SystemEndian
    {
        static public EEndian msLocalEndian = EEndian.eLITTLE_ENDIAN;   // 本地字节序
        static public EEndian msNetEndian = EEndian.eBIG_ENDIAN;        // 网络字节序
        static public EEndian msServerEndian = SystemEndian.msNetEndian;// 服务器字节序，规定服务器字节序就是网络字节序
    }
}