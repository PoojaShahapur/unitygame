using UnityEngine;

namespace SDK.Lib
{
    public class AuxTextureLoader : AuxLoaderBase
    {
        protected TextureRes mTextureRes;       // 纹理资源
        protected Texture mTexture;

        public AuxTextureLoader(string path = "")
            : base(path)
        {
            this.mTextureRes = null;
            this.mTexture = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public Texture getTexture()
        {
            return this.mTexture;
        }

        override public string getLogicPath()
        {
            if (this.mTexture != null)
            {
                return this.mTextureRes.getLogicPath();
            }

            return this.mPath;
        }

        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            base.syncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                this.mTextureRes = Ctx.mInstance.mTexMgr.getAndSyncLoadRes(path, null);
                this.onTexLoaded(this.mTextureRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextureRes);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.syncLoad(path, luaTable, luaFunction);

            if (this.isInvalid())
            {
                this.mTextureRes = Ctx.mInstance.mTexMgr.getAndSyncLoadRes(path, null, null);
                this.onTexLoaded(this.mTextureRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextureRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            base.asyncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                this.mTextureRes = Ctx.mInstance.mTexMgr.getAndAsyncLoadRes(path, this.onTexLoaded);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextureRes);
            }
        }

        override public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.asyncLoad(path, luaTable, luaFunction);

            if (this.isInvalid())
            {
                this.mTextureRes = Ctx.mInstance.mTexMgr.getAndAsyncLoadRes(path, this.onTexLoaded);
            }
            else if (this.hasLoadEnd())
            {
                this.onTexLoaded(this.mTextureRes);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mTextureRes = dispObj as TextureRes;

                if (this.mTextureRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                    this.mTexture = mTextureRes.getTexture();
                }
                else if (this.mTextureRes.hasFailed())
                {
                    this.mResLoadState.setFailed();
                    Ctx.mInstance.mTexMgr.unload(this.mTextureRes.getResUniqueId(), this.onTexLoaded);
                    this.mTextureRes = null;
                }
            }

            if (null != this.mEvtHandle)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(null != this.mTextureRes)
            {
                Ctx.mInstance.mTexMgr.unload(mTextureRes.getResUniqueId(), this.onTexLoaded);
                this.mTextureRes = null;
            }

            base.unload();
        }
    }
}