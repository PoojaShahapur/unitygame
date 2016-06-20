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

        // 同步加载
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
                mBytesRes = Ctx.m_instance.m_bytesResMgr.getAndSyncLoadRes(path);
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
                mBytesRes = Ctx.m_instance.m_bytesResMgr.getAndAsyncLoadRes(path, onBytesLoaded);
            }
        }

        public void onBytesLoaded(IDispatchObject dispObj)
        {
            mBytesRes = dispObj as BytesRes;
            if (mBytesRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mBytesRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_bytesResMgr.unload(mBytesRes.getResUniqueId(), onBytesLoaded);
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
                Ctx.m_instance.m_bytesResMgr.unload(mBytesRes.getResUniqueId(), onBytesLoaded);
                mBytesRes = null;
            }

            base.unload();
        }
    }
}