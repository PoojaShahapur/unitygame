using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief 资源加载器
     */
    public class AuxLoaderBase : GObject, IDispatchObject
    {
        protected bool mIsSuccess;      // 是否成功
        protected string mPrePath;      // 之前加载的资源目录
        protected string mPath;         // 加载的资源目录
        protected ResEventDispatch mEvtHandle;              // 事件分发器
        protected bool mIsInvalid;       // 加载器是否无效

        public AuxLoaderBase()
        {
            this.reset();
        }

        protected void reset()
        {
            mIsSuccess = false;
            mPrePath = "";
            mPath = "";
            mIsInvalid = true;
        }

        virtual public void dispose()
        {
            this.unload();
        }

        public bool hasSuccessLoaded()
        {
            return mIsSuccess;
        }

        public bool hasFailed()
        {
            return !mIsSuccess;
        }

        // 是否需要卸载资源
        public bool needUnload(string path)
        {
            return mPath != path && !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(mPath);
        }

        public void setPath(string path)
        {
            mPrePath = mPath;
            mPath = path;

            if(mPrePath != mPath && !string.IsNullOrEmpty(mPath))
            {
                mIsInvalid = true;
            }
            else
            {
                mIsInvalid = false;
            }
        }

        public bool isInvalid()
        {
            return mIsInvalid;
        }

        virtual public string getLogicPath()
        {
            return mPath;
        }

        virtual public void syncLoad(string path, MAction<IDispatchObject> dispObj = null)
        {

        }

        virtual public void asyncLoad(string path, MAction<IDispatchObject> dispObj)
        {

        }

        virtual public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction)
        {

        }

        virtual public void unload()
        {
            if (mEvtHandle != null)
            {
                mEvtHandle.clearEventHandle();
                mEvtHandle = null;
            }

            this.reset();
        }
    }
}