using Game.Msg;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 基本的战斗中的一项数据
     */
    public class FightItemBase : IDispatchObject
    {
        protected float m_delayTime;            // 延迟处理的时间
        protected bool m_bDamage;       // 是否是伤血
        protected bool m_bAddHp;        // 是否有回血
        protected int m_damage;         // 造成的伤害，需要显示伤害数字
        protected int m_addHp;          // 回血
        protected t_Card m_svrCard;
        protected SceneCardBase m_card;         // 场景中的一个卡牌

        protected byte[] m_changedState;    // 记录状态改变
        protected byte[] m_curState;        // 当前状态 

        public FightItemBase()
        {
            m_delayTime = 0;

            m_changedState = new byte[((int)StateID.CARD_STATE_MAX + 7) / 8];
            m_curState = new byte[((int)StateID.CARD_STATE_MAX + 7) / 8];
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

        public int addHp
        {
            get
            {
                return m_addHp;
            }
            set
            {
                m_addHp = value;
            }
        }

        public bool bDamage
        {
            get
            {
                return m_bDamage;
            }
            set
            {
                m_bDamage = value;
            }
        }

        public bool bAddHp
        {
            get
            {
                return m_bAddHp;
            }
            set
            {
                m_bAddHp = value;
            }
        }

        public byte[] changedState
        {
            get
            {
                return m_changedState;
            }
        }

        public byte[] curState
        {
            get
            {
                return m_curState;
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

        public bool bStateChange()
        {
            int idx = 0;
            for (idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
            {
                if (UtilMath.checkState((StateID)idx, m_changedState))
                {
                    return true;
                }
            }

            return false;
        }

        public int getStateEffect(StateID idx)
        {
            return 3;
        }

        public void updateStateChange(byte[] srcStateArr, byte[] destStateArr)
        {
            int idx = 0;
            bool srcState = false;
            bool destState = false;
            for (idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
            {
                srcState = UtilMath.checkState((StateID)idx, srcStateArr);
                destState = UtilMath.checkState((StateID)idx, destStateArr);

                if (srcState != destState)
                {
                    UtilMath.setState((StateID)idx, m_changedState);     // 设置状态变化标志
                }
                if(destState)
                {
                    UtilMath.setState((StateID)idx, m_curState);
                }
            }
        }

        // 回血判断是统一一个接口
        protected bool hasAddHp(t_Card src, t_Card dest)
        {
            if (src.hp <= dest.hp)        // HP 增加
            {
                if (src.maxhp == dest.maxhp)         // 不是由于技能导致的将这两个值减少并且设置成同样的值，就是伤血
                {
                    return true;
                }
            }

            return false;
        }
    }
}