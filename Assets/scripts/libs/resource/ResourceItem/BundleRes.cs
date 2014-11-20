using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class BundleRes : Res
    {
        protected AssetBundle m_bundle;
        protected string m_prefabName;

        public BundleRes()
        {

        }

        public string prefabName
        {
            get
            {
                return m_prefabName;
            }
            set
            {
                m_prefabName = value;
            }
        }

        override public void init(LoadItem item)
        {
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

        protected IEnumerator initAssetByCoroutine()
        {
            if (!string.IsNullOrEmpty(m_prefabName))
            {
                AssetBundleRequest req = m_bundle.LoadAssetAsync(m_prefabName);
                yield return req;

                GameObject.Instantiate(req.asset);
                //m_bundle.Unload(false);
            }

            if (onLoadedCB != null)
            {
                Ctx.m_instance.m_shareMgr.m_evt.m_param = this;
                onLoadedCB(Ctx.m_instance.m_shareMgr.m_evt);
            }

            yield return null;
        }

        protected void initAsset()
        {
            if (!string.IsNullOrEmpty(m_prefabName))
            {
                //GameObject.Instantiate(m_bundle.Load(m_prefabName));
                GameObject.Instantiate(m_bundle.LoadAsset(m_prefabName));
                //m_bundle.Unload(false);
            }

            if (onLoadedCB != null)
            {
                Ctx.m_instance.m_shareMgr.m_evt.m_param = this;
                onLoadedCB(Ctx.m_instance.m_shareMgr.m_evt);
            }
        }

        override public void reset()
        {
            base.reset();
            m_bundle = null;
        }

        override public GameObject InstantiateObject(string resname)
        {
            // Test 查看包内部资源
            //UnityEngine.Object[] test = m_bundle.LoadAllAssets();
            string[] allName = m_bundle.AllAssetNames();
            //return GameObject.Instantiate(m_bundle.Load(resname)) as GameObject;
            GameObject insObj = null;
            if (m_bundle.Contains(resname))
            {
                UnityEngine.Object assets = m_bundle.LoadAsset(resname);
                if (assets != null)
                {
                    insObj = GameObject.Instantiate(m_bundle.LoadAsset(resname)) as GameObject;
                }
                else
                {
                    assets = m_bundle.LoadAsset("DefaultAvatar");
                }
            }
            return insObj;
        }

        override public UnityEngine.Object getObject(string resname)
        {
            string[] allName = m_bundle.AllAssetNames();

            //return m_bundle.Load(resname);
            UnityEngine.Object assets = null;
            if (m_bundle.Contains(resname))
            {
                assets = m_bundle.LoadAsset(resname);
            }
            return assets;
        }

        override public void unload()
        {
            m_bundle.Unload(true);
            //Resources.UnloadUnusedAssets();
            //GC.Collect();
        }
    }
}