using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public class LoadParam
    {
        public ResType m_type;              // 加载资源的类型
        public string m_path;               // 资源路径
        public string m_lvlName;            // 关卡名字
        public Func<IRes, bool> m_cb;        // 加载完成回调函数

        public bool m_resNeedCoroutine;     // 资源是否需要协同程序
        public bool m_loadNeedCoroutine;    // 加载是否需要协同程序

        public string m_prefabName;         // 预设的名字
    }
}
