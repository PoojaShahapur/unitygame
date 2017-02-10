namespace SDK.Lib
{
    public class AuxSpriteAtlasLoader : AuxLoaderBase
    {
        protected SpriteAtlasRes mSpriteAtlasRes;

        public AuxSpriteAtlasLoader(string path = "")
            : base(path)
        {
            this.mSpriteAtlasRes = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public UnityEngine.Sprite getSprite(string spriteName)
        {
            if (null != this.mSpriteAtlasRes)
            {
                return this.mSpriteAtlasRes.getSprite(spriteName);
            }

            return null;
        }

        override public string getLogicPath()
        {
            if (null != this.mSpriteAtlasRes)
            {
                return this.mSpriteAtlasRes.getLogicPath();
            }

            return this.mPath;
        }

        // 同步加载对象
        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null)
        {
            base.syncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndSyncLoadRes(path, null, null);
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
            else if (this.hasLoadEnd())
            {
                //this.onTexLoaded(this.mSpriteAtlasRes);
                this.onTexLoaded(null);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.syncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndSyncLoadRes(path, null, null);
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
            else if (this.hasLoadEnd())
            {
                //this.onTexLoaded(this.mSpriteAtlasRes);
                this.onTexLoaded(null);
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
                    this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, this.onTexLoaded, null);
                }
                else
                {
                    this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, this.onTexLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                //this.onTexLoaded(this.mSpriteAtlasRes);
                this.onTexLoaded(null);
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
                    this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, this.onTexLoaded, null);
                }
                else
                {
                    this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, this.onTexLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                //this.onTexLoaded(this.mSpriteAtlasRes);
                this.onTexLoaded(null);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mSpriteAtlasRes = dispObj as SpriteAtlasRes;

                if (this.mSpriteAtlasRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else if (this.mSpriteAtlasRes.hasFailed())
                {
                    this.mResLoadState.setFailed();
                    Ctx.mInstance.mSpriteMgr.unload(this.mSpriteAtlasRes.getResUniqueId(), this.onTexLoaded);
                    this.mSpriteAtlasRes = null;
                }
            }

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(this.mSpriteAtlasRes != null)
            {
                Ctx.mInstance.mTexMgr.unload(this.mSpriteAtlasRes.getResUniqueId(), this.onTexLoaded);
                this.mSpriteAtlasRes = null;
            }

            base.unload();
        }
    }
}