namespace SDK.Lib
{
    /**
     * @brief 战斗中用到的数据
     */
    public class FightData
    {
        protected AttackData m_attackData;
        protected HurtData m_hurtData;

        public FightData()
        {
            m_attackData = new AttackData();
            m_hurtData = new HurtData();
        }

        public AttackData attackData
        {
            get
            {
                return m_attackData;
            }
        }

        public HurtData hurtData
        {
            get
            {
                return m_hurtData;
            }
        }

        public void onTime(float delta)
        {
            m_attackData.onTime(delta);
            m_hurtData.onTime(delta);
        }
    }
}