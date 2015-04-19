namespace SDK.Common
{
    public class DataCV
    {
        public const uint PACKET_ZIP_MIN = 32;
        public const uint PACKET_ZIP = 0x40000000;
        public const uint HEADER_SIZE = 4;   // 包长度占据几个字节

        public const uint INIT_ELEM_CAPACITY = 32;               // 默认分配 32 元素
        public const uint INIT_CAPACITY = 1 * 1024;               // 默认分配 1 K
        public const uint MAX_CAPACITY = 8 * 1024 * 1024;      // 最大允许分配 8 M
    }
}