namespace SDK.Common
{
    public class TableStateItemBody : TableItemBodyBase
    {
        public string m_name;           // 名称
        public string m_res;            // 资源
        public int m_effectId;          // 特效 Id

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref m_name);
            UtilTable.readString(bytes, ref m_res);
            bytes.readInt32(ref m_effectId);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if(m_effectId == 0)
            {
                m_effectId = 4;
            }
        }
    }
}