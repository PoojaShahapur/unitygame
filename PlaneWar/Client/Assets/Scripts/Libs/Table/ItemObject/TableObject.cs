namespace SDK.Lib
{
    /**
    * @brief 道具基本表   
    */
    //public class TableItemObject : TableItemBase
    //{
    //    override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
    //    {
    //        if (null == mItemBody)
    //        {
    //            mItemBody = new TableObjectItemBody();
    //        }

    //        mItemBody.parseBodyByteBuffer(bytes, offset);
    //    }
    //}

    public class TableObjectItemBody : TableItemBodyBase
    {
        //public ulong m_field2;
        //public float m_field3;
        //public string m_field4;

        //override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        //{
        //    // 移动 pos 到内容开始处
        //    bytes.position = offset;  // 从偏移处继续读取真正的内容

        //    // 读取内容
        //    m_field2 = bytes.readUnsignedLong();
        //    m_field3 = bytes.readFloat();
        //    // 客户端读取字符串方法
        //    m_field4 = UtilTable.readString(bytes);
        //}

        public string mName;
        public int mMaxNum;
        public int mType;
        public int mColor;
        public string mObjResName;

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.setPos(offset);  // 从偏移处继续读取真正的内容
            UtilTable.readString(bytes, ref mName);
            bytes.readInt32(ref mMaxNum);
            bytes.readInt32(ref mType);
            bytes.readInt32(ref mColor);
            UtilTable.readString(bytes, ref mObjResName);
        }
    }
}