using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    /**
     * @brief AssetBundle 都是最后全部卸载掉
     */
    public class BundleResItem : ReswithDepItem
    {
        protected AssetBundle m_bundle;
        protected UnityEngine.Object m_prefabObj;       // 加载完成的 Prefab 对象
        protected UnityEngine.Object[] mAllPrefabObj;   // 所有的 Prefab 对象

        public BundleResItem()
        {
            
        }

        override public void init(LoadItem item)
        {
            m_bundle = item.assetBundle;

            base.init(item);
        }

        // 资源加载完成调用
        override protected void onResLoaded()
        {
            if (mResNeedCoroutine)
            {
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
            }
        }

        protected void initAsset()
        {
            // 加载完成获取资源，目前用到的时候再获取
            /*
            if (!string.IsNullOrEmpty(mPrefabName) && m_bundle.Contains(mPrefabName))
            {
                // Unity5
                //GameObject.Instantiate(m_bundle.LoadAsset(mPrefabName));
                // Unity4
                //GameObject.Instantiate(m_bundle.Load(mPrefabName));
                //m_bundle.Unload(false);

                if(!mIsLoadAll)
                {
#if UNITY_5
                    // Unty5
                    m_prefabObj = m_bundle.LoadAsset(mPrefabName);
#elif UNITY_4_6
                    // Unity4
                    m_prefabObj = m_bundle.Load(mPrefabName);
#endif
                }
                else
                {
#if UNITY_5
                    mAllPrefabObj = m_bundle.LoadAllAssets<UnityEngine.Object>();
#endif
                }
            }
            */

            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        protected IEnumerator initAssetByCoroutine()
        {
            // 加载完成获取资源，目前用到的时候再获取
            /*
            if (!string.IsNullOrEmpty(mPrefabName) && m_bundle.Contains(mPrefabName))
            {
                // 加载 Prefab 资源
                AssetBundleRequest req = null;
                if (!mIsLoadAll)
                {
#if UNITY_5
                    // Unity5
                    req = m_bundle.LoadAssetAsync(mPrefabName);
#elif UNITY_4_6 || UNITY_4_5
                    // Unity4
                    req = m_bundle.LoadAsync(mPrefabName, typeof(GameObject));
#endif
                    yield return req;

                    m_prefabObj = req.asset;
                }
                else
                {
#if UNITY_5
                    // Unity5
                    req = m_bundle.LoadAllAssetsAsync<UnityEngine.Object>();
#elif UNITY_4_6 || UNITY_4_5
                    // Unity4
                    req = m_bundle.LoadAllAsync<UnityEngine.Object>();
#endif
                    yield return req;

                    mAllPrefabObj = req.allAssets;
                }

                //GameObject.Instantiate(req.asset);
                //m_bundle.Unload(false);
            }
            */

            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);

            //yield return null;
            yield break;
        }

        override public void unrefAssetObject()
        {
            m_prefabObj = null;
            mAllPrefabObj = null;

            base.unrefAssetObject();
        }

        override public void reset()
        {
            base.reset();

            m_bundle = null;
            m_prefabObj = null;
            mAllPrefabObj = null;
        }

        override public GameObject InstantiateObject(string resName)
        {
            // Test 查看包内部资源
            //UnityEngine.Object[] test = m_bundle.LoadAllAssets();
            // Unity5
            //string[] allName = m_bundle.AllAssetNames();
            //return GameObject.Instantiate(m_bundle.Load(resName)) as GameObject;
            GameObject insObj = null;
            if (m_bundle.Contains(resName))
            {
#if UNITY_5
                // Unity5
                UnityEngine.Object assets = m_bundle.LoadAsset(resName);
#elif UNITY_4_6 || UNITY_4_5
                // Unity4
                UnityEngine.Object assets = m_bundle.Load(resName);
#endif
                if (assets != null)
                {
#if UNITY_5
                    // Unity5
                    insObj = GameObject.Instantiate(m_bundle.LoadAsset(resName)) as GameObject;
#elif UNITY_4_6
                    // Unity4
                    insObj = GameObject.Instantiate(m_bundle.Load(resName)) as GameObject;
#endif
                }
                else
                {
                    // Unity5
#if UNITY_5
                    //assets = m_bundle.LoadAsset("DefaultAvatar");
#elif UNITY_4_6 || UNITY_4_5
                    // Unity4
                    assets = m_bundle.Load("DefaultAvatar");
#endif
                }
            }
            return insObj;
        }

        override public UnityEngine.Object getObject(string resName)
        {
            // Unity5
            //string[] allName = m_bundle.AllAssetNames();

            //return m_bundle.Load(resName);

            if (resName == mPrefabName && m_prefabObj != null)
            {
                return m_prefabObj;
            }
            else
            {
                UnityEngine.Object assets = null;
                if (m_bundle.Contains(resName))
                {
#if UNITY_5
                    // Unty5
                    assets = m_bundle.LoadAsset(resName);
#elif UNITY_4_6
                // Unity4
                assets = m_bundle.Load(resName);
#endif
                }
                return assets;
            }
        }

        // 这个是返回所有的对象，例如如果一个有纹理的精灵图集，如果使用这个接口，就会返回一个 Texture2D 和所有的 Sprite 列表，这个时候如果强制转换成 Sprite[]，就会失败
        override public UnityEngine.Object[] getAllObject()
        {
            if (mAllPrefabObj == null)
            {
                mAllPrefabObj = m_bundle.LoadAllAssets<UnityEngine.Object>();
            }

            return mAllPrefabObj;
        }

        override public T[] loadAllAssets<T>()
        {
            //T[] ret = m_bundle.LoadAllAssets<T>();
            //return ret;

            if(mAllPrefabObj == null)
            {
                getAllObject();
            }

            MList<T> list = new MList<T>();
            int idx = 0;
            int len = mAllPrefabObj.Length;
            while (idx < len)
            {
                if (mAllPrefabObj[idx] is T)
                {
                    list.Add(mAllPrefabObj[idx] as T);
                }

                ++idx;
            }

            return list.ToArray();
        }

        override public void unload(bool unloadAllLoadedObjects = true)
        {
            // 如果是用了 Unload(true) ，就不用 Resources.UnloadUnusedAssets() ，如果使用了 Unload(false) ，就需要使用 Resources.UnloadUnusedAssets()
            //m_bundle.Unload(true);
            //Resources.UnloadUnusedAssets();
            //GC.Collect();
            //m_bundle.Unload(false);

            if (m_bundle != null)
            {
                UtilApi.UnloadAssetBundles(m_bundle, unloadAllLoadedObjects);
                //UtilApi.UnloadAssetBundles(m_bundle, unloadAllLoadedObjects);
                m_bundle = null;
            }

            base.unload(unloadAllLoadedObjects);
        }
    }
}