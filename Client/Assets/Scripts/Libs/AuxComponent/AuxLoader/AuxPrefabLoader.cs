using LuaInterface;
using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 移动
     */
    public class AuxPrefabLoader : AuxLoaderBase
    {
        protected GameObject mSelfGo;                       // 加载的 GameObject
        protected PrefabRes mPrefabRes;                     // 预制资源
        protected ResInsEventDispatch mResInsEventDispatch; // 实例化的时候使用的分发器
        protected bool mIsInsNeedCoroutine; // 实例化是否需要协程

        public AuxPrefabLoader(bool isInsNeedCoroutine = true)
            : base()
        {
            mIsInsNeedCoroutine = isInsNeedCoroutine;
        }

        override public void dispose()
        {
            if(this.selfGo != null)
            {
                UtilApi.Destroy(this.selfGo);
            }
            base.dispose();
        }

        public GameObject selfGo
        {
            get
            {
                return mSelfGo;
            }
            set
            {
                mSelfGo = value;
            }
        }

        override public string getLogicPath()
        {
            if (mPrefabRes != null)
            {
                return mPrefabRes.getLogicPath();
            }

            return mPath;
        }

        override public void syncLoad(string path)
        {
            if(mPath != path && !string.IsNullOrEmpty(path))
            {
                unload();
                mPath = path;
                mPrefabRes = Ctx.m_instance.m_prefabMgr.getAndSyncLoadRes(path);
                onPrefabLoaded(mPrefabRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, Action<IDispatchObject> dispObj)
        {
            this.setPath(path);

            if(this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, dispObj);
                mPrefabRes = Ctx.m_instance.m_prefabMgr.getAndAsyncLoadRes(path, onPrefabLoaded);
            }
        }

        override public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, null, luaTable, luaFunction);
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
                    mResInsEventDispatch.addEventHandle(null, onPrefabIns);
                    mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mResInsEventDispatch);
                }
                else
                {
                    this.selfGo = mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName());
                    onAllFinish();
                }
            }
            else if (mPrefabRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.getResUniqueId(), onPrefabLoaded);
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

        override public void unload()
        {
            if(mPrefabRes != null)
            {
                Ctx.m_instance.m_prefabMgr.unload(mPrefabRes.getResUniqueId(), onPrefabLoaded);
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

        public GameObject getGameObject()
        {
            return this.mSelfGo;
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}