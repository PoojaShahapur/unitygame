using SDK.Common;
using UnityEngine;
namespace Game.UI
{
    /**
     * @brief 手里的卡牌列表
     */
    public class InSceneCardList : SceneCardListBase
    {
        public InSceneCardList(SceneDZData data, EnDZPlayer playerFlag)
            : base(data, playerFlag)
        {

        }

        // 对战开始显示的卡牌
        public virtual void addInitCard()
        {

        }

        public void startCardMoveTo()
        {
            updateSceneCardPos();
        }

        protected override void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.splitPos((int)m_playerFlag, m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_HAND].transform, m_internal, m_radius, m_sceneCardList.Count, ref m_posList, ref m_rotList);
        }

        public virtual void addCard(SceneDragCard card)
        {
            m_sceneCardList.Add(card);
        }

        public void removeCard(SceneDragCard card)
        {
            // 移除监听器
            card.m_moveDisp = null;
            m_sceneCardList.Remove(m_sceneDZData.m_curDragItem);
        }

        public void addSceneCard(uint objid, SceneCardItem sceneItem)
        {
            SceneDragCard tmpcard;
            tmpcard = m_sceneDZData.createOneCard(objid, m_playerFlag, CardArea.CARDCELLTYPE_HAND);
            tmpcard.sceneCardItem = sceneItem;
            addCard(tmpcard);
            updateSceneCardPos();
        }
    }
}