using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 攻击一次只能有一个，因此攻击 Item 没有状态
     */
    public class AttackItemBase : FightItemBase
    {
        public const float ComAttMoveTime = 0.3f;  // 普通攻击的移动时间

        protected EAttackType m_attackType;
        protected EAttackRangeType m_attackRangeType;
        protected EventDispatch m_attackEndDisp;      // 整个攻击结束，从发起攻击，到回到原地

        public AttackItemBase(EAttackType attackType)
        {
            m_attackType = attackType;
            m_attackEndDisp = new AddOnceAndCallOnceEventDispatch();
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

        virtual public void execAttack(SceneCardBase card)
        {

        }

        virtual public uint getHurterId()
        {
            return 0;
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_card = att;
            m_svrCard = att.sceneCardItem.svrCard;  // 保存这次攻击的属性，可能这个会被后面的给改掉

            // 播放 Fly 数字，攻击者和被击者都有可能伤血，播放掉血数字
            // 攻击者掉血
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 攻击者掉血 {0}", def.sceneCardItem.svrCard.damage));
            if (def.sceneCardItem.svrCard.damage > 0)        // 攻击力可能为 0 
            {
                m_damage = (int)def.sceneCardItem.svrCard.damage;
            }

            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 攻击者攻击前属性值 {0}", msg.m_origAttObject.log()));
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 攻击者攻击后属性值 {0}", att.sceneCardItem.svrCard.log()));
        }

        // 获取攻击移动到目标的时间
        virtual public float getMoveTime()
        {
            return 0;
        }
    }
}