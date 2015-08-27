using Game.Msg;

namespace SDK.Lib
{
    /**
     * @brief 每一个 Hurt 以特效为结束标准，如果没有特效，就以动作为标准
     */
    public class ImmeHurtItemBase : ImmeFightItemBase
    {
        public const int DAMAGE_EFFECTID = 7;       // 掉血特效

        protected EImmeHurtType m_hurtType;
        protected EImmeHurtExecState m_execState;
        protected EventDispatch m_hurtExecEndDisp;  // Hurt Item 执行结束事件分发

        public ImmeHurtItemBase(EImmeHurtType hurtType)
        {
            m_hurtType = hurtType;
            m_execState = EImmeHurtExecState.eNone;
            m_hurtExecEndDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public EImmeHurtType hurtType
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

        public EImmeHurtExecState execState
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
            m_execState = EImmeHurtExecState.eStartExec;
        }

        virtual public void execHurt(BeingEntity being)
        {
            m_execState = EImmeHurtExecState.eExecing;
        }

        // 这个是整个受伤执行结束
        virtual public void onHurtExecEnd(IDispatchObject dispObj)
        {
            m_execState = EImmeHurtExecState.eEnd;
            m_hurtExecEndDisp.dispatchEvent(this);
        }

        override public void initItemData(BeingEntity att, BeingEntity def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_being = def;
        }
    }
}