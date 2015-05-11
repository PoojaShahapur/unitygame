using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief AssetBundle 都是最后全部卸载掉
     */
    public class BundleResItem : ResItem
    {
        protected AssetBundle m_bundle;

        public BundleResItem()
        {

        }

        override public void init(LoadItem item)
        {
            base.init(item);

            m_bundle = item.assetBundle;
            if (m_resNeedCoroutine)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
            }
        }

        protected void initAsset()
        {
            if (!string.IsNullOrEmpty(m_path))
            {
                // Unity5
                //GameObject.Instantiate(m_bundle.LoadAsset(m_prefabName));
                // Unity4
                //GameObject.Instantiate(m_bundle.Load(m_prefabName));
                //m_bundle.Unload(false);
            }

            if (onLoaded != null)
            {
                onLoaded(this);
            }

            clearListener();
        }

        protected IEnumerator initAssetByCoroutine()
        {
            if (!string.IsNullOrEmpty(m_path))
            {
#if UNITY_5
                // Unity5
                AssetBundleRequest req = m_bundle.LoadAssetAsync(m_path);
#elif UNITY_4_6
                // Unity4
                AssetBundleRequest req = m_bundle.LoadAsync(m_prefabName, typeof(GameObject));
#endif
                yield return req;

                //GameObject.Instantiate(req.asset);
                //m_bundle.Unload(false);
            }

            if (onLoaded != null)
            {
                onLoaded(this);
            }

            clearListener();

            yield return null;
        }

        override public void reset()
        {
            base.reset();
            m_bundle = null;
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
#elif UNITY_4_6
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
#elif UNITY_4_6
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

        override public void unload()
        {
            // 如果是用了 Unload(true) ，就不用 Resources.UnloadUnusedAssets() ，如果使用了 Unload(false) ，就需要使用 Resources.UnloadUnusedAssets()
            //m_bundle.Unload(true);
            //Resources.UnloadUnusedAssets();
            //GC.Collect();
            m_bundle.Unload(false);
        }
    }
}