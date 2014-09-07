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
        //public BundleRes(string path)
        //    : base(path)
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
            m_bundle = item.w3File.assetBundle;
            if (m_resNeedCoroutine)
            {
                StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
            }
        }

        override public IEnumerator initAssetByCoroutine()
        {
            Instantiate(m_bundle.Load(m_prefabName));
            yield return null;
            m_bundle.Unload(false);

            if (onInited != null)
            {
                onInited(this);
            }
        }

        override public void initAsset()
        {
            //Instantiate(m_bundle.Load(m_prefabName));
            //m_bundle.Unload(false);

            if (onInited != null)
            {
                onInited(this);
            }
        }

        override public void reset()
        {
            base.reset();
            m_bundle = null;
        }

        public GameObject InstantiateObject(string resname)
        {
            return Instantiate(m_bundle.Load(resname)) as GameObject;
        }
    }
}