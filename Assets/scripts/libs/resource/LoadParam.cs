using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    public class LoadParam
    {
        public ResType m_type;              // 加载资源的类型
        public string m_path;               // 资源路径
        public string m_lvlName;            // 关卡名字
        public Func<Res, bool> m_cb;        // 加载完成回调函数

        public bool m_resNeedCoroutine;     // 资源是否需要协同程序
        public bool m_loadNeedCoroutine;    // 加载是否需要协同程序
    }
}
