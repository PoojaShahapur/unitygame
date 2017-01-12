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
            base.syncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                this.mBytesRes = Ctx.mInstance.mBytesResMgr.getAndSyncLoadRes(path, null);
                this.onBytesLoaded(this.mBytesRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onBytesLoaded(this.mBytesRes);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.syncLoad(path, luaTable, luaFunction);

            if (this.isInvalid())
            {
                this.mBytesRes = Ctx.mInstance.mBytesResMgr.getAndSyncLoadRes(path, null);
                this.onBytesLoaded(this.mBytesRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onBytesLoaded(this.mBytesRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            base.asyncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                mBytesRes = Ctx.mInstance.mBytesResMgr.getAndAsyncLoadRes(path, onBytesLoaded);
            }
            else if (this.hasLoadEnd())
            {
                this.onBytesLoaded(this.mBytesRes);
            }
        }

        override public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.asyncLoad(path, luaTable, luaFunction);

            if (this.isInvalid())
            {
                mBytesRes = Ctx.mInstance.mBytesResMgr.getAndAsyncLoadRes(path, luaTable, luaFunction);
            }
            else if (this.hasLoadEnd())
            {
                this.onBytesLoaded(this.mBytesRes);
            }
        }

        public void onBytesLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mBytesRes = dispObj as BytesRes;

                if (this.mBytesRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else if (this.mBytesRes.hasFailed())
                {
                    this.mResLoadState.setFailed();
                    Ctx.mInstance.mBytesResMgr.unload(this.mBytesRes.getResUniqueId(), this.onBytesLoaded);
                    this.mBytesRes = null;
                }
            }

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(this.mBytesRes != null)
            {
                Ctx.mInstance.mBytesResMgr.unload(this.mBytesRes.getResUniqueId(), this.onBytesLoaded);
                this.mBytesRes = null;
            }

            base.unload();
        }
    }
}