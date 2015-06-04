﻿using Game.Msg;
using SDK.Lib;

namespace Game.UI
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

        public HurtItemBase()
        {
            m_hurtType = EHurtType.eCommon;
            m_execState = EHurtExecState.eNone;
            m_hurtExecEndDisp = new AddOnceAndCallOnceEventDispatch();
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
        }
    }
}