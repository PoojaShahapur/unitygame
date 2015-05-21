namespace SDK.Common
{
    /**
     * @brief 职业表
     */
    public class TableJobItemBody : TableItemBodyBase
    {
        public string m_jobName;                // 职业名称
        public string m_jobDesc;                // 职业描述
        public string m_frameImage;             // 门派底图资源(这个是场景卡牌需要的资源)
        public string m_yaoDaiImage;            // 卡牌名字腰带资源(这个是场景卡牌需要的资源)
        public string m_jobRes;                 // 门派选择资源(门派名字资源是这个资源名字加上 __name 组成，例如这个名字是 aaa ，那么名字的资源名字就是 aaa_name)
        public string m_cardSetRes;             // 门派卡组资源
        public string m_skillName;              // 技能名称
        public string m_skillDesc;              // 技能描述
        public string m_skillRes;               // 技能图标资源

        public string m_jobNameRes;             // 这个字段表中没有配置

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref m_jobName);
            UtilTable.readString(bytes, ref m_jobDesc);
            UtilTable.readString(bytes, ref m_frameImage);
            UtilTable.readString(bytes, ref m_yaoDaiImage);

            UtilTable.readString(bytes, ref m_jobRes);
            UtilTable.readString(bytes, ref m_cardSetRes);
            UtilTable.readString(bytes, ref m_skillName);
            UtilTable.readString(bytes, ref m_skillDesc);
            UtilTable.readString(bytes, ref m_skillRes);

            if(string.IsNullOrEmpty(m_jobRes))
            {
                m_jobNameRes = "emei_zhiye";
            }
            else
            {
                m_jobNameRes = string.Format("{0}_name", m_jobRes);
            }
        }
    }
}