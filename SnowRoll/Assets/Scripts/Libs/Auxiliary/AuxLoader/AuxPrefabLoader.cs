using LuaInterface;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预制
     */
    public class AuxPrefabLoader : AuxLoaderBase
    {
        protected GameObject mSelfGo;                       // 加载的 GameObject
        protected PrefabRes mPrefabRes;                     // 预制资源
        protected ResInsEventDispatch mResInsEventDispatch; // 实例化的时候使用的分发器
        protected bool mIsInsNeedCoroutine; // 实例化是否需要协程
        protected bool mIsDestroySelf;      // 是否释放自己
        protected bool mIsNeedInsPrefab;    // 是否需要实例化预制

        protected bool mIsSetFakePos;       // 是否初始化的时候设置到很远的位置
        protected bool mIsSetInitOrientPos; // 是否 Instantiate 的时候，设置初始化方向位置信息， UI 是不需要的，UI 的初始化信息都保存在 Prefab 里面，直接从 Prefab 里面读取就行了，如果设置了不对的位置信息，可能位置就不对了
        protected ResInsEventDispatch mInsEventDispatch;

        public AuxPrefabLoader(string path = "")
            : base(path)
        {
            this.mTypeId = "AuxPrefabLoader";

            this.mIsInsNeedCoroutine = true;
            this.mIsDestroySelf = true;
            this.mIsNeedInsPrefab = true;

            this.mIsSetInitOrientPos = false;
            this.mIsSetFakePos = false;
        }

        // 是否需要实例化预制
        public void setIsNeedInsPrefab(bool value)
        {
            this.mIsNeedInsPrefab = value;
        }

        // 是否需要协程实例化预制
        public void setIsInsNeedCoroutine(bool value)
        {
            this.mIsInsNeedCoroutine = value;
        }

        public void setIsInitOrientPos(bool isSet)
        {
            this.mIsSetInitOrientPos = isSet;
        }

        public void setIsFakePos(bool isSet)
        {
            this.mIsSetFakePos = isSet;
        }

        override public void dispose()
        {
            Ctx.mInstance.mDelayTaskMgr.removeTask(this);

            if (this.mIsDestroySelf)
            {
                if (this.mSelfGo != null)
                {
                    UtilApi.DestroyImmediate(this.mSelfGo);
                }
            }

            base.dispose();
        }

        public GameObject selfGo
        {
            get
            {
                return this.mSelfGo;
            }
            set
            {
                this.mSelfGo = value;
            }
        }

        public bool isDestroySelf()
        {
            return this.mIsDestroySelf;
        }

        public void setDestroySelf(bool value)
        {
            this.mIsDestroySelf = value;
        }

        override public string getLogicPath()
        {
            if (this.mPrefabRes != null)
            {
                return this.mPrefabRes.getLogicPath();
            }

            return this.mPath;
        }

        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null, MAction<IDispatchObject> progressHandle = null)
        {
            base.syncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndSyncLoadRes(path, null, null);
                this.onPrefabLoaded(this.mPrefabRes);
            }
            else if (this.hasLoadEnd())
            {
                //this.onPrefabLoaded(mPrefabRes);  // 手工触发
                this.onPrefabLoaded(null);
            }
        }

        override public void syncLoad(string path, LuaTable luaTable, LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.syncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.onStartLoad();

                this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndSyncLoadRes(path, null, null);
                this.onPrefabLoaded(this.mPrefabRes);
            }
            else if (this.hasLoadEnd())
            {
                //this.onPrefabLoaded(this.mPrefabRes);       // 手工触发
                this.onPrefabLoaded(null);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle, MAction<IDispatchObject> progressHandle = null)
        {
            base.asyncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
                this.onStartLoad();

                if (null == progressHandle)
                {
                    this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, this.onPrefabLoaded, null);
                }
                else
                {
                    this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, this.onPrefabLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                //this.onPrefabLoaded(this.mPrefabRes);
                this.onPrefabLoaded(null);
            }
        }

        override public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.asyncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.onStartLoad();

                if (null == progressLuaFunction)
                {
                    this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, this.onPrefabLoaded, null);
                }
                else
                {
                    this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, this.onPrefabLoaded, this.onProgressEventHandle);
                }
            }
            else if (this.hasLoadEnd())
            {
                //this.onPrefabLoaded(this.mPrefabRes);
                this.onPrefabLoaded(null);
            }
        }

        public void onPrefabLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)    // 说明是资源加载完成后的回调
            {
                // 一定要从这里再次取值，因为如果这个资源已经加载，可能在返回之前就先调用这个函数，因此这个时候 mPrefabRes 还是空值
                this.mPrefabRes = dispObj as PrefabRes;

                if (this.mPrefabRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();

                    if (this.mIsNeedInsPrefab)
                    {
                        if (this.mIsInsNeedCoroutine)
                        {
                            // this.asyncIns(this.onPrefabIns);

                            Ctx.mInstance.mDelayTaskMgr.addTask(this);
                        }
                        else
                        {
                            this.syncIns();
                            this.onAllFinish();
                        }
                    }
                    else
                    {
                        this.onAllFinish();
                    }
                }
                else if (this.mPrefabRes.hasFailed())
                {
                    this.mResLoadState.setFailed();

                    Ctx.mInstance.mPrefabMgr.unload(this.mPrefabRes.getResUniqueId(), this.onPrefabLoaded);
                    this.mPrefabRes = null;

                    if (this.mEvtHandle != null)
                    {
                        this.mEvtHandle.dispatchEvent(this);
                    }
                }
            }
            else        // 这个是手工触发的事件加载完成的处理，资源早就加载完成了
            {
                if (this.mEvtHandle != null)
                {
                    this.mEvtHandle.dispatchEvent(this);
                }
            }
        }

        public void onPrefabIns(IDispatchObject dispObj)
        {
            this.mResInsEventDispatch = dispObj as ResInsEventDispatch;
            this.selfGo = this.mResInsEventDispatch.getInsGO();

            this.onAllFinish();
        }

        // 异步实例化
        protected void asyncIns(MAction<IDispatchObject> handle)
        {
            if (this.mIsNeedInsPrefab && null == this.mSelfGo && this.mIsInsNeedCoroutine && null != this.mPrefabRes)
            {
                if (null != this.mResInsEventDispatch)
                {
                    this.mResInsEventDispatch = new ResInsEventDispatch();
                }
                this.mResInsEventDispatch.addEventHandle(null, handle);

                if (this.mIsSetFakePos)
                {
                    this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat, this.mResInsEventDispatch);
                }
                else
                {
                    this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat, this.mResInsEventDispatch);
                }
            }
        }

        // 同步实例化
        protected void syncIns()
        {
            if (this.mIsNeedInsPrefab && null == this.mSelfGo && null != this.mPrefabRes)
            {
                if (this.mIsSetFakePos)
                {
                    this.selfGo = this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat);
                }
                else
                {
                    this.selfGo = this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat);
                }
            }
        }

        override public void delayExec()
        {
            this.syncIns();
            this.onAllFinish();
        }

        protected void checkLoadState()
        {
            if (this.mIsNeedInsPrefab)
            {
                if (null != this.mPrefabRes && this.mPrefabRes.hasSuccessLoaded() && null != this.mSelfGo)
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mResLoadState.setFailed();
                }
            }
            else
            {
                if (null != this.mPrefabRes && this.mPrefabRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mResLoadState.setFailed();
                }
            }
        }

        // 所有的资源都加载完成
        public void onAllFinish()
        {
            this.checkLoadState();

            if (null != this.mEvtHandle)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(this.mPrefabRes != null)
            {
                Ctx.mInstance.mPrefabMgr.unload(this.mPrefabRes.getResUniqueId(), this.onPrefabLoaded);
                this.mPrefabRes = null;
            }

            if(this.mResInsEventDispatch != null)
            {
                this.mResInsEventDispatch.setIsValid(false);
                this.mResInsEventDispatch = null;
            }

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.clearEventHandle();
                this.mEvtHandle = null;
            }
        }

        public GameObject getGameObject()
        {
            return this.mSelfGo;
        }

        // 获取预制模板
        public GameObject getPrefabTmpl()
        {
            GameObject ret = null;
            if(null != this.mPrefabRes)
            {
                ret = this.mPrefabRes.getObject();
            }
            return ret;
        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public UnityEngine.GameObject InstantiateObject(MAction<IDispatchObject> insHandle = null)
        {
            if(null == this.mInsEventDispatch && null != insHandle)
            {
                this.mInsEventDispatch = new ResInsEventDispatch();
            }
            if(null != insHandle)
            {
                this.mInsEventDispatch.addEventHandle(null, insHandle);
            }

            if (this.mIsInsNeedCoroutine)
            {
                this.asyncIns(this.onInstantiateObjectFinish);
            }
            else
            {
                this.syncIns();

                this.onInstantiateObjectFinish();
            }

            return this.selfGo;
        }

        public void onInstantiateObjectFinish(IDispatchObject dispObj = null)
        {
            if(null != dispObj)
            {
                this.selfGo = this.mResInsEventDispatch.getInsGO();
            }

            if (null != this.mInsEventDispatch)
            {
                this.mInsEventDispatch.dispatchEvent(this);
            }
        }

        public static AuxPrefabLoader newObject(string path = "")
        {
            AuxPrefabLoader ret = null;
            ret = AuxLoaderBase.getObject(path) as AuxPrefabLoader;

            if(null == ret)
            {
                ret = new AuxPrefabLoader(path);
            }

            return ret;
        }

        override protected void onGetPool()
        {
            base.onGetPool();

            if (this.mIsNeedInsPrefab && null == this.mSelfGo && null != this.mPrefabRes && this.mPrefabRes.hasSuccessLoaded())
            {
                Ctx.mInstance.mDelayTaskMgr.addTask(this);
            }

            if (null != this.mSelfGo)
            {
                if (this.mSelfGo.name.Length >= UtilApi.POOL_SUFFIX.Length)
                {
                    if (this.mSelfGo.name.Substring(this.mSelfGo.name.Length - UtilApi.POOL_SUFFIX.Length, UtilApi.POOL_SUFFIX.Length) == UtilApi.POOL_SUFFIX)
                    {
                        UtilApi.setGOName(this.mSelfGo, this.mSelfGo.name.Substring(0, this.mSelfGo.name.Length - UtilApi.POOL_SUFFIX.Length));
                    }
                }
            }
        }

        override protected void onRetPool()
        {
            base.onRetPool();

            Ctx.mInstance.mDelayTaskMgr.removeTask(this);

            // 清理所有的事件，不能清理这个事件，如果清理掉，加入正在实例化，还没返回，就得不到这个实例化对象了
            //if(null != this.mResInsEventDispatch)
            //{
            //    this.mResInsEventDispatch.clearEventHandle();
            //}
            if (null != this.mInsEventDispatch)
            {
                this.mInsEventDispatch.clearEventHandle();
            }

            if (null != this.mSelfGo)
            {
                UtilApi.setActorPos(this.mSelfGo, UtilApi.FAKE_POS);
                UtilApi.setGOName(this.mSelfGo, this.mSelfGo.name + UtilApi.POOL_SUFFIX);
            }
        }
    }
}