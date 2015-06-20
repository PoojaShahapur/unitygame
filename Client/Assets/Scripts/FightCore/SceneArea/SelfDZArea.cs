using SDK.Common;

namespace FightCore
{
    public class SelfDZArea : SceneDZArea
    {
        public SelfDZArea(SceneDZData sceneDZData, EnDZPlayer playerSide)
            : base(sceneDZData, playerSide)
        {
            m_inSceneCardList = new SelfInSceneCardList(m_sceneDZData, m_playerSide);
        }

        // 除了 card 禁止所有手牌区域卡牌拖动，目前只有自己区域才能做这个功能
        override public void disableAllInCardDragExceptOne(SceneCardBase card)
        {
            m_inSceneCardList.disableAllCardDragExceptOne(card);
        }

        override public void enableAllInCardDragExceptOne(SceneCardBase card)
        {
            m_inSceneCardList.enableAllCardDragExceptOne(card);
        }
    }
}