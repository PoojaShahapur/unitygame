namespace SDK.Lib
{
    public class AuxTextLoader : AuxLoaderBase
    {
        protected TextRes mTextRes;

        public AuxTextLoader(string path = "")
            : base(path)
        {
            mTextRes = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public string getText()
        {
            return mTextRes.getText(mTextRes.getPrefabName());
        }

        override public string getLogicPath()
        {
            if (mTextRes != null)
            {
                return mTextRes.getLogicPath();
            }

            return mPath;
        }

        // 同步加载
        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            if(needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);
                mTextRes = Ctx.mInstance.mTextResMgr.getAndSyncLoadRes(path);

                onTexLoaded(mTextRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);
                mTextRes = Ctx.mInstance.mTextResMgr.getAndAsyncLoadRes(path, onTexLoaded);
            }
        }

        public void onTexLoaded(IDispatchObject dispObj)
        {
            mTextRes = dispObj as TextRes;
            if (mTextRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mTextRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.mInstance.mTexMgr.unload(mTextRes.getResUniqueId(), onTexLoaded);
                mTextRes = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(mTextRes != null)
            {
                Ctx.mInstance.mTexMgr.unload(mTextRes.getResUniqueId(), onTexLoaded);
                mTextRes = null;
            }

            base.unload();
        }
    }
}