using Game.Msg;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 每一个 Hurt 以特效为结束标准，如果没有特效，就以动作为标准
     */
    public class HurtItemBase : FightItemBase
    {
        public const int DAMAGE_EFFECTID = 7;       // 掉血特效

        protected EHurtType m_hurtType;
        protected EHurtExecState m_execState;
        protected EventDispatch m_hurtExecEndDisp;  // Hurt Item 执行结束事件分发

        public HurtItemBase(EHurtType hurtType)
        {
            m_hurtType = hurtType;
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

        virtual public void startHurt()
        {
            m_execState = EHurtExecState.eStartExec;
        }

        virtual public void execHurt(SceneCardBase card)
        {
            m_execState = EHurtExecState.eExecing;
        }

        // 这个是整个受伤执行结束
        virtual public void onHurtExecEnd(IDispatchObject dispObj)
        {
            m_execState = EHurtExecState.eEnd;
            m_hurtExecEndDisp.dispatchEvent(this);
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_card = def;
            m_svrCard = def.sceneCardItem.svrCard;  // 保存这次被击的属性，可能这个会被后面的给改掉

            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 被击者被击前属性值 {0}", msg.m_origDefObject.log()));
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 被击者被击后属性值 {0}", def.sceneCardItem.svrCard.log()));
            
            // 回血统一接口
            m_bAddHp = hasAddHp(msg.m_origDefObject, def.sceneCardItem.svrCard);
            m_addHp = (int)(def.sceneCardItem.svrCard.hp - msg.m_origDefObject.hp);

            if(m_bAddHp)
            {
                Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 被击者加血 {0}", m_addHp));
            }

            updateStateChange(msg.m_origDefObject.state, def.sceneCardItem.svrCard.state);
        }
    }
}