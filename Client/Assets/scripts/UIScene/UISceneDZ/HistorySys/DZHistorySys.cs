using Game.Msg;
using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 对战历史数据系统
     */
    public class DZHistorySys
    {
        //protected DZHistoryCardList m_DZHistoryCardList;            // 历史卡牌列表
        //protected DZHistoryOpList m_DZHistoryOpList;                // 对战历史操作

        protected List<stRetBattleHistoryInfoUserCmd> m_historyList = new List<stRetBattleHistoryInfoUserCmd>();        // 历史列表
    }
}