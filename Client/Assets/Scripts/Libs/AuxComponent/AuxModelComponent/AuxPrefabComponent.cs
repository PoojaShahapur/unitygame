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
        protected string mPath;

        public AuxPrefabComponent()
            : base(null)
        {
            mIsSuccess = false;
            mPath = "";
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

        public string GetPath()
        {
            if(mPrefabRes != null)
            {
                return mPrefabRes.GetPath();
            }

            return mPath;
        }

        // 异步加载对象
        public void asyncLoad(string path, Action<IDispatchObject> dispObj)
        {
            if(mPath != path)
            {
                unload();
                mPath = path;

                mEvtHandle = dispObj;
                mPrefabRes = Ctx.m_instance.m_prefabMgr.getAndAsyncLoadRes(path, onPrefabLoaded);
            }
        }

        public void onPrefabLoaded(IDispatchObject dispObj)
        {
            // 一定要从这里再次取值，因为如果这个资源已经加载，可能在返回之前就先调用这个函数，因此这个时候 mPrefabRes 还是空值
            mPrefabRes = dispObj as PrefabRes;
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
                mPrefabRes = null;

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