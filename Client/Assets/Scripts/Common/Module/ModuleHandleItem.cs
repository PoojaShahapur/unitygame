using System;

namespace SDK.Common
{
    /**
     * @brief 单一模块处理 Item 
     */
    public class ModuleHandleItem
    {
        public Action<IDispatchObject> m_loaded;
        public Action<IDispatchObject> m_failed;
        public ModuleID m_moduleID;
        public string m_moduleLayerPath;            // 所在的 Layer 的目录
        public string m_path;                       // 资源所在的目录
        public bool m_isLoaded;         // 指明模块是否加载过
    }
}