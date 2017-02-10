namespace SDK.Lib
{
    /**
     * @brief 资源加载器
     */
    public class AuxLoaderBase : GObject, IDispatchObject, IRecycle, IDelayTask
    {
        protected ResLoadState mResLoadState;      // 资源加载状态
        protected string mPrePath;      // 之前加载的资源目录
        protected string mPath;         // 加载的资源目录
        protected ResEventDispatch mEvtHandle;              // 事件分发器
        protected AddOnceEventDispatch mProgressEventDispatch;  // 加载进度事件分发器，这个最终分发的参数是 LoadItem，直接分发
        protected bool mIsInvalid;      // 加载器是否无效

        public AuxLoaderBase(string path = "")
        {
            this.mResLoadState = new ResLoadState();

            this.reset();
        }

        protected void reset()
        {
            this.mResLoadState.reset();
            this.mPrePath = "";
            this.mPath = "";
            this.mIsInvalid = true;
            this.mProgressEventDispatch = null;
        }

        virtual public void dispose()
        {
            this.unload();
        }

        virtual public void resetDefault()
        {

        }

        public static IRecycle getObject(string id)
        {
            IRecycle ret = null;
            ret = Ctx.mInstance.mIdPoolSys.getObject(id);

            if (null != ret)
            {
                (ret as AuxLoaderBase).onGetPool();
            }

            return ret;
        }

        // 从内存池获取
        virtual protected void onGetPool()
        {

        }

        public void deleteObj()
        {
            this.onRetPool();
            Ctx.mInstance.mIdPoolSys.deleteObj(this.mPath, this);
        }

        // 放到内存池
        virtual protected void onRetPool()
        {
            if (null != this.mEvtHandle)
            {
                this.mEvtHandle.clearEventHandle();
            }

            if (null != this.mProgressEventDispatch)
            {
                this.mProgressEventDispatch.clearEventHandle();
            }
        }

        public bool hasSuccessLoaded()
        {
            return this.mResLoadState.hasSuccessLoaded();
        }

        public bool hasFailed()
        {
            return this.mResLoadState.hasFailed();
        }

        // 加载成功或者加载失败
        public bool hasLoadEnd()
        {
            return (this.hasSuccessLoaded() || this.hasFailed());
        }

        // 是否需要卸载资源
        public bool needUnload(string path)
        {
            return this.mPath != path && !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(this.mPath);
        }

        public void setPath(string path)
        {
            this.mPrePath = mPath;
            this.mPath = path;

            if(this.mPrePath != this.mPath && !string.IsNullOrEmpty(this.mPath))
            {
                this.mIsInvalid = true;
            }
            else
            {
                this.mIsInvalid = false;
            }
        }

        public void updatePath(string path)
        {
            if (this.needUnload(path))
            {
                this.unload();
            }

            this.setPath(path);
        }

        public bool isInvalid()
        {
            return this.mIsInvalid;
        }

        virtual public string getLogicPath()
        {
            return this.mPath;
        }

        // 真正的开始加载
        protected void onStartLoad()
        {
            this.mResLoadState.setLoading();
        }

        protected void addEventHandle(MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null)
        {
            if (null != evtHandle)
            {
                if (null == this.mEvtHandle)
                {
                    this.mEvtHandle = new ResEventDispatch();
                }

                this.mEvtHandle.addEventHandle(null, evtHandle);
            }

            if (null != progressHandle)
            {
                if (null == this.mProgressEventDispatch)
                {
                    this.mProgressEventDispatch = new AddOnceEventDispatch();
                }

                this.mProgressEventDispatch.addEventHandle(null, progressHandle);
            }
        }

        protected void addEventHandle(LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            if (null != luaTable && null != luaFunction)
            {
                if (null == this.mEvtHandle)
                {
                    this.mEvtHandle = new ResEventDispatch();
                }

                this.mEvtHandle.addEventHandle(null, null, luaTable, luaFunction);
            }

            if (null != luaTable && null != progressLuaFunction)
            {
                if (null == this.mProgressEventDispatch)
                {
                    this.mProgressEventDispatch = new AddOnceEventDispatch();
                }

                this.mProgressEventDispatch.addEventHandle(null, null, luaTable, progressLuaFunction);
            }
        }

        virtual public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null)
        {
            //this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(evtHandle, progressHandle);
        }

        virtual public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            //this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(luaTable, luaFunction, progressLuaFunction);
        }

        virtual public void asyncLoad(string path, MAction<IDispatchObject> evtHandle, MAction<IDispatchObject> progressHandle = null)
        {
            //this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(evtHandle, progressHandle);
        }

        virtual public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            //this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(luaTable, luaFunction, progressLuaFunction);
        }

        virtual public void download(string origPath, MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null, long fileLen = 0, bool isWriteFile = true, int downloadType = (int)DownloadType.eHttpWeb)
        {
            //this.mResLoadState.setLoading();

            this.updatePath(origPath);

            this.addEventHandle(evtHandle, progressHandle);
        }

        virtual public void download(string origPath, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null, long fileLen = 0, bool isWriteFile = true, int downloadType = (int)DownloadType.eHttpWeb)
        {
            //this.mResLoadState.setLoading();

            this.updatePath(origPath);

            this.addEventHandle(luaTable, luaFunction, progressLuaFunction);
        }

        virtual public void unload()
        {
            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.clearEventHandle();
                this.mEvtHandle = null;
            }

            if (this.mProgressEventDispatch != null)
            {
                this.mProgressEventDispatch.clearEventHandle();
                this.mProgressEventDispatch = null;
            }

            this.reset();
        }

        // 加载进度事件处理器
        public void onProgressEventHandle(IDispatchObject dispObj)
        {
            if(null != this.mProgressEventDispatch)
            {
                this.mProgressEventDispatch.dispatchEvent(dispObj);
            }
        }

        virtual public void delayExec()
        {

        }
    }
}