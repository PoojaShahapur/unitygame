using SDK.Lib;
namespace Game.UI
{
    public class HurtItemBase : FightItemBase
    {
        protected EHurtType m_hurtType;
        protected EHurtItemState m_state;

        public HurtItemBase()
        {
            m_hurtType = EHurtType.eCommon;
            m_state = EHurtItemState.eDisable;
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
    }
}