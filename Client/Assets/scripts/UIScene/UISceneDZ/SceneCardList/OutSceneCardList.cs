using SDK.Common;
namespace Game.UI
{
    /**
     * @brief 已经出手的卡牌列表
     */
    public class OutSceneCardList : SceneCardListBase
    {
        protected SceneDragCard m_whiteCard;            // 这个卡牌就是当要出手的时候，就加入列表中，好计算位置

        public OutSceneCardList(SceneDZData data, EnDZPlayer playerFlag)
            : base(data, playerFlag)
        {
            m_whiteCard = m_sceneDZData.createOneCard(SceneDragCard.WHITECARDID, playerFlag, CardArea.CARDCELLTYPE_HAND);
            m_whiteCard.getGameObject().SetActive(false);
        }

        protected override void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.rectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_COMMON].transform, m_internal, m_sceneCardList.Count, ref m_posList, ref m_rotList);
        }

        public void addCard(SceneDragCard card, int idx = 0)
        {
            #if !DEBUG_NOTNET
            card.sceneCardItem.m_cardArea = CardArea.CARDCELLTYPE_COMMON;
            #endif
            // 添加进来的卡牌是不能移动的
            card.disableDrag();

            //m_sceneCardList.Add(card);
            m_sceneCardList.Insert(idx, card);
        }

        public void removeCard(SceneCardItem sceneCardItem)
        {
            int idx = 0;
            while (idx < m_sceneCardList.Count)
            {
                if (m_sceneCardList[idx].sceneCardItem.m_svrCard.qwThisID == sceneCardItem.m_svrCard.qwThisID)
                {
                    m_sceneCardList[idx].destroy();
                    m_sceneCardList.RemoveAt(idx);
                    break;
                }
                ++idx;
            }
        }

        // 自己手里的牌移动，需要更新已经出的牌的位置
        public void onMove()
        {
            if(m_sceneDZData.m_curDragItem == null)
            {
                Ctx.m_instance.m_log.log("aaa");
                return;
            }
            // 检查插入的位置
            int idx = 0;
            while (idx < m_sceneCardList.Count)
            {
                if (m_sceneDZData.m_curDragItem.transform.localPosition.x < m_sceneCardList[idx].transform.localPosition.x)
                {
                    break;
                }
                ++idx;
            }

            if (idx != m_sceneDZData.m_curWhiteIdx)    // 如果两次位置不一样
            {
                m_sceneDZData.m_curWhiteIdx = idx;
                if (m_sceneDZData.m_curWhiteIdx < m_sceneCardList.Count)
                {
                    m_sceneCardList.Remove(m_whiteCard);
                    m_sceneCardList.Insert(m_sceneDZData.m_curWhiteIdx, m_whiteCard);
                }
                else if (m_sceneDZData.m_curWhiteIdx == m_sceneCardList.Count)        // 如果已经
                {
                    if(m_sceneCardList.IndexOf(m_whiteCard) == -1)      // 如果 m_whiteCard 还没有加进入
                    {
                        m_sceneCardList.Insert(m_sceneDZData.m_curWhiteIdx, m_whiteCard);
                    }
                    else 
                    {
                        m_sceneDZData.m_curWhiteIdx -= 1;
                        m_sceneCardList.Remove(m_whiteCard);
                        m_sceneCardList.Insert(m_sceneDZData.m_curWhiteIdx, m_whiteCard);
                    }
                }

                updateSceneCardPos();
            }
        }

        public void removeWhiteCard()
        {
            m_sceneCardList.Remove(m_whiteCard);
        }
    }
}