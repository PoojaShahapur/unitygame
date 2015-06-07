using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 每一个 Hurt 以特效为结束标准，如果没有特效，就以动作为标准
     */
    public class HurtItemBase : FightItemBase
    {
        public const int DAMAGE_EFFECTID = 7;

        protected EHurtType m_hurtType;
        protected EHurtExecState m_execState;
        protected EventDispatch m_hurtExecEndDisp;  // Hurt Item 执行结束事件分发

        protected bool m_bDamage;       // 是否是伤血
        protected bool m_bAddHp;        // 是否有回血

        public byte[] m_state;          // 记录状态改变

        public HurtItemBase(EHurtType hurtType)
        {
            m_hurtType = hurtType;
            m_execState = EHurtExecState.eNone;
            m_hurtExecEndDisp = new AddOnceAndCallOnceEventDispatch();

            m_state = new byte[((int)StateID.CARD_STATE_MAX + 7) / 8];
        }

        public EHurtType hurtType
        {
            get
            {
                return m_hurtType;
            }
            set
            {
                m_hurtType = value;
            }
        }

        public EHurtExecState execState
        {
            get
            {
                return m_execState;
            }
            set
            {
                m_execState = value;
            }
        }

        public EventDispatch hurtExecEndDisp
        {
            get
            {
                return m_hurtExecEndDisp;
            }
            set
            {
                m_hurtExecEndDisp = value;
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

        public byte[] state
        {
            get
            {
                return m_state;
            }
        }

        public override void onTime(float delta)
        {
            base.onTime(delta);
        }

        virtual public void execHurt(SceneCardBase card)
        {

        }

        // 这个是整个受伤执行结束
        public void onHurtExecEnd(IDispatchObject dispObj)
        {
            m_execState = EHurtExecState.eEnd;
            m_hurtExecEndDisp.dispatchEvent(this);
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_svrCard = def.sceneCardItem.svrCard;  // 保存这次被击的属性，可能这个会被后面的给改掉

            if (att.sceneCardItem.svrCard.damage > 0)
            {
                m_damage = att.sceneCardItem.svrCard.damage;
            }

            m_bDamage = hasDamageHp(msg.m_origDefObject, def.sceneCardItem.svrCard);
            m_bAddHp = hasAddHp(msg.m_origDefObject, def.sceneCardItem.svrCard);

            int idx = 0;
            bool srcState = false;
            bool destState = false;
            for(idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
            {
                srcState = UtilMath.checkState((StateID)idx, msg.m_origDefObject.state);
                destState = UtilMath.checkState((StateID)idx, def.sceneCardItem.svrCard.state);

                if(srcState != destState)
                {
                    UtilMath.checkState((StateID)idx, m_state);     // 设置状态变化标志
                }
            }
        }

        // 计算是否是伤血
        protected bool hasDamageHp(t_Card src, t_Card dest)
        {
            if (src.hp > dest.hp)        // HP 减少
            {
                if (src.hp != src.maxhp)         // 不是由于技能导致的将这两个值减少并且设置成同样的值，就是伤血
                {
                    return true;
                }
            }

            return false;
        }

        protected bool hasAddHp(t_Card src, t_Card dest)
        {
            if (src.hp > dest.hp)        // HP 减少
            {
                if (src.hp != src.maxhp)         // 不是由于技能导致的将这两个值减少并且设置成同样的值，就是伤血
                {
                    return true;
                }
            }

            return false;
        }

        public bool bStateChange()
        {
            int idx = 0;
            for(idx = 0; idx < (int)StateID.CARD_STATE_MAX; ++idx)
            {
                if(UtilMath.checkState((StateID)idx, m_state))
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
    }
}