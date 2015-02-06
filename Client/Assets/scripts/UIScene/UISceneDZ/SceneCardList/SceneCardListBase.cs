using SDK.Common;
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

        public float m_internal = 0.5f;            // 卡牌间隔
        public float m_radius = 0.5f;                    // 半径

        protected List<Vector3> m_posList = new List<Vector3>();
        protected List<Quaternion> m_rotList = new List<Quaternion>();

        protected List<SceneDragCard> m_sceneCardList = new List<SceneDragCard>();

        public SceneCardListBase(SceneDZData data, EnDZPlayer playerFlag)
        {
            m_sceneDZData = data;
            m_playerFlag = playerFlag;
        }

        protected virtual void getCardPos()
        {

        }

        // 更新场景卡牌位置
        public virtual void updateSceneCardPos()
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
                cardItem.moveToDest();

                ++idx;
            }
        }

        public void setCardDataByIdx(int idx, SceneCardItem sceneItem)
        {
            m_sceneCardList[idx].sceneCardItem = sceneItem;
        }

        public SceneDragCard getSceneCardByThisID(uint thisid)
        {
            foreach (SceneDragCard item in m_sceneCardList)
            {
                if (item.sceneCardItem.m_svrCard.qwThisID == thisid)
                {
                    return item;
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
    }
}