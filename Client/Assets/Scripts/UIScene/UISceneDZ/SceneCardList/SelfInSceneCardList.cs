using SDK.Common;
using SDK.Lib;
using UnityEngine;
namespace Game.UI
{
    class SelfInSceneCardList : InSceneCardList
    {
        public SelfInSceneCardList(SceneDZData data, EnDZPlayer playerFlag)
            : base(data, playerFlag)
        {

        }

        // 对战开始显示的卡牌
        public override void addInitCard()
        {
            // 释放之前的所有的卡牌
            clearSceneCardList();

            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.rectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_COMMON].transform, m_bigInternal, Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList.Length, ref m_posList, ref m_rotList);

            int idx = 0;
            while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList.Length)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx] > 0)
                {
                    SceneDragCard cardItem = m_sceneDZData.createOneCard(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx], m_playerFlag, CardArea.CARDCELLTYPE_HAND);
                    addCard(cardItem);

                    // 记录开始卡牌的 id ，后面好判断更新
                    cardItem.m_startCardID = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx];
                    cardItem.updateCardOutState(true);
                    cardItem.startRot = new Vector3(-90f, -90f, 0);       // 将卡牌竖起来
                    cardItem.startPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition;
                    cardItem.destPos = m_posList[idx];
                    cardItem.destRot = new Vector3(0, 0, 0);

                    cardItem.moveToStart();        // 放到开始位置
                    cardItem.moveToDestRST();          // 播放动画

                    cardItem.updateCardDataByTable();          // 这个时候还没有服务器的数据，只能更新客户端表中的数据
                }

                ++idx;
            }

            updateCardIndex();
        }

        // 替换初始卡牌
        public override void replaceInitCard()
        {
            int idx = 0;
            SceneDragCard cardItem;
            Vector3 curPos;
            Quaternion curRot;

            // 释放之前的叉号
            while (idx < m_sceneCardList.Count)
            {
                cardItem = m_sceneCardList[idx];
                if (cardItem.m_startCardID != Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx])
                {
                    curPos = cardItem.gameObject.transform.localPosition;
                    curRot = cardItem.gameObject.transform.localRotation;
                    UtilApi.Destroy(cardItem.gameObject);      // 释放之前的资源

                    // 创建新的资源
                    cardItem = m_sceneDZData.createOneCard(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx], m_playerFlag, CardArea.CARDCELLTYPE_HAND);
                    cardItem.gameObject.transform.localPosition = curPos;
                    cardItem.gameObject.transform.localRotation = curRot;

                    cardItem.enableDrag();      // 开启拖动
                }

                ++idx;
            }
        }

        // 自己的开拍需要监听卡牌的拖动
        public override void addCard(SceneDragCard card)
        {
            base.addCard(card);

            // 需要监听卡牌的拖动
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            card.m_moveDisp = uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.onMove;
        }

        // 更新场景卡牌位置
        public override void startCardMoveTo()
        {
            int idx = 0;
            SceneDragCard cardItem;

            // 释放之前的叉号
            while (idx < m_sceneCardList.Count)
            {
                cardItem = m_sceneCardList[idx];
                if (cardItem.chaHaoGo != null)        // 如果之前添加的资源交换卡牌叉号
                {
                    UtilApi.Destroy(cardItem.chaHaoGo);
                }
                cardItem.enableDrag();      // 开启拖动

                ++idx;
            }

            base.startCardMoveTo();
        }

        override public void disableAllCardDragExceptOne(SceneDragCard card)
        {
            foreach (SceneDragCard cardItem in m_sceneCardList)
            {
                if(!cardItem.Equals(card))       // 如果内存地址相等
                {
                    cardItem.disableDrag();
                }
            }
        }

        override public void enableAllCardDragExceptOne(SceneDragCard card)
        {
            foreach (SceneDragCard cardItem in m_sceneCardList)
            {
                if (!cardItem.Equals(card))       // 如果内存地址相等
                {
                    cardItem.enableDrag();
                }
            }
        }
    }
};