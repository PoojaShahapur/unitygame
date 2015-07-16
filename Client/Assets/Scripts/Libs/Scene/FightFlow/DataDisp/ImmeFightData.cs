namespace SDK.Lib
{
    /**
     * @brief 战斗中用到的数据
     */
    public class ImmeFightData
    {
        protected ImmeAttackData m_attackData;
        protected ImmeHurtData m_hurtData;

        public ImmeFightData()
        {
            m_attackData = new ImmeAttackData();
            m_hurtData = new ImmeHurtData();
        }

        public ImmeAttackData attackData
        {
            get
            {
                return m_attackData;
            }
        }

        public ImmeHurtData hurtData
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