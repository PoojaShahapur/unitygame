using Game.Msg;
namespace Game.UI
{
    public class ComAttackItem : AttackItemBase
    {
        protected uint m_attackEffectId; // 攻击特效id
        protected uint m_hurterId;       // 被击者 thisId

        public ComAttackItem()
        {

        }

        public uint attackEffectId
        {
            get
            {
                return m_attackEffectId;
            }
            set
            {
                m_attackEffectId = value;
            }
        }

        public uint hurterId
        {
            get
            {
                return m_hurterId;
            }
            set
            {
                m_hurterId = value;
            }
        }

        override public uint getHurterId()
        {
            return m_hurterId;
        }

        override public void execAttack(SceneCardBase card)
        {
            card.behaviorControl.execAttack(this);
        }

        override public void initItemData(SceneCardBase att, SceneCardBase def, stNotifyBattleCardPropertyUserCmd msg)
        {
            base.initItemData(att, def, msg);

            m_hurterId = def.sceneCardItem.svrCard.qwThisID;
            m_attackEffectId = 0; // 普通攻击没有攻击特效
        }
    }
}