using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    class AsyncBundleRes : AsyncRes, IBundleRes
    {
        protected AssetBundle m_bundle;
        protected string m_prefabName;

        public AsyncBundleRes()
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
        }

        override public void initAsset()
        {
            //Instantiate(m_bundle.Load(m_prefabName));
            //m_bundle.Unload(false);

            //if (onInited != null)
            //{
            //    onInited(this);
            //}
        }

        override public void reset()
        {
            base.reset();
            m_bundle = null;
        }

        public GameObject InstantiateObject(string resname)
        {
            return GameObject.Instantiate(m_bundle.Load(resname)) as GameObject;
        }

        public UnityEngine.Object getObject(string resname)
        {
            return m_bundle.Load(resname);
        }

        override public void unload()
        {
            m_bundle.Unload(true);
        }
    }
}
