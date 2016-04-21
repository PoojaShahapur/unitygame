using System;

namespace SDK.Lib
{
    /**
     * @brief 移动
     */
    public class AuxPrefabComponent : AuxComponent
    {
        protected PrefabRes mPrefabRes;
        protected Action<IDispatchObject> mEvtHandle;
        protected ResInsEventDispatch mResInsEventDispatch;
        protected bool mIsSuccess;      // 是否成功

        public AuxPrefabComponent()
            : base(null)
        {
            mIsSuccess = false;
        }

        override public void dispose()
        {
            unload();
            if(this.selfGo != null)
            {
                UtilApi.Destroy(this.selfGo);
            }
            base.dispose();
        }

        public bool hasSuccessLoaded()
        {
            return mIsSuccess;
        }

        public bool hasFailed()
        {
            return !mIsSuccess;
        }

        // 异步加载对象
        public void asyncLoad(string path, Action<IDispatchObject> dispObj)
        {
            mEvtHandle = dispObj;
            mPrefabRes = Ctx.m_instance.m_prefabMgr.getAndAsyncLoadRes(path, onPrefabLoaded);
        }

        public void onPrefabLoaded(IDispatchObject dispObj)
        {
            //mPrefabRes = dispObj as PrefabRes;
            if(mPrefabRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
                mResInsEventDispatch = new ResInsEventDispatch();
                mResInsEventDispatch.addEventHandle(onPrefabIns);
                mPrefabRes.InstantiateObject(mPrefabRes.GetPath(), mResInsEventDispatch);
            }
            else if (mPrefabRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.GetPath(), onPrefabLoaded);

                if (mEvtHandle != null)
                {
                    mEvtHandle(this);
                }
            }
        }

        public void onPrefabIns(IDispatchObject dispObj)
        {
            mResInsEventDispatch = dispObj as ResInsEventDispatch;
            this.selfGo = mResInsEventDispatch.getInsGO();
            if(this.selfGo != null)
            {
                mIsSuccess = true;
            }
            else
            {
                mIsSuccess = false;
            }

            if(mEvtHandle != null)
            {
                mEvtHandle(this);
            }
        }

        public void unload()
        {
            if(mPrefabRes != null)
            {
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.GetPath(), onPrefabLoaded);
                mPrefabRes = null;
            }

            if(mResInsEventDispatch != null)
            {
                mResInsEventDispatch.setIsValid(false);
                mResInsEventDispatch = null;
            }

            mEvtHandle = null;
        }
    }
}