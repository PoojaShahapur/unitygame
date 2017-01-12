using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief 资源加载器
     */
    public class AuxLoaderBase : GObject, IDispatchObject
    {
        protected ResLoadState mResLoadState;      // 资源加载状态
        protected string mPrePath;      // 之前加载的资源目录
        protected string mPath;         // 加载的资源目录
        protected ResEventDispatch mEvtHandle;              // 事件分发器
        protected bool mIsInvalid;      // 加载器是否无效
        protected string mInitPath;     // 初始化目录

        public AuxLoaderBase(string path = "")
        {
            this.mInitPath = path;
            this.mResLoadState = new ResLoadState();

            this.reset();
        }

        protected void reset()
        {
            this.mResLoadState.reset();
            this.mPrePath = "";
            this.mPath = "";
            this.mIsInvalid = true;
        }

        virtual public void dispose()
        {
            this.unload();
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

        protected void addEventHandle(MAction<IDispatchObject> evtHandle = null)
        {
            if (null != evtHandle)
            {
                if (null == this.mEvtHandle)
                {
                    this.mEvtHandle = new ResEventDispatch();
                }
                this.mEvtHandle.addEventHandle(null, evtHandle);
            }
        }

        protected void addEventHandle(LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null)
        {
            if (null != luaTable && null != luaFunction)
            {
                if (null == this.mEvtHandle)
                {
                    this.mEvtHandle = new ResEventDispatch();
                }
                this.mEvtHandle.addEventHandle(null, null, luaTable, luaFunction);
            }
        }

        virtual public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(evtHandle);
        }

        virtual public void syncLoad(string path, LuaTable luaTable, LuaFunction luaFunction)
        {
            this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(luaTable, luaFunction);
        }

        virtual public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(evtHandle);
        }

        virtual public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction)
        {
            this.mResLoadState.setLoading();

            this.updatePath(path);

            this.addEventHandle(luaTable, luaFunction);
        }

        virtual public void download(string origPath, MAction<IDispatchObject> dispObj = null, long fileLen = 0, bool isWriteFile = true, int downloadType = (int)DownloadType.eHttpWeb)
        {
            this.mResLoadState.setLoading();

            this.updatePath(origPath);

            this.addEventHandle(dispObj);
        }

        virtual public void unload()
        {
            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.clearEventHandle();
                this.mEvtHandle = null;
            }

            this.reset();
        }
    }
}