using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 历史区域
     */
    public class HistoryArea
    {
        public SceneDZData m_sceneDZData;
        public List<SceneDragCard> m_historySceneCardList = new List<SceneDragCard>(); // 已经出过的牌的列表
    }
}