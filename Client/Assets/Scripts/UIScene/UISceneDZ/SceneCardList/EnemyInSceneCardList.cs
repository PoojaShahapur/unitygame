using SDK.Common;
using UnityEngine;

namespace Game.UI
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
            // enemy 开拍直接进列表，就不在展示区域展示
            // 先创建手里的背面卡牌
            int idx = 0;
            while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount)
            {
                // 暂时硬编码卡牌 ID 1000
                SceneDragCard cardItem = m_sceneDZData.createOneCard(uint.MaxValue, m_playerFlag, CardArea.CARDCELLTYPE_HAND);
                addCard(cardItem);

                ++idx;
            }

            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount > 0)
            {
                updateSceneCardRST();
                updateCardIndex();
            }
        }

        public void removeEmptyCard()
        {
            // 移除最后一张
            if(m_sceneCardList.Count > 0)
            {
                UtilApi.Destroy(m_sceneCardList[m_sceneCardList.Count - 1].getGameObject());
                m_sceneCardList.RemoveAt(m_sceneCardList.Count - 1);                        // 移除数据
            }
        }
    }
}