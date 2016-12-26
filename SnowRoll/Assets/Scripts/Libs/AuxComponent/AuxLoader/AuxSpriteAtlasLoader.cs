namespace SDK.Lib
{
    public class AuxSpriteAtlasLoader : AuxLoaderBase
    {
        protected SpriteAtlasRes mSpriteAtlasRes;

        public AuxSpriteAtlasLoader(string path = "")
            : base(path)
        {
            mSpriteAtlasRes = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public UnityEngine.Sprite getSprite(string spriteName)
        {
            return mSpriteAtlasRes.getSprite(spriteName);
        }

        override public string getLogicPath()
        {
            if (mSpriteAtlasRes != null)
            {
                return mSpriteAtlasRes.getLogicPath();
            }

            return mPath;
        }

        // 同步加载对象
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
                mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndSyncLoadRes(path, null);

                onTexLoaded(mSpriteAtlasRes);
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
                mSpriteAtlasRes = Ctx.mInstance.mSpriteMgr.getAndAsyncLoadRes(path, onTexLoaded);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            mSpriteAtlasRes = dispObj as SpriteAtlasRes;
            if (mSpriteAtlasRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mSpriteAtlasRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.mInstance.mSpriteMgr.unload(mSpriteAtlasRes.getResUniqueId(), onTexLoaded);
                mSpriteAtlasRes = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(mSpriteAtlasRes != null)
            {
                Ctx.mInstance.mTexMgr.unload(mSpriteAtlasRes.getResUniqueId(), onTexLoaded);
                mSpriteAtlasRes = null;
            }

            base.unload();
        }
    }
}