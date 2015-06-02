using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 基本的战斗中的一项数据
     */
    public class FightItemBase : IDispatchObject
    {
        protected float m_delayTime;            // 延迟处理的时间
        protected uint m_damage;                // 造成的伤害，需要显示伤害数字

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

        public uint damage
        {
            get
            {
                return m_damage;
            }
            set
            {
                m_damage = value;
            }
        }

        virtual public void onTime(float delta)
        {
            if (m_delayTime > 0)
            {
                m_delayTime -= delta;
            }
        }

        virtual public void dispose()
        {

        }

        // 是否可以处理当前战斗项
        public bool canHandle()
        {
            return m_delayTime <= 0;
        }
    }
}