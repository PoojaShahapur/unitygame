using System;
using UnityEngine;

namespace SDK.Lib
{
    public class AuxTexComponent : IDispatchObject
    {
        protected TextureRes mTextureRes;       // 纹理资源
        protected ResEventDispatch mEvtHandle;   // 事件分发器
        protected bool mIsSuccess;              // 是否成功
        protected string mPath;                 // 加载的资源目录
        protected Texture mTexture;

        public AuxTexComponent()
        {
            mIsSuccess = false;
            mPath = "";
        }

        public void dispose()
        {
            unload();
        }

        public Texture getTexture()
        {
            return mTexture;
        }

        public bool hasSuccessLoaded()
        {
            return mIsSuccess;
        }

        public bool hasFailed()
        {
            return !mIsSuccess;
        }

        //public string GetPath()
        //{
        //    if(mTexture != null)
        //    {
        //        return mTextureRes.GetPath();
        //    }

        //    return mPath;
        //}

        // 异步加载对象
        public void asyncLoad(string path, Action<IDispatchObject> dispObj)
        {
            if (mPath != path)
            {
                unload();
                mPath = path;
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(dispObj);
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
                onAllFinish();
            }
            else if (mTextureRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_texMgr.unload(mTextureRes.getResUniqueId(), onTexLoaded);
                mTextureRes = null;

                if (mEvtHandle != null)
                {
                    mEvtHandle.dispatchEvent(this);
                }
            }
        }

        // 所有的资源都加载完成
        public void onAllFinish()
        {
            if(this.mTexture != null)
            {
                mIsSuccess = true;
            }
            else
            {
                mIsSuccess = false;

            }

            if(mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        public void unload()
        {
            if(mTextureRes != null)
            {
                Ctx.m_instance.m_texMgr.unload(mTextureRes.getResUniqueId(), onTexLoaded);
                mTextureRes = null;
            }

            if(mEvtHandle != null)
            {
                mEvtHandle.clearEventHandle();
                mEvtHandle = null;
            }
        }
    }
}