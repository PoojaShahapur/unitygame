using SDK.Common;

namespace SDK.Lib
{
    public class InsResBase : IDispatchObject
    {
        protected ResLoadState m_resLoadState;  // 资源加载状态
        protected RefCount m_refCount;
        public string m_path;

        public InsResBase()
        {
            m_resLoadState = new ResLoadState();
        }

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

        public ResLoadState resLoadState
        {
            get
            {
                return m_resLoadState;
            }
            set
            {
                m_resLoadState = value;
            }
        }
    }
}