using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class BundleRes : Res, IBundleRes
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
                Ctx.m_instance.m_CoroutineMgr.StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
            }
        }

        override public IEnumerator initAssetByCoroutine()
        {
            //GameObject.Instantiate(m_bundle.Load(m_prefabName));
            GameObject.Instantiate(m_bundle.LoadAsset(m_prefabName));
            yield return null;
            //m_bundle.Unload(false);

            if (onLoadedCB != null)
            {
                Ctx.m_instance.m_shareMgr.m_evt.m_param = this;
                onLoadedCB(Ctx.m_instance.m_shareMgr.m_evt);
            }
        }

        override public void initAsset()
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

        public GameObject InstantiateObject(string resname)
        {
            //return GameObject.Instantiate(m_bundle.Load(resname)) as GameObject;
            return GameObject.Instantiate(m_bundle.LoadAsset(resname)) as GameObject;
        }

        public UnityEngine.Object getObject(string resname)
        {
            //return m_bundle.Load(resname);
            return m_bundle.LoadAsset(resname);
        }

        override public void unload()
        {
            m_bundle.Unload(true);
            //Resources.UnloadUnusedAssets();
            //GC.Collect();
        }
    }
}