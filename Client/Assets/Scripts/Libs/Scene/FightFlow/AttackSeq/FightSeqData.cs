namespace SDK.Lib
{
    /**
     * @brief 战斗序列数据
     */
    public class FightSeqData
    {
        protected AttackSeqData m_attackSeqData;
        protected HurtSeqData m_hurtSeqData;

        public FightSeqData()
        {
            m_attackSeqData = new AttackSeqData();
            m_hurtSeqData = new HurtSeqData();
        }

        public AttackSeqData attackSeqData
        {
            get
            {
                return m_attackSeqData;
            }
        }

        public HurtSeqData hurtSeqData
        {
            get
            {
                return m_hurtSeqData;
            }
        }

        public void onTime(float delta)
        {
            m_attackSeqData.onTime(delta);
            m_hurtSeqData.onTime(delta);
        }
    }
}