namespace SDK.Common
{
    /**
    * @brief 掉落物表   
    */
    //public class TableItemObject : TableItemBase
    //{
    //    override public void parseBodyByteArray(IByteArray bytes, uint offset)
    //    {
    //        if (null == m_itemBody)
    //        {
    //            m_itemBody = new TableObjectItemBody();
    //        }

    //        m_itemBody.parseBodyByteArray(bytes, offset);
    //    }
    //}

    public class TableObjectItemBody : TableItemBodyBase
    {
        //public ulong m_field2;
        //public float m_field3;
        //public string m_field4;

        //override public void parseBodyByteArray(IByteArray bytes, uint offset)
        //{
        //    // 移动 pos 到内容开始处
        //    (bytes as ByteArray).position = offset;  // 从偏移处继续读取真正的内容

        //    // 读取内容
        //    m_field2 = bytes.readUnsignedLong();
        //    m_field3 = bytes.readFloat();
        //    // 客户端读取字符串方法
        //    m_field4 = UtilTable.readString(bytes as ByteArray);
        //}

        public string m_name;
        public int m_maxNum;
        public int m_type;
        public int m_color;
        public string m_prefab;

        override public void parseBodyByteArray(IByteArray bytes, uint offset)
        {
            (bytes as ByteArray).position = offset;  // 从偏移处继续读取真正的内容
            m_name = UtilTable.readString(bytes as ByteArray);
            m_maxNum = bytes.readInt();
            m_type = bytes.readInt();
            m_color = bytes.readInt();
            m_prefab = UtilTable.readString(bytes as ByteArray);
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