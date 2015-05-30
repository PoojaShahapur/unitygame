using SDK.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 卡牌列表基类
     */
    public class SceneCardListBase
    {
        public SceneDZData m_sceneDZData;
        public EnDZPlayer m_playerFlag;                 // 指示玩家的位置

        public float m_bigInternal = 1.171349f;            // 大卡牌间隔
        public float m_smallInternal = 0.5f;                 // 小卡牌间隔
        public float m_radius = 0.5f;                   // 半径

        protected List<Vector3> m_posList = new List<Vector3>();
        protected List<Quaternion> m_rotList = new List<Quaternion>();

        protected List<SceneDragCard> m_sceneCardList = new List<SceneDragCard>();

        public SceneCardListBase(SceneDZData data, EnDZPlayer playerFlag)
        {
            m_sceneDZData = data;
            m_playerFlag = playerFlag;
        }

        virtual public void dispose()
        {

        }

        //public List<SceneDragCard> sceneCardList
        //{
        //    get
        //    {
        //        return m_sceneCardList;
        //    }
        //    set
        //    {
        //        m_sceneCardList = value;
        //    }
        //}

        protected virtual void getCardPos()
        {

        }

        // 这个返回的可能会有一张白色占位卡牌，可能不准确
        virtual public int getCardCount()
        {
            return m_sceneCardList.Count;
        }

        // 更新场景卡牌位置
        public virtual void updateSceneCardRST()
        {
            int idx = 0;
            SceneDragCard cardItem;

            getCardPos();

            while (idx < m_sceneCardList.Count)
            {
                cardItem = m_sceneCardList[idx];
                cardItem.destPos = m_posList[idx];
                cardItem.destRot = m_rotList[idx].eulerAngles;
                cardItem.destScale = SceneCardEntityBase.SMALLFACT;
                cardItem.moveToDestRST();

                ++idx;
            }
        }

        public virtual void updateSceneCardPos()
        {
            int idx = 0;
            SceneDragCard cardItem;

            getCardPos();

            while (idx < m_sceneCardList.Count)
            {
                cardItem = m_sceneCardList[idx];
                cardItem.destPos = m_posList[idx];
                cardItem.moveToDestT();

                ++idx;
            }
        }

        public void setCardDataByIdx(int idx, SceneCardItem sceneItem)
        {
            if (idx < m_sceneCardList.Count)       // 这个地方有时候会超出范围
            {
                m_sceneCardList[idx].sceneCardItem = sceneItem;
            }
            else
            {
                Ctx.m_instance.m_logSys.error("列表超出范围");
            }
        }

        public SceneDragCard getSceneCardByThisID(uint thisid)
        {
            foreach (SceneDragCard item in m_sceneCardList)
            {
                if (item.sceneCardItem != null)
                {
                    if (item.sceneCardItem.svrCard.qwThisID == thisid)
                    {
                        return item;
                    }
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("根据 thisid 获取对应的卡牌出错");
                }
            }

            return null;
        }

        public void updateCardData(SceneCardItem sceneItem)
        {
            foreach (SceneDragCard item in m_sceneCardList)
            {
                if (item.sceneCardItem.svrCard.qwThisID == sceneItem.svrCard.qwThisID)
                {
                    item.updateCardDataChange();
                    break;
                }
            }
        }

        public SceneCardEntityBase getUnderSceneCard(GameObject underGo)
        {
            foreach(SceneDragCard item in m_sceneCardList)
            {
                if (UtilApi.isAddressEqual(item.gameObject, underGo))
                {
                    return item;
                }
            }

            return null;
        }

        public SceneCardEntityBase removeNoDestroyAndRet(int idx = 0)
        {
            SceneCardEntityBase card = null;
            if(idx < m_sceneCardList.Count)
            {
                card = m_sceneCardList[idx];
                m_sceneCardList.RemoveAt(idx);
            }

            return card;
        }

        public void removeCardNoDestroy(SceneCardEntityBase card)
        {
            m_sceneCardList.Remove(card as SceneDragCard);
        }

        public SceneCardEntityBase getCardByIdx(int idx = 0)
        {
            SceneCardEntityBase card = null;
            if (idx < m_sceneCardList.Count)
            {
                card = m_sceneCardList[idx];
            }

            return card;
        }

        // 更新卡牌索引
        public void updateCardIndex()
        {
            ushort idx = 0;

            foreach (SceneDragCard item in m_sceneCardList)
            {
                item.curIndex = idx;
                ++idx;
            }
        }

        public void updateCardOutState(bool benable)
        {
            foreach (SceneDragCard cardItem in m_sceneCardList)
            {
                cardItem.updateCardOutState(benable);
            }
        }

        public void updateCardAttackedState(GameOpState opt)
        {
            foreach (SceneCardEntityBase cardItem in m_sceneCardList)
            {
                if (opt.canAttackOp(cardItem, opt.curOp))
                {
                    cardItem.updateCardAttackedState(true);
                }
            }
        }

        public bool removeCard(SceneCardItem sceneCardItem)
        {
            bool bRet = false;
            int idx = 0;
            while (idx < m_sceneCardList.Count)
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    m_sceneCardList[idx].dispose();
                    m_sceneCardList.RemoveAt(idx);
                    bRet = true;
                    break;
                }
                ++idx;
            }

            return bRet;
        }

        public SceneCardEntityBase removeAndRetCardByItemNoDestroy(SceneCardItem sceneCardItem)
        {
            SceneCardEntityBase retCard = null;
            int idx = 0;
            while (idx < m_sceneCardList.Count)
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    retCard = m_sceneCardList[idx];
                    m_sceneCardList.RemoveAt(idx);
                    break;
                }
                ++idx;
            }

            return retCard;
        }

        public int findCardIdx(SceneCardEntityBase card)
        {
            return m_sceneCardList.IndexOf(card as SceneDragCard);
        }

        // 根据服务器索引添加一个卡牌，不是根据卡牌列表索引
        public void addCardByServerPos(SceneCardEntityBase card)
        {
            int idx = 0;
            // 检查是否是最后一个
            if (0 == m_sceneCardList.Count)         // 如果列表中没有，直接加入
            {
                m_sceneCardList.Add(card as SceneDragCard);
            }
            else if(m_sceneCardList[m_sceneCardList.Count - 1].curIndex < card.curIndex)    // 如果是最后一个
            {
                m_sceneCardList.Add(card as SceneDragCard);
            }
            else
            {
                foreach (var cardItem in m_sceneCardList)
                {
                    if (cardItem.curIndex > card.curIndex)
                    {
                        m_sceneCardList.Insert(idx, card as SceneDragCard);
                        break;
                    }

                    ++idx;
                }
            }
        }

        virtual public void disableAllCardDragExceptOne(SceneDragCard card)
        {

        }

        virtual public void enableAllCardDragExceptOne(SceneDragCard card)
        {

        }
    }
}