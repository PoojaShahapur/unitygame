using Fight;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    class SelfInSceneCardList : InSceneCardList
    {
        protected int m_initCardCount = 0;
        protected DynSceneGrid m_dynSceneGrid;

        public SelfInSceneCardList(SceneDZData data, EnDZPlayer playerSide)
            : base(data, playerSide)
        {
            
        }

        override public void init()
        {
            base.init();

            m_dynSceneGrid = new DynSceneGrid();
            m_dynSceneGrid.centerPos = m_centerPos;
            m_dynSceneGrid.elemNormalWidth = SceneDZCV.HAND_CARD_WIDTH;
            m_dynSceneGrid.radius = m_radius;
            m_dynSceneGrid.yDelta = SceneDZCV.HAND_YDELTA;
        }

        // 对战开始显示的卡牌
        public override void addInitCard()
        {
            // 释放之前的所有的卡牌
            clearSceneCardList();

            SceneCardBase cardItem;

            m_initCardCount = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList.Length;
            int idx = 0;
            while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList.Length)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList[idx] > 0)
                {
                    cardItem = Ctx.m_instance.m_sceneCardMgr.createCardById(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList[idx], m_playerSide, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                    addCard(cardItem);
                    // 记录开始卡牌的 id ，后面好判断更新
                    cardItem.startCardID = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList[idx];
                    cardItem.setStartIdx(idx);
                    cardItem.updateCardOutState(true);
                    cardItem.updateInitCardSceneInfo(m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_NONE].transform);
                    cardItem.updateCardDataByTable();          // 这个时候还没有服务器的数据，只能更新客户端表中的数据
                    cardItem.sceneCardBaseData.m_trackAniControl.faPai2MinAni();
                }

                ++idx;
            }
        }

        // 替换初始卡牌
        public override void replaceInitCard()
        {
            int idx = 0;
            SceneCardBase cardItem;

            // 替换卡牌
            while (idx < m_sceneCardList.Count())
            {
                cardItem = m_sceneCardList[idx];
                if (cardItem.startCardID != Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList[idx])
                {
                    cardItem.startCardID = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_startCardList[idx];
                    cardItem.updateRenderInfo((int)cardItem.startCardID);
                }

                ++idx;
            }
        }

        // 自己的开拍需要监听卡牌的拖动
        public override void addCard(SceneCardBase card, int idx = -1)
        {
            base.addCard(card, idx);
            // 需要监听卡牌的拖动
            card.ioControl.setMoveDisp(m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.onMove);
        }

        override public void removeCard(SceneCardBase card)
        {
            card.trackAniControl.removeGridElem();
            base.removeCard(card);
        }

        // 移动初始卡牌到手牌列表，更新场景卡牌位置
        override public void startCardMoveTo()
        {
            int idx = 0;
            SceneCardBase cardItem;

            // 释放之前的叉号
            while (idx < m_sceneCardList.Count())
            {
                cardItem = m_sceneCardList[idx];
                cardItem.destroyChaHaoModel();
                cardItem.ioControl.enableDrag();      // 开启拖动
                cardItem.sceneCardBaseData.m_trackAniControl.min2HandleAni();
                cardItem.sceneCardBaseData.m_trackAniControl.addEnterHandleEntryDisp(onSelfStartCardEnterHandEntry);

                ++idx;
            }
        }

        override public void disableAllCardDragExceptOne(SceneCardBase card)
        {
            foreach (SceneCardBase cardItem in m_sceneCardList.list())
            {
                if(!cardItem.Equals(card))       // 如果内存地址相等
                {
                    cardItem.ioControl.disableDrag();
                }
            }
        }

        override public void enableAllCardDragExceptOne(SceneCardBase card)
        {
            foreach (SceneCardBase cardItem in m_sceneCardList.list())
            {
                if (!cardItem.Equals(card))       // 如果内存地址相等
                {
                    cardItem.ioControl.enableDrag();
                }
            }
        }

        public void onSelfStartCardEnterHandEntry(IDispatchObject card_)
        {
            SceneCardBase _card = card_ as SceneCardBase;
            _card.trackAniControl.createAndAddGridElem();

            --m_initCardCount;
            if(0 == m_initCardCount)
            {
                updateSceneCardPos();
            }
        }

        // 自己手牌更新位置信息
        override public void updateSceneCardPos()
        {
            m_dynSceneGrid.updateElem();
            updateCardIndex();
        }

        override public DynSceneGrid getDynSceneGrid()
        {
            return m_dynSceneGrid;
        }

        override public void onOneCardEnterHandleEntry(IDispatchObject card_)
        {
            SceneCardBase _card = card_ as SceneCardBase;
            _card.trackAniControl.createAndAddGridElem();
            base.onOneCardEnterHandleEntry(card_);
        }
    }
};