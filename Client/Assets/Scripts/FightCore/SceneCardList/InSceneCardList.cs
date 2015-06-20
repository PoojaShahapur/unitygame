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
        public InSceneCardList(SceneDZData data, EnDZPlayer playerSide)
            : base(data, playerSide)
        {

        }

        // 对战开始显示的卡牌
        public virtual void addInitCard()
        {

        }

        // 替换初始卡牌
        virtual public void replaceInitCard()
        {

        }

        // 这个函数不用了
        virtual public void startCardMoveTo()
        {
            
        }

        override protected void getCardPos()
        {
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.newRectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_HAND].transform, SceneDZCV.HAND_CARD_WIDTH, m_sceneDZData.m_cardHandAreaWidthArr[(int)m_playerSide], SceneDZCV.HAND_YDELTA, m_sceneCardList.Count(), ref m_posList);
        }

        // 通过客户端的数据移除一张卡牌
        override public void removeCard(SceneCardBase card)
        {
            // 关闭拖拽功能
            if (card.dragControl != null)       // Enemy Hand 手牌没有拖动
            {
                card.dragControl.disableDrag();
            }
            base.removeCard(card);
        }

        // 敌人发牌和自己发牌都走这里(除自己开始发牌到场景的 4 张牌)，通过服务器卡牌数据添加卡牌
        public void addCardByIdAndItem(uint objid, SceneCardItem sceneItem)
        {
            SceneCardBase tmpcard = null;
            if (SceneDZCV.BLACK_CARD_ID == objid)   // 如果是 enemy 手牌，由于没有 m_sceneCardItem 数据，只能使用 id 创建
            {
                tmpcard = Ctx.m_instance.m_sceneCardMgr.createCardById(objid, m_playerSide, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                tmpcard.updateInitCardSceneInfo(m_sceneDZData.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_NONE].transform);
                tmpcard.startEnemyFaPaiAni();       // 播放动画
            }
            else
            {
                tmpcard = Ctx.m_instance.m_sceneCardMgr.createCard(sceneItem, m_sceneDZData);
                tmpcard.updateInitCardSceneInfo(m_sceneDZData.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_NONE].transform);
                tmpcard.start2HandleAni();          // 播放动画
            }
            addCard(tmpcard);
            tmpcard.addEnterHandleEntryDisp(onOneCardEnterHandleEntry);
        }

        // 清空卡牌列表
        public void clearSceneCardList()
        {
            int idx = m_sceneCardList.Count() - 1;
            for(; idx >= 0; --idx)
            {
                m_sceneCardList[idx].dispose();         // 这个地方内部删除的时候会从 m_sceneCardList 这个列表中删除数据，防止遍历过程中删除数据，因此从后向前遍历数据
            }

            m_sceneCardList.Clear();
        }

        // 是否还有剩余的点数可以使用
        public bool hasLeftMagicPtCanUse()
        {
            foreach (SceneCardBase card in m_sceneCardList.list)
            {
                if(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp >= card.sceneCardItem.svrCard.mpcost)
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

        override public void updateSceneCardPos(bool bUpdateIdx = true)
        {
            updateSceneCardPosInternal(CardArea.CARDCELLTYPE_HAND);
        }
    }
}