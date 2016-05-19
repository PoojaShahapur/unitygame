using System;

namespace SDK.Lib
{
    public class AuxTextLoader : AuxLoaderBase
    {
        protected TextRes mTextRes;

        public AuxTextLoader()
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

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> dispObj)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, dispObj);
                mTextRes = Ctx.m_instance.m_textResMgr.getAndAsyncLoadRes(path, onTexLoaded);
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
                Ctx.m_instance.m_texMgr.unload(mTextRes.getResUniqueId(), onTexLoaded);
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
                Ctx.m_instance.m_texMgr.unload(mTextRes.getResUniqueId(), onTexLoaded);
                mTextRes = null;
            }

            base.unload();
        }
    }
}