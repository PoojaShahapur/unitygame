﻿using SDK.Common;
using SDK.Lib;
using System;

namespace FightCore
{
    /**
     * @brief 已经出手的卡牌列表
     */
    public class OutSceneCardList : SceneCardListBase
    {
        protected SceneCardBase m_whiteCard;            // 这个卡牌就是当要出手的时候，就加入列表中，好计算位置

        public OutSceneCardList(SceneDZData data, EnDZPlayer playerSide)
            : base(data, playerSide)
        {
            m_whiteCard = Ctx.m_instance.m_sceneCardMgr.createCardById(SceneDZCV.WHITE_CARDID, playerSide, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
            Ctx.m_instance.m_sceneCardMgr.delObject(m_whiteCard);         // 白色卡牌就不加入列表中了
            m_whiteCard.gameObject().SetActive(false);
        }

        override public void dispose()
        {
            m_whiteCard.dispose();
            m_whiteCard = null;
        }

        protected override void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.newRectSplit(m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_COMMON].transform, SceneDZCV.COMMON_CARD_WIDTH, m_sceneDZData.m_placeHolderGo.m_cardCommonAreaWidthArr[(int)m_playerSide], 0, m_sceneCardList.Count(), ref m_posList);
        }

        override public void addCard(SceneCardBase card, int idx = -1)
        {
            base.addCard(card, idx);

            #if !DEBUG_NOTNET
            card.sceneCardItem.cardArea = CardArea.CARDCELLTYPE_COMMON;
            #endif

            // 添加进来的卡牌是不能移动的
            card.ioControl.disableDrag();
            card.updateOutCardScaleInfo(m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_COMMON].transform);    // 缩放按照配置运行
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

        override public void updateSceneCardPos(bool bUpdateIdx = true)
        {
            updateSceneCardPosInternal(CardArea.CARDCELLTYPE_COMMON);
        }

        public void updateStateEffect()
        {
            foreach(var _card in m_sceneCardList.list)
            {
                _card.updateStateEffect();
            }
        }
    }
}