using SDK.Common;

namespace SDK.Lib
{
    public class InsResBase : RefCount, IDispatchObject
    {
        public bool m_isLoaded;              // 资源是否加载完成
        public bool m_isSucceed;             // 资源是否加载成功
        public string m_path;

        public string GetPath()
        {
            return m_path;
        }

        public string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return "";
        }

        public virtual void unload()
        {

        }
    }
}