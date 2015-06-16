using Fight;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    class SelfInSceneCardList : InSceneCardList
    {
        protected int m_initCardCount = 0;

        public SelfInSceneCardList(SceneDZData data, EnDZPlayer playerFlag)
            : base(data, playerFlag)
        {

        }

        // 对战开始显示的卡牌
        public override void addInitCard()
        {
            // 释放之前的所有的卡牌
            clearSceneCardList();

            SceneCardBase cardItem;

            m_initCardCount = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList.Length;
            int idx = 0;
            while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList.Length)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx] > 0)
                {
                    cardItem = Ctx.m_instance.m_sceneCardMgr.createCardById(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx], m_playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                    addCard(cardItem);
                    // 记录开始卡牌的 id ，后面好判断更新
                    cardItem.startCardID = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx];
                    cardItem.setStartIdx(idx);
                    cardItem.updateCardOutState(true);
                    cardItem.updateInitCardSceneInfo(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform);
                    cardItem.updateCardDataByTable();          // 这个时候还没有服务器的数据，只能更新客户端表中的数据
                    cardItem.faPai2MinAni();
                }

                ++idx;
            }

            //updateCardIndex();
        }

        // 替换初始卡牌
        public override void replaceInitCard()
        {
            int idx = 0;
            SceneCardBase cardItem;
            Vector3 curPos;
            Quaternion curRot;

            // 释放之前的叉号
            while (idx < m_sceneCardList.Count())
            {
                cardItem = m_sceneCardList[idx];
                if (cardItem.startCardID != Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx])
                {
                    curPos = cardItem.gameObject().transform.localPosition;
                    curRot = cardItem.gameObject().transform.localRotation;
                    UtilApi.Destroy(cardItem.gameObject());      // 释放之前的资源

                    // 创建新的资源
                    cardItem = Ctx.m_instance.m_sceneCardMgr.createCardById(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx], m_playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                    UtilApi.setPos(cardItem.transform(), curPos);
                    UtilApi.setRot(cardItem.transform(), curRot);

                    cardItem.dragControl.enableDrag();      // 开启拖动
                }

                ++idx;
            }
        }

        // 自己的开拍需要监听卡牌的拖动
        public override void addCard(SceneCardBase card, int idx = 0)
        {
            base.addCard(card, idx);
            // 需要监听卡牌的拖动
            card.dragControl.m_moveDisp = m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.onMove;
        }

        // 移动初始卡牌到手牌列表，更新场景卡牌位置
        public override void startCardMoveTo()
        {
            int idx = 0;
            SceneCardBase cardItem;

            // 释放之前的叉号
            while (idx < m_sceneCardList.Count())
            {
                cardItem = m_sceneCardList[idx];
                if (cardItem.chaHaoGo != null)        // 如果之前添加的资源交换卡牌叉号
                {
                    UtilApi.Destroy(cardItem.chaHaoGo);
                }
                cardItem.dragControl.enableDrag();      // 开启拖动
                cardItem.min2HandleAni();
                cardItem.addEnterHandleEntryDisp(onSelfStartCardEnterHandEntry);

                ++idx;
            }

            //base.startCardMoveTo();
        }

        override public void disableAllCardDragExceptOne(SceneCardBase card)
        {
            foreach (SceneCardBase cardItem in m_sceneCardList.list)
            {
                if(!cardItem.Equals(card))       // 如果内存地址相等
                {
                    cardItem.dragControl.disableDrag();
                }
            }
        }

        override public void enableAllCardDragExceptOne(SceneCardBase card)
        {
            foreach (SceneCardBase cardItem in m_sceneCardList.list)
            {
                if (!cardItem.Equals(card))       // 如果内存地址相等
                {
                    cardItem.dragControl.enableDrag();
                }
            }
        }

        public void onSelfStartCardEnterHandEntry(IDispatchObject card_)
        {
            --m_initCardCount;
            if(0 == m_initCardCount)
            {
                updateSceneCardPos();
            }
        }
    }
};