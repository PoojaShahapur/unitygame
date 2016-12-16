namespace SDK.Lib
{
    /**
     * @brief 职业表
     */
    public class TableJobItemBody : TableItemBodyBase
    {
        public string mJobName;                // 职业名称
        public string mJobDesc;                // 职业描述
        public string mFrameImage;             // 门派底图资源(这个是场景卡牌需要的资源)
        public string mYaoDaiImage;            // 卡牌名字腰带资源(这个是场景卡牌需要的资源)
        public string mJobRes;                 // 门派选择资源(门派名字资源是这个资源名字加上 __name 组成，例如这个名字是 aaa ，那么名字的资源名字就是 aaa_name)
        public string mCardSetRes;             // 门派卡组资源
        public string mSkillName;              // 技能名称
        public string mSkillDesc;              // 技能描述
        public string mSkillRes;               // 技能图标资源

        public string mJobNameRes;             // 这个字段表中没有配置
        public string mJobBtnRes;              // 职业按钮资源

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref mJobName);
            UtilTable.readString(bytes, ref mJobDesc);
            UtilTable.readString(bytes, ref mFrameImage);
            UtilTable.readString(bytes, ref mYaoDaiImage);

            UtilTable.readString(bytes, ref mJobRes);
            UtilTable.readString(bytes, ref mCardSetRes);
            UtilTable.readString(bytes, ref mSkillName);
            UtilTable.readString(bytes, ref mSkillDesc);
            UtilTable.readString(bytes, ref mSkillRes);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if (string.IsNullOrEmpty(mFrameImage))
            {
                mFrameImage = "paidi_kapai";
            }
            if (string.IsNullOrEmpty(mYaoDaiImage))
            {
                mYaoDaiImage = "mingzidi_kapai";
            }
            if (string.IsNullOrEmpty(mCardSetRes))
            {
                mCardSetRes = "emei_taopai";
            }
            if (string.IsNullOrEmpty(mSkillRes))
            {
                mSkillRes = "emeibiao_zhiye";
            }
            if (string.IsNullOrEmpty(mJobRes))
            {
                mJobNameRes = "emei_zhiye";
                mJobBtnRes = "gaibang_paizu";
            }
            else
            {
                mJobNameRes = string.Format("{0}_name", mJobRes);
                mJobBtnRes = string.Format("{0}_btn", mJobRes);
            }
            if (string.IsNullOrEmpty(mJobRes))
            {
                mJobRes = "emei_zhiyepai";
            }
        }
    }
}