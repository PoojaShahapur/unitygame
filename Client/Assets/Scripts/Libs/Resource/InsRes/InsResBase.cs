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
#if PKG_RES_LOAD
            m_bOrigResNeedImmeUnload = false;
#else
            m_bOrigResNeedImmeUnload = true;
#endif
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

        public void init(ResItem res)
        {
            initImpl(res);         // 内部初始化完成后，才分发事件
            refCountResLoadResultNotify.onLoadEventHandle(this);
        }

        // 这个是内部初始化实现，初始化都重载这个，但是现在很多都是重在了
        virtual protected void initImpl(ResItem res)
        {

        }

        virtual public void failed(ResItem res)
        {
            unload();
            refCountResLoadResultNotify.onLoadEventHandle(this);
        }

        virtual public void unload()
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