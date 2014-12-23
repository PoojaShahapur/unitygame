namespace SDK.Common
{
	/**
	 * @brief 掉落物表   
	 */
	public class ObjectItem : ItemBase
	{
        public ulong m_field2;
		public float m_field3;
		public string m_field4;

        override public void parseAllByteArray(ByteArray bytes)
		{
            base.parseHeaderByteArray(bytes);
            parseBodyByteArray(bytes);
		}

        override public void parseBodyByteArray(ByteArray bytes)
        {
            // 移动 pos 到内容开始处
            bytes.position = m_bodyOffset;  // 从偏移处继续读取真正的内容

            // 读取内容
            m_field2 = bytes.readUnsignedLong();
            m_field3 = bytes.readFloat();
            // 客户端读取字符串方法
            m_field4 = UtilTable.readString(bytes);

            // 移动 pos 到之前读取的位置
            bytes.position = m_headerOffset + 8;
        }

        override public void parseByteArrayTestServer(ByteArray bytes)
        {
            base.parseByteArrayTestServer(bytes);

            // 读取内容
            m_field2 = bytes.readUnsignedLong();
            m_field3 = bytes.readFloat();
            // 服务器读取字符串方法
            m_field4 = bytes.readMultiByte(256, GkEncode.UTF8);
        }
	}
}