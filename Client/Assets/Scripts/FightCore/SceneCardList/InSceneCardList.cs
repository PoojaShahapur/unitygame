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
            UtilMath.newRectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_HAND].transform, SceneDZCV.HAND_CARD_WIDTH, m_sceneDZData.m_cardHandAreaWidthArr[(int)m_playerFlag], SceneDZCV.HAND_YDELTA, m_sceneCardList.Count(), ref m_posList);
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

        // 敌人发牌和自己发牌都走这里(除自己开始发牌到场景的 4 张牌)
        public void addSceneCard(uint objid, SceneCardItem sceneItem)
        {
            SceneCardBase tmpcard = null;
            if (SceneDZCV.BLACK_CARD_ID == objid)   // 如果是 enemy 手牌，由于没有 m_sceneCardItem 数据，只能使用 id 创建
            {
                tmpcard = Ctx.m_instance.m_sceneCardMgr.createCardById(objid, m_playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                tmpcard.startEnemyFaPaiAni();       // 播放动画
            }
            else
            {
                tmpcard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData);
                tmpcard.start2HandleAni();          // 播放动画
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
            updateSceneCardPos();       // 开始发送到手牌，只更新位置就行了
        }
    }
}