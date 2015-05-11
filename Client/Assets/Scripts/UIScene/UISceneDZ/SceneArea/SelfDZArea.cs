using SDK.Common;

namespace Game.UI
{
    public class SelfDZArea : SceneDZArea
    {
        public SelfDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
            : base(sceneDZData, playerFlag)
        {
            m_centerHero.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfHero));
            m_inSceneCardList = new SelfInSceneCardList(m_sceneDZData, m_playerFlag);
        }

        // 除了 card 禁止所有手牌区域卡牌拖动，目前只有自己区域才能做这个功能
        override public void disableAllInCardDragExceptOne(SceneDragCard card)
        {
            m_inSceneCardList.disableAllCardDragExceptOne(card);
        }

        override public void enableAllInCardDragExceptOne(SceneDragCard card)
        {
            m_inSceneCardList.enableAllCardDragExceptOne(card);
        }
    }
}