namespace SDK.Lib
{
    /**
     * @brief 技能动作配置资源
     */
    public class SkillActionRes : InsResBase
    {
        protected AttackActionSeq m_attackActionSeq;

        public SkillActionRes()
        {

        }

        override protected void initImpl(ResItem res)
        {
            string text = res.getText(GetPath());
            m_attackActionSeq = new AttackActionSeq();
            m_attackActionSeq.parseXml(text);

            base.initImpl(res);
        }

        override public void failed(ResItem res)
        {
            base.failed(res);
        }

        override public void unload()
        {
            base.unload();
        }

        public AttackActionSeq attackActionSeq
        {
            get
            {
                return m_attackActionSeq;
            }
        }
    }
}