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

        public AuxPrefabLoader(string path = "", bool isNeedInsPrefab = true, bool isInsNeedCoroutine = true)
            : base(path)
        {
            this.mTypeId = "AuxPrefabLoader";

            this.mIsInsNeedCoroutine = isInsNeedCoroutine;
            this.mIsDestroySelf = true;
            this.mIsNeedInsPrefab = isNeedInsPrefab;

            this.mIsSetInitOrientPos = false;
            this.mIsSetFakePos = false;
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
                this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndSyncLoadRes(path, null, null);
                this.onPrefabLoaded(mPrefabRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onPrefabLoaded(mPrefabRes);
            }
        }

        override public void syncLoad(string path, LuaTable luaTable, LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.syncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
                this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndSyncLoadRes(path, null, null);
                this.onPrefabLoaded(this.mPrefabRes);
            }
            else if (this.hasLoadEnd())
            {
                this.onPrefabLoaded(this.mPrefabRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle, MAction<IDispatchObject> progressHandle = null)
        {
            base.asyncLoad(path, evtHandle, progressHandle);

            if (this.isInvalid())
            {
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
                this.onPrefabLoaded(this.mPrefabRes);
            }
        }

        override public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            base.asyncLoad(path, luaTable, luaFunction, progressLuaFunction);

            if (this.isInvalid())
            {
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
                this.onPrefabLoaded(this.mPrefabRes);
            }
        }

        public void onPrefabLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
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
                            this.mResInsEventDispatch = new ResInsEventDispatch();
                            this.mResInsEventDispatch.addEventHandle(null, onPrefabIns);

                            if (this.mIsSetFakePos)
                            {
                                this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat, this.mResInsEventDispatch);
                            }
                            else
                            {
                                this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat, this.mResInsEventDispatch);
                            }
                        }
                        else
                        {
                            if (this.mIsSetFakePos)
                            {
                                this.selfGo = this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat);
                            }
                            else
                            {
                                this.selfGo = this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat);
                            }

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
            else
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

        // 所有的资源都加载完成
        public void onAllFinish()
        {
            if (this.mIsNeedInsPrefab)
            {
                if (this.selfGo != null)
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
                if(null != mPrefabRes && mPrefabRes.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mResLoadState.setFailed();
                }
            }

            if (this.mEvtHandle != null)
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
                if (null == this.mResInsEventDispatch)
                {
                    this.mResInsEventDispatch = new ResInsEventDispatch();
                }
                this.mResInsEventDispatch.addEventHandle(null, onInstantiateObjectFinish);

                if (this.mIsSetFakePos)
                {
                    this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat, this.mResInsEventDispatch);
                }
                else
                {
                    this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat, this.mResInsEventDispatch);
                }
            }
            else
            {
                if (this.mIsSetFakePos)
                {
                    this.selfGo = this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat);
                }
                else
                {
                    this.selfGo = this.mPrefabRes.InstantiateObject(this.mPrefabRes.getPrefabName(), this.mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat);
                }

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
    }
}