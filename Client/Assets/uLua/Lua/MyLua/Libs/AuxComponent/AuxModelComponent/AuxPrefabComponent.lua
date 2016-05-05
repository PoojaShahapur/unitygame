using System;

namespace SDK.Lib
{
    /**
     * @brief 移动
     */
    public class AuxPrefabComponent : AuxComponent
    {
        protected PrefabRes mPrefabRes;                     // 预制资源
        protected ResEventDispatch mEvtHandle;              // 事件分发器
        protected ResInsEventDispatch mResInsEventDispatch; // 实例化的时候使用的分发器
        protected bool mIsSuccess;      // 是否成功
        protected string mPath;         // 加载的资源目录
        protected bool mIsInsNeedCoroutine; // 实例化是否需要协程

        public AuxPrefabComponent(bool isInsNeedCoroutine = true)
            : base(null)
        {
            mIsInsNeedCoroutine = isInsNeedCoroutine;
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
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(dispObj);
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
                if (mIsInsNeedCoroutine)
                {
                    mResInsEventDispatch = new ResInsEventDispatch();
                    mResInsEventDispatch.addEventHandle(onPrefabIns);
                    mPrefabRes.InstantiateObject(mPrefabRes.GetPath(), mResInsEventDispatch);
                }
                else
                {
                    this.selfGo = mPrefabRes.InstantiateObject(mPrefabRes.GetPath());
                    onAllFinish();
                }
            }
            else if (mPrefabRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.GetPath(), onPrefabLoaded);
                mPrefabRes = null;

                if (mEvtHandle != null)
                {
                    mEvtHandle.dispatchEvent(this);
                }
            }
        }

        public void onPrefabIns(IDispatchObject dispObj)
        {
            mResInsEventDispatch = dispObj as ResInsEventDispatch;
            this.selfGo = mResInsEventDispatch.getInsGO();
            onAllFinish();
        }

        // 所有的资源都加载完成
        public void onAllFinish()
        {
            if (this.selfGo != null)
            {
                mIsSuccess = true;
            }
            else
            {
                mIsSuccess = false;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
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

            if (mEvtHandle != null)
            {
                mEvtHandle.clearEventHandle();
                mEvtHandle = null;
            }
        }
    }
}