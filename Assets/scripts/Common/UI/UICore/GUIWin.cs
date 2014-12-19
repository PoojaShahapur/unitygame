using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    public class GUIWin
    {
        public GameObject m_uiRoot;      // ui 根节点
        //public Dictionary<ComponentType, List<UIWidget>> m_id2WinList = new Dictionary<ComponentType, List<UIWidget>>();  // 控件列表
        public UIButton[] m_btnArr;     // 按钮列表
        public List<UIPanel> m_pnlList = new List<UIPanel>();         // 窗口中的 UIPanel，主要用来更改 Depth
    }
}