namespace SDK.Lib
{
    public class AuxTextLoader : AuxLoaderBase
    {
        protected TextRes mTextRes;

        public AuxTextLoader(string path = "")
            : base(path)
        {
            this.mTextRes = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public string getText()
        {
            return this.mTextRes.getText(this.mTextRes.getPrefabName());
        }

        override public string getLogicPath()
        {
            if (this.mTextRes != null)
            {
                return this.mTextRes.getLogicPath();
            }

            return this.mPath;
        }

        // 同步加载
        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null)
        {
            base.syncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                this.mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(path, null, null);
                this.onTexLoaded(this.mTextRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextRes);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.syncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(path, null, null);
                this.onTexLoaded(this.mTextRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle, MAction<IDispatchObject> progressHandle = null)
        {
            base.asyncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                if (null == progressHandle)
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTexLoaded, null);
                }
                else
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTexLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextRes);
            }
        }

        override public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.asyncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                if (null == progressLuaFunction)
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTexLoaded, null);
                }
                else
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTexLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextRes);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mTextRes = dispObj as TextRes;

                if (this.mTextRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else if (this.mTextRes.hasFailed())
                {
                    this.mResLoadState.setFailed();
                    Ctx.mInstance.mTexMgr.unload(this.mTextRes.getResUniqueId(), this.onTexLoaded);
                    this.mTextRes = null;
                }
            }

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(this.mTextRes != null)
            {
                Ctx.mInstance.mTexMgr.unload(this.mTextRes.getResUniqueId(), this.onTexLoaded);
                this.mTextRes = null;
            }

            base.unload();
        }
    }
}