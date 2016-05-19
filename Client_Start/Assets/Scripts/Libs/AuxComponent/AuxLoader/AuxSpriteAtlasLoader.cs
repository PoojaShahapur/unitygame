using System;

namespace SDK.Lib
{
    public class AuxSpriteAtlasLoader : AuxLoaderBase
    {
        protected SpriteAtlasRes mSpriteAtlasRes;

        public AuxSpriteAtlasLoader()
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

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> dispObj)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, dispObj);
                mSpriteAtlasRes = Ctx.m_instance.mSpriteMgr.getAndAsyncLoadRes(path, onTexLoaded);
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
                Ctx.m_instance.mSpriteMgr.unload(mSpriteAtlasRes.getResUniqueId(), onTexLoaded);
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
                Ctx.m_instance.m_texMgr.unload(mSpriteAtlasRes.getResUniqueId(), onTexLoaded);
                mSpriteAtlasRes = null;
            }

            base.unload();
        }
    }
}