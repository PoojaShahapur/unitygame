using SDK.Common;
using System;

namespace Game.App
{
    /**
     * @brief 单一模块处理 Item 
     */
    public class ModuleHandleItem
    {
        public Action<IDispatchObject> m_loaded;
        public ModuleID m_moduleID;
        public string m_modulePath;
        public string m_moduleName;
        public bool m_isLoaded;         // 指明模块是否加载过
    }
}