using Game.Msg;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 攻击一次只能有一个，因此攻击 Item 没有状态
     */
    public class ImmeAttackItemBase : ImmeFightItemBase
    {
        public const float ComAttMoveTime = 0.3f;  // 普通攻击的移动时间

        protected EImmeAttackType m_attackType;
        protected EImmeAttackRangeType m_attackRangeType;
        protected EventDispatch m_attackEndDisp;      // 整个攻击结束，从发起攻击，到回到原地

        public ImmeAttackItemBase(EImmeAttackType attackType)
        {
            m_attackType = attackType;
            m_attackEndDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public EImmeAttackType attackType
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

        public EImmeAttackRangeType attackRangeType
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

        public EventDispatch attackEndDisp
        {
            get
            {
                return m_attackEndDisp;
            }
        }

        override public void dispose()
        {
            m_attackEndDisp.clearEventHandle();
            m_attackEndDisp = null;
            base.dispose();
        }

        virtual public void execAttack(BeingEntity being)
        {

        }

        virtual public uint getHurterId()
        {
            return 0;
        }

        override public void initItemData(BeingEntity att, BeingEntity def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_being = att;

            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 攻击者攻击前属性值 {0}", msg.m_origAttObject.log()));
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 攻击者攻击后属性值 {0}", 0));
        }

        // 获取攻击移动到目标的时间
        virtual public float getMoveTime()
        {
            return 0;
        }
    }
}