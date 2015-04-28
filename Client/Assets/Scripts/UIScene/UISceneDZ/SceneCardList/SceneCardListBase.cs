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
                    if (item.sceneCardItem.m_svrCard.qwThisID == thisid)
                    {
                        return item;
                    }
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("error");
                }
            }

            return null;
        }

        public void updateCardData(SceneCardItem sceneItem)
        {
            foreach (SceneDragCard item in m_sceneCardList)
            {
                if (item.sceneCardItem.m_svrCard.qwThisID == sceneItem.m_svrCard.qwThisID)
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
                if (UtilApi.isAddressEqual(item.getGameObject(), underGo))
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
                item.m_index = idx;

                ++idx;
            }
        }

        public void updateCardGreenFrame(bool benable)
        {
            foreach (SceneDragCard cardItem in m_sceneCardList)
            {
                cardItem.updateCardGreenFrame(benable);
            }
        }

        public void updateCardGreenFrameByCond(EnGameOp gameOpt, Func<SceneCardEntityBase, EnGameOp, bool> func)
        {
            foreach (SceneCardEntityBase cardItem in m_sceneCardList)
            {
                if (func(cardItem, gameOpt))
                {
                    cardItem.updateCardGreenFrame(true);
                }
            }
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

        public int findCardIdx(SceneCardEntityBase card)
        {
            return m_sceneCardList.IndexOf(card as SceneDragCard);
        }
    }
}