using System;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    public class BundleRes : Res
    {
        AssetBundle m_bundle;
        public BundleRes(string path)
            : base(path)
        {

        }

        override public void init(LoadItem item)
        {
            m_bundle = item.w3File.assetBundle;
            StartCoroutine(initAsset());
        }

        override public IEnumerator initAsset()
        {
            Instantiate(m_bundle.Load("cube"));
            yield return null;
            m_bundle.Unload(false);

            if (onInited != null)
            {
                onInited();
            }
        }
    }
}