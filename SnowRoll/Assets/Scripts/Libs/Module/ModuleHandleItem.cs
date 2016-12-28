using System;

namespace SDK.Lib
{
    /**
     * @brief 单一模块处理 Item 
     */
    public class ModuleHandleItem
    {
        public MAction<IDispatchObject> mLoadEventHandle;
        public ModuleId m_moduleID;
        public string m_moduleLayerPath;            // 所在的 Layer 的目录
        public string mPath;                       // 资源所在的目录
        public bool mIsLoaded;         // 指明模块是否加载过
    }
}