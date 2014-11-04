using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 框架层管理器
     */
    public class LayerMgr
    {
        public Dictionary<string, GameObject> m_path2Go = new Dictionary<string,GameObject>();
    }
}