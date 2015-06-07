using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
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

        // 替换初始卡牌
        public virtual void replaceInitCard()
        {

        }

        public virtual void startCardMoveTo()
        {
            updateSceneCardRST();
        }

        protected override void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.splitPos((int)m_playerFlag, m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_HAND].transform, m_smallInternal, m_radius, m_sceneCardList.Count(), ref m_posList, ref m_rotList);
        }

        public virtual void addCard(SceneCardBase card)
        {
            m_sceneCardList.Add(card);
        }

        public void removeCard(SceneCardBase card)
        {
            // 移除监听器
            // card.m_moveDisp = null;
            card.dragControl.disableDrag();
            m_sceneCardList.Remove(m_sceneDZData.m_curDragItem);
        }

        public void addSceneCard(uint objid, SceneCardItem sceneItem)
        {
            SceneCardBase tmpcard;
            tmpcard = Ctx.m_instance.m_sceneCardMgr.createCard(objid, m_playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
            tmpcard.sceneCardItem = sceneItem;
            addCard(tmpcard);
            updateSceneCardRST();
            updateCardIndex();
        }

        // 清空卡牌列表
        public void clearSceneCardList()
        {
            foreach(SceneCardBase card in m_sceneCardList.list)
            {
                UtilApi.Destroy(card.gameObject());
            }

            m_sceneCardList.Clear();
        }

        // 是否还有剩余的点数可以使用
        public bool hasLeftMagicPtCanUse()
        {
            foreach (SceneCardBase card in m_sceneCardList.list)
            {
                if(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_heroMagicPoint.mp >= card.sceneCardItem.svrCard.mpcost)
                {
                    return true;
                }
            }

            return false;
        }
    }
}