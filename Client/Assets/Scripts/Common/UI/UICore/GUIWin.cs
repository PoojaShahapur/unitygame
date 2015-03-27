using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Common
{
    public class GUIWin
    {
        public GameObject m_uiRoot;      // ui 根节点
        //public Dictionary<ComponentType, List<UIWidget>> m_id2WinList = new Dictionary<ComponentType, List<UIWidget>>();  // 控件列表
        public Button[] m_btnArr;     // 按钮列表
    }
}