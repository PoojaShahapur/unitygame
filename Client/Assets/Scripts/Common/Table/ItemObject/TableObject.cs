using SDK.Lib;
namespace SDK.Common
{
    /**
    * @brief 道具基本表   
    */
    //public class TableItemObject : TableItemBase
    //{
    //    override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
    //    {
    //        if (null == m_itemBody)
    //        {
    //            m_itemBody = new TableObjectItemBody();
    //        }

    //        m_itemBody.parseBodyByteBuffer(bytes, offset);
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
        //    (bytes as ByteBuffer).position = offset;  // 从偏移处继续读取真正的内容

        //    // 读取内容
        //    m_field2 = bytes.readUnsignedLong();
        //    m_field3 = bytes.readFloat();
        //    // 客户端读取字符串方法
        //    m_field4 = UtilTable.readString(bytes as ByteBuffer);
        //}

        public string m_name;
        public int m_maxNum;
        public int m_type;
        public int m_color;
        public string m_prefab;

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            (bytes as ByteBuffer).position = offset;  // 从偏移处继续读取真正的内容
            m_name = UtilTable.readString(bytes as ByteBuffer);
            m_maxNum = bytes.readInt32();
            m_type = bytes.readInt32();
            m_color = bytes.readInt32();
            m_prefab = UtilTable.readString(bytes as ByteBuffer);
        }

        public string path
        {
            get
            {
                return Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + m_prefab;
            }
        }
    }
}