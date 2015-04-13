using SDK.Common;
using System;

namespace SDK.Lib
{
    /**
     * @brief 资源加载监听项
     */
    public class ResListenerItem
    {
        public string m_path = "";               // 资源路径
        public Action<IDispatchObject> m_loaded;        // 加载成功回调函数
        public Action<IDispatchObject> m_failed;        // 加载失败回调函数

        public void copyForm(LoadParam param)
        {
            if (m_path.Length == 0)
            {
                m_path = param.m_path;
            }
            if (param.m_loaded != null)
            {
                m_loaded += param.m_loaded;
            }
            if (param.m_failed != null)
            {
                m_failed += param.m_failed;
            }
        }
    }
}