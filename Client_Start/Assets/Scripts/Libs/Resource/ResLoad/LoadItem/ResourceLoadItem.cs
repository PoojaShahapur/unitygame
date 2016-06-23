using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅仅支持从 Resources 默认 Bundle 包中加载
     */
    public class ResourceLoadItem : LoadItem
    {
        protected UnityEngine.Object m_prefabObj;
        protected UnityEngine.Object[] mAllPrefabObj;

        public UnityEngine.Object prefabObj
        {
            get
            {
                return m_prefabObj;
            }
            set
            {
                m_prefabObj = value;
            }
        }

        public UnityEngine.Object[] getAllPrefabObject()
        {
            return mAllPrefabObj;
        }

        public override void reset()
        {
            m_prefabObj = null;
            mAllPrefabObj = null;

            base.reset();
        }

        override public void load()
        {
            base.load();
            if (m_loadNeedCoroutine)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(loadFromDefaultAssetBundleByCoroutine());
            }
            else
            {
                loadFromDefaultAssetBundle();
            }
        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        override public void unload()
        {
            if (m_prefabObj != null)
            {
                //UtilApi.DestroyImmediate(m_prefabObj, true);
                // 如果你用个全局变量保存你 Load 的 Assets，又没有显式的设为 null，那 在这个变量失效前你无论如何 UnloadUnusedAssets 也释放不了那些Assets的。如果你这些Assets又不是从磁盘加载的，那除了 UnloadUnusedAssets 或者加载新场景以外没有其他方式可以卸载之。
                m_prefabObj = null;

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
                m_prefabObj = Resources.Load<Object>(m_loadPath);
                if (m_prefabObj != null)
                {
                    isSuccess = true;
                }
            }
            else
            {
                mAllPrefabObj = Resources.LoadAll<Object>(m_loadPath);
                if (mAllPrefabObj != null)
                {
                    isSuccess = true;
                }
            }

            if (isSuccess)
            {
                Ctx.m_instance.m_logSys.log(string.Format("ResourceLoadItem::loadFromDefaultAssetBundle, Success, ResLoadType is {0}, ResPackType is {1}, Load Not Need Coroutine, m_origPath is {2}", "LoadResource", "Prefab", m_origPath), LogTypeId.eLogResLoader);

                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("ResourceLoadItem::loadFromDefaultAssetBundle, Fail, ResLoadType is {0}, ResPackType is {1}, Load Not Need Coroutine, m_origPath is {2}", "LoadResource", "Prefab", m_origPath), LogTypeId.eLogResLoader);

                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected IEnumerator loadFromDefaultAssetBundleByCoroutine()
        {
            if(!mIsLoadAll)
            {
                ResourceRequest req = Resources.LoadAsync<Object>(m_loadPath);
                yield return req;

                if (req.asset != null && req.isDone)
                {
                    Ctx.m_instance.m_logSys.log(string.Format("ResourceLoadItem::loadFromDefaultAssetBundleByCoroutine, Success, ResLoadType is {0}, ResPackType is {1}, Load Not Need Coroutine, m_origPath is {2}", "LoadResource", "Prefab", m_origPath), LogTypeId.eLogResLoader);

                    m_prefabObj = req.asset;
                    m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    Ctx.m_instance.m_logSys.log(string.Format("ResourceLoadItem::loadFromDefaultAssetBundleByCoroutine, Fail, ResLoadType is {0}, ResPackType is {1}, Load Not Need Coroutine, m_origPath is {2}", "LoadResource", "Prefab", m_origPath), LogTypeId.eLogResLoader);

                    m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
                }
            }
            else
            {
                mAllPrefabObj = Resources.LoadAll<Object>(m_loadPath);

                if (mAllPrefabObj != null)
                {
                    Ctx.m_instance.m_logSys.log(string.Format("ResourceLoadItem::loadFromDefaultAssetBundleByCoroutine, Success, ResLoadType is {0}, ResPackType is {1}, Load Not Need Coroutine, m_origPath is {2}", "LoadResource", "Prefab", m_origPath), LogTypeId.eLogResLoader);

                    m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    Ctx.m_instance.m_logSys.log(string.Format("ResourceLoadItem::loadFromDefaultAssetBundleByCoroutine, Fail, ResLoadType is {0}, ResPackType is {1}, Load Not Need Coroutine, m_origPath is {2}", "LoadResource", "Prefab", m_origPath), LogTypeId.eLogResLoader);

                    m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
                }

                yield return null;
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}