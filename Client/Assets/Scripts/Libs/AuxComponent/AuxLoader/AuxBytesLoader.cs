using System;

namespace SDK.Lib
{
    public class AuxBytesLoader : AuxLoaderBase
    {
        protected BytesRes mBytesRes;

        public AuxBytesLoader()
        {
            mBytesRes = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public Byte[] getBytes()
        {
            return mBytesRes.getBytes(mBytesRes.getPrefabName());
        }

        override public string getLogicPath()
        {
            if (mBytesRes != null)
            {
                return mBytesRes.getLogicPath();
            }

            return mPath;
        }

        // 异步加载对象
        override public void asyncLoad(string path, Action<IDispatchObject> dispObj)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(dispObj);
                mBytesRes = Ctx.m_instance.m_bytesResMgr.getAndAsyncLoadRes(path, onTexLoaded);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            mBytesRes = dispObj as BytesRes;
            if (mBytesRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mBytesRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_texMgr.unload(mBytesRes.getResUniqueId(), onTexLoaded);
                mBytesRes = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(mBytesRes != null)
            {
                Ctx.m_instance.m_texMgr.unload(mBytesRes.getResUniqueId(), onTexLoaded);
                mBytesRes = null;
            }

            base.unload();
        }
    }
}