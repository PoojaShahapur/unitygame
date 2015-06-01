namespace Game.UI
{
    /**
     * @brief 基本的战斗中的一项数据
     */
    public class FightItemBase
    {
        protected float m_delayTime;            // 延迟处理的时间

        public FightItemBase()
        {
            m_delayTime = 0;
        }

        public float delayTime
        {
            get
            {
                return m_delayTime;
            }
            set
            {
                m_delayTime = value;
            }
        }

        virtual public void onTime(float delta)
        {
            if (m_delayTime > 0)
            {
                m_delayTime -= delta;
            }
        }

        // 是否可以处理当前战斗项
        public bool canHandle()
        {
            return m_delayTime <= 0;
        }
    }
}