﻿namespace FightCore
{
    public class HeroBehaviorControl : BehaviorControl
    {
        public HeroBehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {

        }

        // 是否可以发起攻击
        override public bool canLaunchAtt()
        {
            if (m_card.sceneCardItem.bSelfSide())
            {
                // 英雄卡判断伤害值和攻击次数，技能卡判断耗 Mp 和攻击次数
                return (this.m_card.sceneCardItem.svrCard.damage > 0 && this.m_card.sceneCardItem.checkAttackTimes());
            }

            return false;
        }
    }
}