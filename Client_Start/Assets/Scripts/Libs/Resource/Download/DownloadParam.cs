using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief 下载参数
     */
    public class DownloadParam
    {
        public ResPackType m_resPackType;           // 加载资源的类型
        public ResLoadType m_resLoadType;           // 资源加载类型

        public string mLoadPath = "";               // 真正的资源加载目录
        public string m_origPath = "";              // 原始资源加载目录，就是直接传递进来的目录
        protected string m_prefabName = "";         // 预设的名字，就是在 AssetBundle 里面完整的资源目录和名字
        protected string m_extName = "prefab";      // 加载的原始资源的扩展名字，不是 AssetBundles 的扩展名字
        public string m_pathNoExt = "";             // mLoadPath 的没有扩展名字的路径
        public string mLogicPath;                   // 逻辑传递进来的目录，这个目录可能是没有扩展名字的，而 m_origPath 就是有扩展名字的，如果 mLogicPath 有扩展名字，就是和 m_origPath 完全一样了

        public string m_subPath = "";               // 子目录，可能一个包中有多个资源
        public string m_pakPath = "";               // 打包的资源目录，如果打包， m_pakPath 应该就是 m_path
        public string mResUniqueId;                 // 资源唯一 Id，查找资源的索引

        public string m_version = "";               // 加载的资源的版本号
        protected string m_lvlName = "";            // 关卡名字
        public MAction<IDispatchObject> m_loadEventHandle;    // 加载事件回调函数

        public bool m_resNeedCoroutine = true;      // 资源是否需要协同程序
        public bool m_loadNeedCoroutine = true;     // 加载是否需要协同程序

        public ResItem m_loadRes = null;
        public InsResBase m_loadInsRes = null;
        public LuaTable mLuaTable;
        public LuaFunction mLuaFunction;
        public bool mIsLoadAll;                 // 是否一次性加载所有的内容
        public bool mIsCheckDep;                // 是否检查依赖
    }
}