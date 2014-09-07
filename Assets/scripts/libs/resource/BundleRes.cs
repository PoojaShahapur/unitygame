using System;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    public class BundleRes : Res
    {
        protected AssetBundle m_bundle;
        //public BundleRes(string path)
        //    : base(path)
        public BundleRes()
        {

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
            Instantiate(m_bundle.Load("cube"));
            yield return null;
            m_bundle.Unload(false);

            if (onInited != null)
            {
                onInited();
            }
        }

        override public void initAsset()
        {
            Instantiate(m_bundle.Load("cube"));
            m_bundle.Unload(false);

            if (onInited != null)
            {
                onInited();
            }
        }

        override public void reset()
        {
            base.reset();
            m_bundle = null;
        }
    }
}