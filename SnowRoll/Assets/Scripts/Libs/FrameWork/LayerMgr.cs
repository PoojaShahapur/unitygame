using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 框架层管理器
     */
    public class LayerMgr
    {
        public Dictionary<string, GameObject> mPath2Go = new Dictionary<string,GameObject>();
    }
}