using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    public class AttackItemBase : FightItemBase
    {
        protected EAttackType m_attackType;
        protected EAttackRangeType m_attackRangeType;
        protected EventDispatch m_attackEndPlayDisp;      // 攻击结束播放分发

        public AttackItemBase()
        {
            m_attackEndPlayDisp = new AddOnceAndCallOnceEventDispatch();
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

        public EventDispatch attackEndPlayDisp
        {
            get
            {
                return m_attackEndPlayDisp;
            }
        }

        override public void dispose()
        {
            m_attackEndPlayDisp.clearEventHandle();
            m_attackEndPlayDisp = null;
            base.dispose();
        }

        virtual public uint getHurterId()
        {
            return 0;
        }

        virtual public void execAttack(SceneCardBase card)
        {
            
        }
    }
}