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

        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null)
        {
            base.syncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mTextureRes = Ctx.mInstance.mTexMgr.getAndSyncLoadRes(path, null, null);
                this.onTextureLoaded(this.mTextureRes);
            }
            else if (this.hasLoadEnd())
            {
                //this.onTextureLoaded(this.mTextureRes);
                this.onTextureLoaded(null);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.syncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mTextureRes = Ctx.mInstance.mTexMgr.getAndSyncLoadRes(path, null, null);
                this.onTextureLoaded(this.mTextureRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onTextureLoaded(null);
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
                    this.mTextureRes = Ctx.mInstance.mTexMgr.getAndAsyncLoadRes(path, this.onTextureLoaded, null);
                }
                else
                {
                    this.mTextureRes = Ctx.mInstance.mTexMgr.getAndAsyncLoadRes(path, this.onTextureLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                this.onTextureLoaded(null);
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
                    this.mTextureRes = Ctx.mInstance.mTexMgr.getAndAsyncLoadRes(path, this.onTextureLoaded, null);
                }
                else
                {
                    this.mTextureRes = Ctx.mInstance.mTexMgr.getAndAsyncLoadRes(path, this.onTextureLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                this.onTextureLoaded(null);
            }
        }

        public void onTextureLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mTextureRes = dispObj as TextureRes;

                if (this.mTextureRes.hasSuccessLoaded())
                {
                    this.onLoaded();

                    this.mTexture = mTextureRes.getTexture();
                }
                else if (this.mTextureRes.hasFailed())
                {
                    this.onFailed();

                    Ctx.mInstance.mTexMgr.unload(this.mTextureRes.getResUniqueId(), this.onTextureLoaded);
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
                Ctx.mInstance.mTexMgr.unload(mTextureRes.getResUniqueId(), this.onTextureLoaded);
                this.mTextureRes = null;
            }

            base.unload();
        }

        public static AuxTextureLoader newObject(string path = "")
        {
            AuxTextureLoader ret = null;
            ret = AuxTextureLoader.getObject(path) as AuxTextureLoader;

            if (null == ret)
            {
                ret = new AuxTextureLoader(path);
            }

            return ret;
        }
    }
}