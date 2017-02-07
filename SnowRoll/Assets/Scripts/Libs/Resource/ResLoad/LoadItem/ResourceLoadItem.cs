using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅仅支持从 Resources 默认 Bundle 包中加载
     */
    public class ResourceLoadItem : LoadItem
    {
        protected UnityEngine.Object mPrefabObj;
        protected UnityEngine.Object[] mAllPrefabObj;
        protected ResourceRequest mAsyncRequest;

        public ResourceLoadItem()
        {
            this.mAsyncRequest = null;
        }

        public UnityEngine.Object prefabObj
        {
            get
            {
                return this.mPrefabObj;
            }
            set
            {
                this.mPrefabObj = value;
            }
        }

        public UnityEngine.Object[] getAllPrefabObject()
        {
            return this.mAllPrefabObj;
        }

        public override void reset()
        {
            this.mPrefabObj = null;
            this.mAllPrefabObj = null;

            base.reset();
        }

        override public void load()
        {
            base.load();

            if (this.mLoadNeedCoroutine)
            {
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(this.loadFromDefaultAssetBundleByCoroutine());
            }
            else
            {
                this.loadFromDefaultAssetBundle();
            }
        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        override public void unload()
        {
            this.mAsyncRequest = null;

            if (this.mPrefabObj != null)
            {
                //UtilApi.DestroyImmediate(mPrefabObj, true);
                // 如果你用个全局变量保存你 Load 的 Assets，又没有显式的设为 null，那 在这个变量失效前你无论如何 UnloadUnusedAssets 也释放不了那些Assets的。如果你这些Assets又不是从磁盘加载的，那除了 UnloadUnusedAssets 或者加载新场景以外没有其他方式可以卸载之。
                this.mPrefabObj = null;

                // Asset-Object 无法被Destroy销毁，Asset-Object 由 Resources 系统管理，需要手工调用Resources.UnloadUnusedAssets()或者其他类似接口才能删除。
                // 很卡，暂时屏蔽掉
                //UtilApi.UnloadUnusedAssets();
            }

            if(this.mAllPrefabObj != null)
            {
                this.mAllPrefabObj = null;
                // 很卡，暂时屏蔽掉
                //UtilApi.UnloadUnusedAssets();
            }

            base.unload();
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void loadFromDefaultAssetBundle()
        {
            bool isSuccess = false;

            if(!this.mIsLoadAll)
            {
                // Table/ObjectBase_client 可以加载， 而 Table//ObjectBase_client 加载失败
                this.mPrefabObj = Resources.Load<Object>(mLoadPath);

                if (this.mPrefabObj != null)
                {
                    isSuccess = true;
                }
            }
            else
            {
                this.mAllPrefabObj = Resources.LoadAll<Object>(mLoadPath);

                if (this.mAllPrefabObj != null)
                {
                    isSuccess = true;
                }
            }

            if (isSuccess)
            {
                this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            this.mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected IEnumerator loadFromDefaultAssetBundleByCoroutine()
        {
            //if(!this.mIsLoadAll)
            //{
            //    ResourceRequest req = Resources.LoadAsync<Object>(mLoadPath);
            //    yield return req;

            //    if (req.asset != null && req.isDone)
            //    {
            //        this.mPrefabObj = req.asset;
            //        this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            //    }
            //    else
            //    {
            //        this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
            //    }
            //}
            //else
            //{
            //    this.mAllPrefabObj = Resources.LoadAll<Object>(mLoadPath);

            //    if (this.mAllPrefabObj != null)
            //    {
            //        this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            //    }
            //    else
            //    {
            //        this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
            //    }

            //    yield return null;
            //}

            //this.mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);

            if (!this.mIsLoadAll)
            {
                this.mAsyncRequest = Resources.LoadAsync<Object>(mLoadPath);
                yield return this.mAsyncRequest;

                if (this.mAsyncRequest.asset != null && this.mAsyncRequest.isDone)
                {
                    this.mPrefabObj = this.mAsyncRequest.asset;
                    this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
                }
            }
            else
            {
                this.mAllPrefabObj = Resources.LoadAll<Object>(mLoadPath);

                if (this.mAllPrefabObj != null)
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    this.mNonRefCountResLoadResultNotify.resLoadState.setFailed();
                }

                yield return null;
            }

            this.mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override protected void updateProgress()
        {
            if (null != this.mAsyncRequest)
            {
                this.mLoadProgress = this.mAsyncRequest.progress;
            }

            base.updateProgress();
        }
    }
}