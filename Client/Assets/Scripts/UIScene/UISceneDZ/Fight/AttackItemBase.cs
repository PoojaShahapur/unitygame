using Game.Msg;
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

            m_svrCard = att.sceneCardItem.svrCard;  // 保存这次攻击的属性，可能这个会被后面的给改掉

            // 播放 Fly 数字，攻击者和被击者都有可能伤血，播放掉血数字
            // 攻击者掉血
            if (def.sceneCardItem.svrCard.damage > 0)        // 攻击力可能为 0 
            {
                m_damage = def.sceneCardItem.svrCard.damage;
            }
        }
    }
}