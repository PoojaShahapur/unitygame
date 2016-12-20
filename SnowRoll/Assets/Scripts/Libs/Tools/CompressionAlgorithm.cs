namespace SDK.Lib
{
    public enum CompressionAlgorithm
    {
        DEFLATE,            // 这个好像也是使用的是 ZLIB 这个压缩算法
 	 	ZLIB,               // ZLIB 压缩算法
        LZMA,               // 7Z 默认使用的压缩算法
    }
}