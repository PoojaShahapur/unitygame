using SDK.Common;
using SDK.Lib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 卡牌列表基类
     */
    public class SceneCardListBase
    {
        public SceneDZData m_sceneDZData;
        public EnDZPlayer m_playerSide;                 // 指示玩家的位置

        protected Transform m_centerPos;    // 中心点
        protected float m_radius;           // 半径

        protected List<Vector3> m_posList = new List<Vector3>();
        //protected List<Quaternion> m_rotList = new List<Quaternion>();

        protected MList<SceneCardBase> m_sceneCardList = new MList<SceneCardBase>();

        public SceneCardListBase(SceneDZData data, EnDZPlayer playerSide)
        {
            m_sceneDZData = data;
            m_playerSide = playerSide;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        public Transform centerPos
        {
            get
            {
                return m_centerPos;
            }
            set
            {
                m_centerPos = value;
            }
        }

        public float radius
        {
            get
            {
                return m_radius;
            }
            set
            {
                m_radius = value;
            }
        }

        protected virtual void getCardPos()
        {

        }

        // 这个返回的可能会有一张白色占位卡牌，可能不准确
        virtual public int getCardCount()
        {
            return m_sceneCardList.Count();
        }

        //// 更新场景卡牌位置，更新缩放和位移，这个只有在卡牌被拖动出去然后退回来的时候才会调用，因为拖放出去后，卡牌会变大，因此退回来需要缩放，战吼的时候不更新索引，需要等服务器返回来才更新索引
        //public virtual void updateSceneCardST(bool bUpdateIdx = true)
        //{
        //    int idx = 0;
        //    SceneCardBase cardItem;
        //    WayPtItem pt;

        //    getCardPos();

        //    while (idx < m_sceneCardList.Count())
        //    {
        //        cardItem = m_sceneCardList[idx];
        //        if (bUpdateIdx)
        //        {
        //            cardItem.curIndex = (ushort)idx;
        //        }
        //        //cardItem.trackAniControl.destPos = m_posList[idx];
        //        //cardItem.trackAniControl.destRot = m_rotList[idx].eulerAngles;
        //        //cardItem.trackAniControl.destScale = SceneCardBase.SMALLFACT;
        //        pt = cardItem.trackAniControl.wayPtList.getAndAddPosInfo(PosType.eHandDown);    // 缩放就是 PosType.eHandDown 保存的值
        //        pt.pos = m_posList[idx];
        //        cardItem.trackAniControl.moveToDestST();

        //        ++idx;
        //    }
        //}

        // 战吼的时候不更新索引，需要等服务器返回来才更新索引
        virtual public void updateSceneCardPos(bool bUpdateIdx = true)
        {

        }

        virtual protected void updateSceneCardPosInternal(CardArea area, bool bUpdateIdx = true)
        {
            int idx = 0;
            SceneCardBase cardItem = null;
            WayPtItem pt = null;
            PosType posType = PosType.eHandDown;
            if (CardArea.CARDCELLTYPE_HAND == area)
            {
                posType = PosType.eHandDown;
            }
            else if (CardArea.CARDCELLTYPE_COMMON == area)
            {
                posType = PosType.eOutDown;
            }

            getCardPos();

            while (idx < m_sceneCardList.Count())
            {
                cardItem = m_sceneCardList[idx];
                if (bUpdateIdx)
                {
                    cardItem.curIndex = (ushort)idx;
                }

                pt = cardItem.trackAniControl.wayPtList.getAndAddPosInfo(posType);
                pt.pos = m_posList[idx];
                cardItem.trackAniControl.moveToDestPos(posType);

                ++idx;
            }
        }

        public void setCardDataByIdx(int idx, SceneCardItem sceneItem)
        {
            if (idx < m_sceneCardList.Count())       // 这个地方有时候会超出范围
            {
                m_sceneCardList[idx].sceneCardItem = sceneItem;
            }
            else
            {
                Ctx.m_instance.m_logSys.error("列表超出范围");
            }
        }

        public SceneCardBase getSceneCardByThisID(uint thisid)
        {
            foreach (SceneCardBase item in m_sceneCardList.list)
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
            foreach (SceneCardBase item in m_sceneCardList.list)
            {
                if (item.sceneCardItem.svrCard.qwThisID == sceneItem.svrCard.qwThisID)
                {
                    item.updateCardDataChangeBySvr();
                    break;
                }
            }
        }

        public SceneCardBase getUnderSceneCard(GameObject underGo)
        {
            foreach(SceneCardBase item in m_sceneCardList.list)
            {
                if (UtilApi.isAddressEqual(item.gameObject(), underGo))
                {
                    return item;
                }
            }

            return null;
        }

        public SceneCardBase removeNoDestroyAndRet(int idx = 0)
        {
            SceneCardBase card = null;
            if(idx < m_sceneCardList.Count())
            {
                card = m_sceneCardList[idx];
                //m_sceneCardList.RemoveAt(idx);
                removeCard(card);
            }

            return card;
        }

        public SceneCardBase getCardByIdx(int idx = 0)
        {
            SceneCardBase card = null;
            if (idx < m_sceneCardList.Count())
            {
                card = m_sceneCardList[idx];
            }

            return card;
        }

        // 更新卡牌索引
        public void updateCardIndex()
        {
            ushort idx = 0;

            foreach (SceneCardBase item in m_sceneCardList.list)
            {
                item.curIndex = idx;
                ++idx;
            }
        }

        public void updateCardOutState(bool benable)
        {
            foreach (SceneCardBase cardItem in m_sceneCardList.list)
            {
                cardItem.updateCardOutState(benable);
            }
        }

        // 更新被击状态
        public void updateCardAttackedState(GameOpState opt)
        {
            foreach (SceneCardBase cardItem in m_sceneCardList.list)
            {
                cardItem.updateCardAttackedState(opt);
            }
        }

        // 通过客户端的卡牌数据添加卡牌
        virtual public void addCard(SceneCardBase card, int idx = -1)
        {
            if (idx == -1)
            {
                m_sceneCardList.Add(card);
            }
            else
            {
                m_sceneCardList.Insert(idx, card);
            }
        }

        virtual public void removeCard(SceneCardBase card)
        {
            m_sceneCardList.Remove(card);
        }

        public bool Contains(SceneCardBase card)
        {
            return m_sceneCardList.Contains(card);
        }

        public bool ContainsByItem(SceneCardItem sceneCardItem)
        {
            bool bRet = false;
            int idx = 0;
            while (idx < m_sceneCardList.Count())
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    bRet = true;
                    break;
                }
                ++idx;
            }

            return bRet;
        }

        public SceneCardBase findCardIByItem(SceneCardItem sceneCardItem)
        {
            SceneCardBase retCard = null;
            int idx = 0;
            while (idx < m_sceneCardList.Count())
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    retCard = m_sceneCardList[idx];
                    break;
                }
                ++idx;
            }

            return retCard;
        }

        // 通过服务器数据移除一张卡牌，并不释放
        public bool removeCardIByItem(SceneCardItem sceneCardItem)
        {
            bool bRet = false;
            int idx = 0;
            while (idx < m_sceneCardList.Count())
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    //m_sceneCardList.RemoveAt(idx);
                    removeCard(m_sceneCardList[idx]);
                    bRet = true;
                    break;
                }
                ++idx;
            }

            return bRet;
        }

        public bool removeAndDestroyCardByItem(SceneCardItem sceneCardItem)
        {
            bool bRet = false;
            int idx = 0;
            while (idx < m_sceneCardList.Count())
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    //Ctx.m_instance.m_sceneCardMgr.removeAndDestroy(m_sceneCardList[idx]);
                    //m_sceneCardList.RemoveAt(idx);
                    m_sceneCardList[idx].dispose();
                    bRet = true;
                    break;
                }
                ++idx;
            }

            return bRet;
        }

        public SceneCardBase removeAndRetCardByItem(SceneCardItem sceneCardItem)
        {
            SceneCardBase retCard = null;
            int idx = 0;
            while (idx < m_sceneCardList.Count())
            {
                if (m_sceneCardList[idx].sceneCardItem.svrCard.qwThisID == sceneCardItem.svrCard.qwThisID)
                {
                    retCard = m_sceneCardList[idx];
                    //m_sceneCardList.RemoveAt(idx);
                    removeCard(retCard);
                    break;
                }
                ++idx;
            }

            return retCard;
        }

        public int findCardIdx(SceneCardBase card)
        {
            return m_sceneCardList.IndexOf(card as SceneCardBase);
        }

        // 根据服务器索引添加一个卡牌，不是根据卡牌列表索引
        public void addCardByServerPos(SceneCardBase card)
        {
            int idx = 0;
            // 检查是否是最后一个
            if (0 == m_sceneCardList.Count())         // 如果列表中没有，直接加入
            {
                m_sceneCardList.Add(card as SceneCardBase);
            }
            else if(m_sceneCardList[m_sceneCardList.Count() - 1].curIndex < card.curIndex)    // 如果是最后一个
            {
                m_sceneCardList.Add(card as SceneCardBase);
            }
            else
            {
                foreach (var cardItem in m_sceneCardList.list)
                {
                    if (cardItem.curIndex > card.curIndex)
                    {
                        m_sceneCardList.Insert(idx, card as SceneCardBase);
                        break;
                    }

                    ++idx;
                }
            }
        }

        virtual public void disableAllCardDragExceptOne(SceneCardBase card)
        {

        }

        virtual public void enableAllCardDragExceptOne(SceneCardBase card)
        {

        }
    }
}