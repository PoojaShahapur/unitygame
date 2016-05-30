namespace SDK.Lib
{
    public class BufferCV
    {
        public const uint INIT_ELEM_CAPACITY = 32;          // 默认分配 32 元素
        public const uint INIT_CAPACITY = 1 * 1024;         // 默认分配 1 K
        public const uint MAX_CAPACITY = 8 * 1024 * 1024;   // 最大允许分配 8 M
    }
}