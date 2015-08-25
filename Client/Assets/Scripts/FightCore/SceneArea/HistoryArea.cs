using Game.Msg;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 历史区域
     */
    public class HistoryArea
    {
        public SceneDZData m_sceneDZData;
        protected SlideList m_historyList;

        public HistoryArea(GameObject go_)
        {
            m_historyList = new SlideList(go_);
        }

        public void psstRetBattleHistoryInfoUserCmd(stRetBattleHistoryInfoUserCmd cmd)
        {
            SlideListItem item = new SlideListItem();
            item.data = cmd;
            item.path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "log.prefab");
            item.texPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBuildImage], "pig.png");
            m_historyList.addItem(item);
        }
    }
}