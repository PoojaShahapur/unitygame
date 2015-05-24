using SDK.Common;

namespace SDK.Lib
{
    public class InsResBase : IDispatchObject
    {
        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;
        public string m_path;
        protected bool m_bOrigResNeedImmeUnload;        // 原始资源是否需要立刻卸载

        public InsResBase()
        {
            m_bOrigResNeedImmeUnload = true;
            m_refCountResLoadResultNotify = new RefCountResLoadResultNotify();
        }

        public bool bOrigResNeedImmeUnload
        {
            get
            {
                return m_bOrigResNeedImmeUnload;
            }
            set
            {
                m_bOrigResNeedImmeUnload = value;
            }
        }

        public string GetPath()
        {
            return m_path;
        }

        public string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return "";
        }

        virtual public void init(ResItem res)
        {
            refCountResLoadResultNotify.onLoadEventHandle(this);
        }

        virtual public void failed(ResItem res)
        {
            unload();
            refCountResLoadResultNotify.onLoadEventHandle(this);
        }

        public virtual void unload()
        {

        }

        public RefCountResLoadResultNotify refCountResLoadResultNotify
        {
            get
            {
                return m_refCountResLoadResultNotify;
            }
            set
            {
                m_refCountResLoadResultNotify = value;
            }
        }
    }
}