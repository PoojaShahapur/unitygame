namespace SDK.Lib
{
    public class InsResBase : IDispatchObject
    {
        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;
        public string mLoadPath;
        public string mOrigPath;
        public string mPrefabName;
        protected string mResUniqueId;
        protected string mLogicPath;

        protected bool m_bOrigResNeedImmeUnload;        // 原始资源是否需要立刻卸载

        public InsResBase()
        {
            if (MacroDef.PKG_RES_LOAD)
            {
                m_bOrigResNeedImmeUnload = false;
            }
            else
            {
                m_bOrigResNeedImmeUnload = false;
            }
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

        public string getLoadPath()
        {
            return mLoadPath;
        }

        public string getOrigPath()
        {
            return mOrigPath;
        }

        public string getPrefabName()
        {
            return mPrefabName;
        }

        public void setResUniqueId(string value)
        {
            mResUniqueId = value;
        }

        public string getResUniqueId()
        {
            return mResUniqueId;
        }

        public void setLogicPath(string value)
        {
            mLogicPath = value;
        }

        public string getLogicPath()
        {
            return mLogicPath;
        }

        public void init(ResItem res)
        {
            initImpl(res);         // 内部初始化完成后，才分发事件
            m_refCountResLoadResultNotify.onLoadEventHandle(this);
        }

        // 这个是内部初始化实现，初始化都重载这个，但是现在很多都是重在了
        virtual protected void initImpl(ResItem res)
        {
            res.unrefAssetObject();     // 直接自己保存 Asset-Object 对象
        }

        virtual public void failed(ResItem res)
        {
            unload();
            m_refCountResLoadResultNotify.onLoadEventHandle(this);
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

        public bool hasSuccessLoaded()
        {
            return m_refCountResLoadResultNotify.resLoadState.hasSuccessLoaded();
        }

        public bool hasFailed()
        {
            return m_refCountResLoadResultNotify.resLoadState.hasFailed();
        }

        public void setLoadParam(LoadParam param)
        {
            this.mLoadPath = param.mLoadPath;
            this.mResUniqueId = param.mResUniqueId;
            this.mOrigPath = param.mOrigPath;
            this.mPrefabName = param.prefabName;
            this.mLogicPath = param.mLogicPath;
        }
    }
}