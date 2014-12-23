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
		
		override public void parseAllByteArray(IByteArray bytes)
		{
            base.parseBodyByteArray(bytes);
            parseBodyByteArray(bytes);
		}

        override public void parseBodyByteArray(IByteArray bytes)
        {
            // 移动 pos 到内容开始处
            (bytes as ByteArray).position = m_offset;  // 从偏移处继续读取真正的内容

            // 读取内容
            m_field2 = bytes.readUnsignedLong();
            m_field3 = bytes.readFloat();
            // 客户端读取字符串方法
            m_field4 = UtilTable.readString(bytes as ByteArray);

            // 移动 pos 到之前读取的位置
            (bytes as ByteArray).position = UtilTable.m_prePos;
        }

        override public void parseByteArrayTestServer(IByteArray bytes)
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