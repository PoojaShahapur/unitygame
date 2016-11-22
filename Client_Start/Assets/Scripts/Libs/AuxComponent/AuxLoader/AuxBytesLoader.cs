using System;

namespace SDK.Lib
{
    public class AuxBytesLoader : AuxLoaderBase
    {
        protected BytesRes mBytesRes;

        public AuxBytesLoader(string path = "")
            : base(path)
        {
            mBytesRes = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public Byte[] getBytes()
        {
            return mBytesRes.getBytes(mBytesRes.getPrefabName());
        }

        override public string getLogicPath()
        {
            if (mBytesRes != null)
            {
                return mBytesRes.getLogicPath();
            }

            return mPath;
        }

        // 同步加载
        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);
                mBytesRes = Ctx.mInstance.mBytesResMgr.getAndSyncLoadRes(path);
                onBytesLoaded(mBytesRes);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, null, luaTable, luaFunction);
                mBytesRes = Ctx.mInstance.mBytesResMgr.getAndSyncLoadRes(path);
                onBytesLoaded(mBytesRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);
                mBytesRes = Ctx.mInstance.mBytesResMgr.getAndAsyncLoadRes(path, onBytesLoaded);
            }
        }

        public void onBytesLoaded(IDispatchObject dispObj)
        {
            mBytesRes = dispObj as BytesRes;
            if (mBytesRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mBytesRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.mInstance.mBytesResMgr.unload(mBytesRes.getResUniqueId(), onBytesLoaded);
                mBytesRes = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(mBytesRes != null)
            {
                Ctx.mInstance.mBytesResMgr.unload(mBytesRes.getResUniqueId(), onBytesLoaded);
                mBytesRes = null;
            }

            base.unload();
        }
    }
}