using SDK.Common;
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
            m_posList.Clear();
            m_rotList.Clear();
            UtilMath.rectSplit(m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_COMMON].transform, m_internal, Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList.Length, ref m_posList, ref m_rotList);

            int idx = 0;
            while (idx < 4)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx] > 0)
                {
                    SceneDragCard cardItem = m_sceneDZData.createOneCard(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerFlag].m_startCardList[idx], m_playerFlag, CardArea.CARDCELLTYPE_HAND);
                    addCard(cardItem);

                    cardItem.startRot = new Vector3(-90f, -90f, 0);       // 将卡牌竖起来
                    cardItem.startPos = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition;
                    cardItem.destPos = m_posList[idx];
                    cardItem.destRot = new Vector3(0, 0, 0);

                    cardItem.moveToStart();        // 放到开始位置
                    cardItem.moveToDest();          // 播放动画
                }

                ++idx;
            }
        }

        // 自己的开拍需要监听卡牌的拖动
        public override void addCard(SceneDragCard card)
        {
            base.addCard(card);

            // 需要监听卡牌的拖动
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            card.m_moveDisp = uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_outSceneCardList.onMove;
        }
    }
};