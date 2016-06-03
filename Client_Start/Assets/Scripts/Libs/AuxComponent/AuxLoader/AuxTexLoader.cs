using UnityEngine;

namespace SDK.Lib
{
    public class AuxTexLoader : AuxLoaderBase
    {
        protected TextureRes mTextureRes;       // 纹理资源
        protected Texture mTexture;

        public AuxTexLoader()
        {
            mTextureRes = null;
            mTexture = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public Texture getTexture()
        {
            return mTexture;
        }

        override public string getLogicPath()
        {
            if (mTexture != null)
            {
                return mTextureRes.getLogicPath();
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
                mTextureRes = Ctx.m_instance.m_texMgr.getAndAsyncLoadRes(path, onTexLoaded);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            mTextureRes = dispObj as TextureRes;
            if (mTextureRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
                this.mTexture = mTextureRes.getTexture();
            }
            else if (mTextureRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_texMgr.unload(mTextureRes.getResUniqueId(), onTexLoaded);
                mTextureRes = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(mTextureRes != null)
            {
                Ctx.m_instance.m_texMgr.unload(mTextureRes.getResUniqueId(), onTexLoaded);
                mTextureRes = null;
            }

            base.unload();
        }
    }
}