namespace SDK.Common
{
    /**
     * @brief 技能基本表
     * // 添加一个表的步骤一
     */
    public class TableSkillItemBody : TableItemBodyBase
    {
        public string m_name;               // 名称
        public string m_effect;             // 效果
        public string m_desc;               // 说明

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            (bytes as ByteBuffer).position = offset;
            m_name = UtilTable.readString(bytes as ByteBuffer);
            m_effect = UtilTable.readString(bytes as ByteBuffer);
            m_desc = UtilTable.readString(bytes as ByteBuffer);
        }
    }
}