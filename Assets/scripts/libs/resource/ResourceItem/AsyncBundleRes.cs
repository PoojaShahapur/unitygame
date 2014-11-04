using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    class AsyncBundleRes : AsyncRes
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

        public void initAsset()
        {
            //Instantiate(m_bundle.Load(m_prefabName));
            //m_bundle.Unload(false);

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
			return GameObject.Instantiate(m_bundle.LoadAsset(resname)) as GameObject;}

        public UnityEngine.Object getObject(string resname)
        {
            //return m_bundle.Load(resname);
            return m_bundle.LoadAsset(resname);
        }

        override public void unload()
        {
            m_bundle.Unload(true);
        }
    }
}
