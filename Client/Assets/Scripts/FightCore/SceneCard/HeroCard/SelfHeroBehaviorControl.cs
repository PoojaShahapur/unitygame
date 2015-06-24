namespace FightCore
{
    public class SelfHeroBehaviorControl : HeroBehaviorControl
    {
        public SelfHeroBehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {

        }

        // 是否可以发起攻击
        override public bool canLaunchAtt()
        {
            // 英雄卡判断伤害值和攻击次数，技能卡判断耗 Mp 和攻击次数
            return (this.m_card.sceneCardItem.svrCard.damage > 0 && this.m_card.sceneCardItem.checkAttackTimes());
        }
    }
}