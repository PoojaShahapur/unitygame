using SDK.Common;

namespace Game.UI
{
    public class AttackItemBase : FightItemBase
    {
        protected EAttackType m_attackType;
        protected EAttackRangeType m_attackRangeType;
        protected MList<HurtItemBase> m_hurtList;

        public AttackItemBase()
        {
            m_hurtList = new MList<HurtItemBase>();
        }

        public EAttackType attackType
        {
            get
            {
                return m_attackType;
            }
            set
            {
                m_attackType = value;
            }
        }

        public EAttackRangeType attackRangeType
        {
            get
            {
                return m_attackRangeType;
            }
            set
            {
                m_attackRangeType = value;
            }
        }

        virtual public uint getHurterId()
        {
            return 0;
        }

        public void setEnableHurt()
        {
            foreach(HurtItemBase item in m_hurtList.list)
            {
                item.state = EHurtItemState.eEnable;
            }

            m_hurtList.Clear();
        }
    }
}