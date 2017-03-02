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
                this.onStartLoad();

                this.mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(path, null, null);
                this.onTextLoaded(this.mTextRes);
            }
            else if (this.hasLoadEnd())
            {
                //this.onTextLoaded(this.mTextRes);
                this.onTextLoaded(null);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.syncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(path, null, null);
                this.onTextLoaded(this.mTextRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTextLoaded(null);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle, MAction<IDispatchObject> progressHandle = null)
        {
            base.asyncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                this.onStartLoad();

                if (null == progressHandle)
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTextLoaded, null);
                }
                else
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTextLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                this.onTextLoaded(null);
            }
        }

        override public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.asyncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.onStartLoad();

                if (null == progressLuaFunction)
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTextLoaded, null);
                }
                else
                {
                    this.mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, this.onTextLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                this.onTextLoaded(null);
            }
        }

        public void onTextLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mTextRes = dispObj as TextRes;

                if (this.mTextRes.hasSuccessLoaded())
                {
                    this.onLoaded();
                }
                else if (this.mTextRes.hasFailed())
                {
                    this.onFailed();

                    Ctx.mInstance.mTexMgr.unload(this.mTextRes.getResUniqueId(), this.onTextLoaded);
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
                Ctx.mInstance.mTexMgr.unload(this.mTextRes.getResUniqueId(), this.onTextLoaded);
                this.mTextRes = null;
            }

            base.unload();
        }
    }
}