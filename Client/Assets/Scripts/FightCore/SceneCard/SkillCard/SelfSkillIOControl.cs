using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    public class SelfSkillIOControl : SkillIOControl
    {
        public SelfSkillIOControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        override public void onCardClick(IDispatchObject dispObj)
        {
            if ((int)CardType.CARDTYPE_SKILL == m_card.sceneCardItem.m_cardTableItem.m_type)     // 如果点击的是技能卡牌
            {
                if (m_card.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget == 0)         // 如果没有攻击目标
                {
                    // 直接发送消息
                    stCardMoveAndAttackMagicUserCmd skillCmd = new stCardMoveAndAttackMagicUserCmd();
                    skillCmd.dwAttThisID = m_card.sceneCardItem.svrCard.qwThisID;
                    UtilMsg.sendMsg(skillCmd);
                }
                else        // 有攻击目标
                {
                    m_card.m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpSkillAttackTarget, m_card);
                }
            }
        }
    }
}