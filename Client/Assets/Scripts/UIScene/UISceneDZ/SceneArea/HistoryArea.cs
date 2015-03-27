using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
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
            item.prefab = "log";
            item.path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + item.prefab;
            item.texPrefab = "pig";
            item.texPath = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathImage] + item.texPrefab;
            m_historyList.addItem(item);
        }
    }
}