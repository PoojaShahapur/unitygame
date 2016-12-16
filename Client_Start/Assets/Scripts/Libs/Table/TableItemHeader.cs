namespace SDK.Lib
{
    public class TableItemHeader
    {
        public uint mId;              // 唯一 ID
        public uint mOffset;           // 这一项在文件中的偏移

        // 解析头部
        virtual public void parseHeaderByteBuffer(ByteBuffer bytes)
        {
            bytes.readUnsignedInt32(ref mId);
            bytes.readUnsignedInt32(ref mOffset);
        }
    }
}