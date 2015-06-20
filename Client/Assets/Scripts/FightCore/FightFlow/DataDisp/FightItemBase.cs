using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 基本的战斗中的一项数据
     */
    public class FightItemBase : IDispatchObject
    {
        protected float m_delayTime;            // 延迟处理的时间
        protected int m_damage;                 // 造成的伤害，需要显示伤害数字
        protected t_Card m_svrCard;
        protected SceneCardBase m_card;         // 场景中的一个卡牌

        public FightItemBase()
        {
            m_delayTime = 0;
        }

        public t_Card svrCard
        {
            get
            {
                return m_svrCard;
            }
            set
            {
                m_svrCard = value;
            }
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

        public int damage
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

        // 初始化数据
        virtual public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {

        }

        virtual public void initDieItemData(SceneCardBase dieCard, stRetRemoveBattleCardUserCmd msg)
        {

        }
    }
}