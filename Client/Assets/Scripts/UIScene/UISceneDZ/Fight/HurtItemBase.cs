using SDK.Lib;
namespace Game.UI
{
    public class HurtItemBase : FightItemBase
    {
        protected EHurtType m_hurtType;
        protected EHurtItemState m_state;
        protected EHurtExecState m_execState;

        public HurtItemBase()
        {
            m_hurtType = EHurtType.eCommon;
            m_state = EHurtItemState.eDisable;
            m_execState = EHurtExecState.eNone;
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

        public EHurtItemState state
        {
            get
            {
                return m_state;
            }
            set
            {
                m_state = value;
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

        public override void onTime(float delta)
        {
            if (EHurtItemState.eEnable == state)
            {
                base.onTime(delta);
            }
        }

        public void onAttackItemEnd(IDispatchObject dispObj)
        {
            m_state = EHurtItemState.eEnable;
        }

        virtual public void execHurt(SceneCardBase card)
        {

        }

        // 这个是整个受伤执行结束
        public void onHuerExecEnd(IDispatchObject dispObj)
        {
            m_execState = EHurtExecState.eEnd;
        }
    }
}