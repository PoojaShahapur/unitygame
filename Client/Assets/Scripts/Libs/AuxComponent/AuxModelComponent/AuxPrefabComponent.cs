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

        public AuxPrefabComponent()
            : base(null)
        {
            
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
                mResInsEventDispatch = new ResInsEventDispatch();
                mResInsEventDispatch.addEventHandle(onPrefabIns);
                mPrefabRes.InstantiateObject(mPrefabRes.GetPath(), mResInsEventDispatch);
                mResInsEventDispatch.setIsValid(false);
            }
            else if (mPrefabRes.hasFailed())
            {
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.GetPath(), onPrefabLoaded);
            }
        }

        public void onPrefabIns(IDispatchObject dispObj)
        {
            this.selfGo = mResInsEventDispatch.getInsGO();
            if(mEvtHandle != null)
            {
                mEvtHandle(dispObj);
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