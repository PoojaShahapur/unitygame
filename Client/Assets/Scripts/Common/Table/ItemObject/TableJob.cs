namespace SDK.Common
{
    /**
     * @brief 职业表
     */
    public class TableJobItemBody : TableItemBodyBase
    {
        public string m_jobName;                // 职业名称
        public string m_jobDesc;                // 职业描述
        public int m_jobRes;                    // 职业资源
        public int m_cardSetRes;             // 卡组资源
        public string m_skillName;                // 技能名称
        public string m_skillDesc;                // 技能描述
        public int m_skillRes;                // 技能图标资源
        

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref m_jobName);
            UtilTable.readString(bytes, ref m_jobDesc);
            bytes.readInt32(ref m_jobRes);
            bytes.readInt32(ref m_cardSetRes);
            UtilTable.readString(bytes, ref m_skillName);
            UtilTable.readString(bytes, ref m_skillDesc);
            bytes.readInt32(ref m_skillRes);
        }
    }
}