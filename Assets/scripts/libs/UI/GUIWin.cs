using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class GUIWin
    {
        public GameObject m_uiRoot;      // ui 根节点
        public Dictionary<ComponentType, List<UIWidget>> m_id2WinList;  // 控件列表
        public List<UIPanel> m_pnlList;         // 窗口中的 UIPanel，主要用来更改 Depth
    }
}
