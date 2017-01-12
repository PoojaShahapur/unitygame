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
            return this.mSpriteAtlasRes.getSprite(spriteName);
        }

        override public string getLogicPath()
        {
            if (this.mSpriteAtlasRes != null)
            {
                return this.mSpriteAtlasRes.getLogicPath();
            }

            return this.mPath;
        }

        // 同步加载对象
        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            base.syncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndSyncLoadRes(path, null);
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.syncLoad(path, luaTable, luaFunction);

            if (this.isInvalid())
            {
                this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndSyncLoadRes(path, null);
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            base.asyncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, this.onTexLoaded);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mSpriteAtlasRes);
            }
        }

        override public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.asyncLoad(path, luaTable, luaFunction);

            if (this.isInvalid())
            {
                this.mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, this.onTexLoaded);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mSpriteAtlasRes);
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