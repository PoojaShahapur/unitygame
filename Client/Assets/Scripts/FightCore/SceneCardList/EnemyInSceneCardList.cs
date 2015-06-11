using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 手里的卡牌列表
     */
    public class EnemyInSceneCardList : InSceneCardList
    {
        public EnemyInSceneCardList(SceneDZData data, EnDZPlayer playerFlag)
            : base(data, playerFlag)
        {

        }

        // 对战开始显示的卡牌
        public override void addInitCard()
        {
            // 释放之前的所有的卡牌
            clearSceneCardList();

            // enemy 开拍直接进列表，就不在展示区域展示
            // 先创建手里的背面卡牌
            int idx = 0;
            while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount)
            {
                SceneCardBase cardItem = Ctx.m_instance.m_sceneCardMgr.createCardById(SceneCardBase.BLACK_CARD_ID, m_playerFlag, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, m_sceneDZData);
                addCard(cardItem);

                ++idx;
            }

            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount > 0)
            {
                updateSceneCardST();
                updateCardIndex();
            }
        }

        public void removeEmptyCard()
        {
            // 移除最后一张
            if(m_sceneCardList.Count() > 0)
            {
                Ctx.m_instance.m_sceneCardMgr.removeAndDestroy(m_sceneCardList[m_sceneCardList.Count() - 1]);
                m_sceneCardList.RemoveAt(m_sceneCardList.Count() - 1);                        // 移除数据
            }
        }
    }
}