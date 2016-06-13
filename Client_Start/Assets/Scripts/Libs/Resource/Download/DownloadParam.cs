using LuaInterface;

namespace SDK.Lib
{
    public enum DownloadType
    {
        eWWW,
        eHttpWeb,
        eTotal
    }

    /**
     * @brief 下载参数
     */
    public class DownloadParam
    {
        public string mLoadPath = "";               // 真正的资源加载目录
        public string mOrigPath = "";              // 原始资源加载目录，就是直接传递进来的目录
        public string mLogicPath;                   // 逻辑传递进来的目录，这个目录可能是没有扩展名字的，而 m_origPath 就是有扩展名字的，如果 mLogicPath 有扩展名字，就是和 m_origPath 完全一样了
        public string mResUniqueId;                 // 资源唯一 Id，查找资源的索引

        public string m_version = "";               // 加载的资源的版本号
        public MAction<IDispatchObject> m_loadEventHandle;    // 加载事件回调函数

        public DownloadType mDownloadType;     // 加载类型

        public LuaTable mLuaTable;
        public LuaFunction mLuaFunction;

        public DownloadParam()
        {
            reset();
        }

        public void reset()
        {
            mDownloadType = DownloadType.eHttpWeb;
        }
    }
}