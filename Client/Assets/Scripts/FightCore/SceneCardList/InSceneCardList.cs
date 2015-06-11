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
            updateSceneCardST();
        }

        protected override void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.newRectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_HAND].transform, m_sceneDZData.m_cardHandleAreaWidthArr[(int)m_playerFlag], m_sceneCardList.Count(), ref m_posList);
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
            SceneCardBase tmpcard = null;
            if (SceneCardBase.BLACK_CARD_ID == objid)   // 如果是 enemy 手牌，由于没有 m_sceneCardItem 数据，只能使用 id 创建
            {
                tmpcard = Ctx.m_instance.m_sceneCardMgr.createCardById(objid, m_playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
            }
            else
            {
                tmpcard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData);
            }
            addCard(tmpcard);
            tmpcard.addEnterHandleEntryDisp(onOneCardEnterHandleEntry);
            //updateSceneCardST();
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

        // 从发牌去最终到手牌区起始位置，可能初始发牌，或者游戏中抽新卡牌
        public void onOneCardEnterHandleEntry(IDispatchObject card_)
        {
            SceneCardBase _card = card_ as SceneCardBase;
            updateSceneCardST();
        }
    }
}