using SDK.Common;
using SDK.Lib;
using System;
namespace Game.UI
{
    /**
     * @brief 已经出手的卡牌列表
     */
    public class OutSceneCardList : SceneCardListBase
    {
        protected SceneCardBase m_whiteCard;            // 这个卡牌就是当要出手的时候，就加入列表中，好计算位置

        public OutSceneCardList(SceneDZData data, EnDZPlayer playerFlag)
            : base(data, playerFlag)
        {
            m_whiteCard = m_sceneDZData.createOneCard(SceneCardBase.WHITECARDID, playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND) as SceneCardBase;
            m_whiteCard.gameObject().SetActive(false);
        }

        protected override void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.rectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_COMMON].transform, m_smallInternal, m_sceneCardList.Count(), ref m_posList, ref m_rotList);
        }

        public void addCard(SceneCardBase card, int idx = 0)
        {
            #if !DEBUG_NOTNET
            card.sceneCardItem.cardArea = CardArea.CARDCELLTYPE_COMMON;
            #endif
            // 添加进来的卡牌是不能移动的
            card.dragControl.disableDrag();

            //m_sceneCardList.Add(card);
            m_sceneCardList.Insert(idx, card);
        }

        // 自己手里的牌移动，需要更新已经出的牌的位置
        public void onMove()
        {
            if(m_sceneDZData.m_curDragItem == null)
            {
                Ctx.m_instance.m_logSys.log("error: move update position error");
                return;
            }
            // 检查插入的位置
            if (m_sceneCardList.Count() > 0)      // 如果有卡牌才需要进行如下处理
            {
                int idx = 0;
                while (idx < m_sceneCardList.Count())
                {
                    if (m_sceneDZData.m_curDragItem.transform().localPosition.x < m_sceneCardList[idx].transform().localPosition.x)
                    {
                        break;
                    }
                    ++idx;
                }

                //if (idx != m_sceneDZData.curWhiteIdx)    // 如果两次位置不一样
                //{
                //    m_sceneDZData.curWhiteIdx = idx;

                //    if (m_sceneCardList.IndexOf(m_whiteCard) == -1)      // 如果之前没有具体位置插入
                //    {
                //        m_sceneCardList.Insert(m_sceneDZData.curWhiteIdx, m_whiteCard);
                //        updateSceneCardPos();       // 整个都更新，只更新位置
                //    }
                //    else                // 如果之前已经有具体位置，这个是新的位置
                //    {
                //        if (m_sceneDZData.curWhiteIdx != m_sceneCardList.Count)     // 如果移动位置不是 m_sceneCardList.Count ，如果移动位置是 m_sceneCardList.Count ，说明已经移动到最右边了，因为移动位置 m_sceneCardList.Count - 1 就已经到达正确位置了
                //        {
                //            // 只更新需要移动的一个
                //            try
                //            {
                //                if (m_sceneDZData.preWhiteIdx != m_sceneCardList.Count)     // 如果前一个位置不是 m_sceneCardList.count 彩移动
                //                {
                //                    m_sceneCardList[m_sceneDZData.curWhiteIdx].destPos = m_sceneCardList[m_sceneDZData.preWhiteIdx].destPos;
                //                    m_sceneCardList[m_sceneDZData.curWhiteIdx].moveToDestT();
                //                }
                //            }
                //            catch (Exception e)
                //            {
                //                Ctx.m_instance.m_logSys.log("aaa");
                //            }

                //            m_sceneCardList.Remove(m_whiteCard);
                //            m_sceneCardList.Insert(m_sceneDZData.curWhiteIdx, m_whiteCard);
                //        }
                //    }
                //}

                if (idx != m_sceneDZData.curWhiteIdx)    // 如果两次位置不一样
                {
                    m_sceneDZData.curWhiteIdx = idx;
                    if (m_sceneDZData.curWhiteIdx < m_sceneCardList.Count())
                    {
                        m_sceneCardList.Remove(m_whiteCard);
                        m_sceneCardList.Insert(m_sceneDZData.curWhiteIdx, m_whiteCard);
                    }
                    else if (m_sceneDZData.curWhiteIdx == m_sceneCardList.Count())        // 如果已经是超出范围了
                    {
                        if (m_sceneCardList.IndexOf(m_whiteCard) == -1)      // 如果 m_whiteCard 还没有加进入
                        {
                            m_sceneCardList.Insert(m_sceneDZData.curWhiteIdx, m_whiteCard);
                        }
                        else
                        {
                            m_sceneDZData.curWhiteIdx -= 1;
                            m_sceneCardList.Remove(m_whiteCard);
                            m_sceneCardList.Insert(m_sceneDZData.curWhiteIdx, m_whiteCard);
                        }
                    }

                    updateSceneCardPos();
                }
            }
            else
            {
                m_sceneDZData.curWhiteIdx = 0;
            }
        }

        public void removeWhiteCard()
        {
            m_sceneCardList.Remove(m_whiteCard);
        }

        // 当前卡牌区域是否已经满了
        public bool bAreaCardFull()
        {
            if(getCardCount() == SceneDZCV.OUT_CARD_TOTAL)
            {
                return true;
            }

            return false;
        }

        override public int getCardCount()
        {
            int total = 0;
            foreach(var cardItem in m_sceneCardList.list)
            {
                if(!cardItem.Equals(m_whiteCard))
                {
                    ++total;
                }
            }

            return total;
        }
    }
}