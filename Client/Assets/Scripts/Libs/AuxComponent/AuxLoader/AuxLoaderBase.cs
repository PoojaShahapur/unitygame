using LuaInterface;
using System;

namespace SDK.Lib
{
    /**
     * @brief 资源加载器
     */
    public class AuxLoaderBase : IDispatchObject
    {
        protected bool mIsSuccess;      // 是否成功
        protected string mPath;         // 加载的资源目录
        protected ResEventDispatch mEvtHandle;              // 事件分发器

        public AuxLoaderBase()
        {
            mIsSuccess = false;
            mPath = "";
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

        virtual public string getLogicPath()
        {
            return mPath;
        }

        virtual public void syncLoad(string path)
        {

        }

        virtual public void asyncLoad(string path, Action<IDispatchObject> dispObj)
        {

        }

        virtual public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction)
        {

        }

        virtual public void unload()
        {

        }
    }
}