using SDK.Common;
using System.Collections.Generic;

namespace FightCore
{
    /**
     * @brief 对战历史卡牌列表，只要有卡牌就添加到这个列表，一直保存到战斗结束
     */
    public class DZHistoryCardList
    {
        protected List<SceneCardItem> m_sceneItemList;          // 所有卡牌列表，历史流程会引用这个
    }
}