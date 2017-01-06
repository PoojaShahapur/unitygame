namespace SDK.Lib
{
    public class InsResBase : IDispatchObject
    {
        protected RefCountResLoadResultNotify mRefCountResLoadResultNotify;
        public string mLoadPath;
        public string mOrigPath;
        public string mPrefabName;
        protected string mResUniqueId;
        protected string mLogicPath;

        protected bool mIsOrigResNeedImmeUnload;        // 原始资源是否需要立刻卸载

        public InsResBase()
        {
            if (MacroDef.PKG_RES_LOAD)
            {
                mIsOrigResNeedImmeUnload = false;
            }
            else
            {
                mIsOrigResNeedImmeUnload = false;
            }
            mRefCountResLoadResultNotify = new RefCountResLoadResultNotify();
        }

        public bool isOrigResNeedImmeUnload
        {
            get
            {
                return mIsOrigResNeedImmeUnload;
            }
            set
            {
                mIsOrigResNeedImmeUnload = value;
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
            mRefCountResLoadResultNotify.onLoadEventHandle(this);
        }

        // 这个是内部初始化实现，初始化都重载这个，但是现在很多都是重在了
        virtual protected void initImpl(ResItem res)
        {
            res.unrefAssetObject();     // 直接自己保存 Asset-Object 对象
        }

        virtual public void failed(ResItem res)
        {
            unload();
            mRefCountResLoadResultNotify.onLoadEventHandle(this);
        }

        virtual public void unload()
        {

        }

        public RefCountResLoadResultNotify refCountResLoadResultNotify
        {
            get
            {
                return mRefCountResLoadResultNotify;
            }
            set
            {
                mRefCountResLoadResultNotify = value;
            }
        }

        public bool hasSuccessLoaded()
        {
            return mRefCountResLoadResultNotify.resLoadState.hasSuccessLoaded();
        }

        public bool hasFailed()
        {
            return mRefCountResLoadResultNotify.resLoadState.hasFailed();
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