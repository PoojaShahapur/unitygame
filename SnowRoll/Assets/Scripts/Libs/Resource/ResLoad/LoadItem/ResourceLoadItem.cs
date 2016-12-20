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

        public UnityEngine.Object prefabObj
        {
            get
            {
                return mPrefabObj;
            }
            set
            {
                mPrefabObj = value;
            }
        }

        public UnityEngine.Object[] getAllPrefabObject()
        {
            return mAllPrefabObj;
        }

        public override void reset()
        {
            mPrefabObj = null;
            mAllPrefabObj = null;

            base.reset();
        }

        override public void load()
        {
            base.load();
            if (mLoadNeedCoroutine)
            {
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(loadFromDefaultAssetBundleByCoroutine());
            }
            else
            {
                loadFromDefaultAssetBundle();
            }
        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        override public void unload()
        {
            if (mPrefabObj != null)
            {
                //UtilApi.DestroyImmediate(mPrefabObj, true);
                // 如果你用个全局变量保存你 Load 的 Assets，又没有显式的设为 null，那 在这个变量失效前你无论如何 UnloadUnusedAssets 也释放不了那些Assets的。如果你这些Assets又不是从磁盘加载的，那除了 UnloadUnusedAssets 或者加载新场景以外没有其他方式可以卸载之。
                mPrefabObj = null;

                // Asset-Object 无法被Destroy销毁，Asset-Objec t由 Resources 系统管理，需要手工调用Resources.UnloadUnusedAssets()或者其他类似接口才能删除。
                UtilApi.UnloadUnusedAssets();
            }

            if(mAllPrefabObj != null)
            {
                mAllPrefabObj = null;
                UtilApi.UnloadUnusedAssets();
            }

            base.unload();
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void loadFromDefaultAssetBundle()
        {
            bool isSuccess = false;
            if(!mIsLoadAll)
            {
                // Table/ObjectBase_client 可以加载， 而 Table//ObjectBase_client 加载失败
                mPrefabObj = Resources.Load<Object>(mLoadPath);
                if (mPrefabObj != null)
                {
                    isSuccess = true;
                }
            }
            else
            {
                mAllPrefabObj = Resources.LoadAll<Object>(mLoadPath);
                if (mAllPrefabObj != null)
                {
                    isSuccess = true;
                }
            }

            if (isSuccess)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected IEnumerator loadFromDefaultAssetBundleByCoroutine()
        {
            if(!mIsLoadAll)
            {
                ResourceRequest req = Resources.LoadAsync<Object>(mLoadPath);
                yield return req;

                if (req.asset != null && req.isDone)
                {
                    mPrefabObj = req.asset;
                    m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
                }
            }
            else
            {
                mAllPrefabObj = Resources.LoadAll<Object>(mLoadPath);

                if (mAllPrefabObj != null)
                {
                    m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
                }

                yield return null;
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}