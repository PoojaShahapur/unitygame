using SDK.Common;

namespace SDK.Lib
{
    public class InsResBase : IDispatchObject
    {
        protected ResLoadState m_resLoadState;  // 资源加载状态
        protected RefCount m_refCount;
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

        public RefCount refCount
        {
            get
            {
                return m_refCount;
            }
            set
            {
                m_refCount = value;
            }
        }

        // 是否加载完成，可能成功可能失败
        public bool hasLoaded()
        {
            return m_resLoadState == ResLoadState.eFailed || m_resLoadState == ResLoadState.eLoaded;
        }

        public bool hasSuccessLoaded()
        {
            return m_resLoadState == ResLoadState.eLoaded;
        }

        public bool hasFailed()
        {
            return m_resLoadState == ResLoadState.eFailed;
        }

        public void setSuccessLoaded()
        {
            m_resLoadState = ResLoadState.eLoaded;
        }

        public void setFailed()
        {
            m_resLoadState = ResLoadState.eFailed;
        }

    }
}