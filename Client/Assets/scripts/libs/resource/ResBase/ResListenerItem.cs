using SDK.Common;
using System;

namespace SDK.Lib
{
    /**
     * @brief 资源加载监听项
     */
    public class ResListenerItem
    {
        public string m_prefabName = "";         // 预设的名字
        public string m_path = "";               // 资源路径
        public Action<IDispatchObject> m_loaded;        // 加载成功回调函数
        public Action<IDispatchObject> m_failed;        // 加载失败回调函数

        public void copyForm(LoadParam param)
        {
            m_prefabName = param.m_prefabName;
            m_path = param.m_path;
            m_loaded += param.m_loaded;
            m_failed += param.m_failed;
        }
    }
}