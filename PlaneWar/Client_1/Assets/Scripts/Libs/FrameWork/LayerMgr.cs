using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 框架层管理器
     */
    public class LayerMgr
    {
        public MDictionary<string, GameObject> mPath2Go;

        public LayerMgr()
        {
            this.mPath2Go = new MDictionary<string, GameObject>();
        }
    }
}