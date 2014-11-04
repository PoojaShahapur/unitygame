using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预设资源，通常就一个资源
     */
    public class PrefabRes : Res
    {
        protected UnityEngine.Object m_prefabObj;

        override public void init(LoadItem item)
        {
            m_prefabObj = (item as ResourceLoadItem).prefabObj;
        }

        public UnityEngine.Object prefabObj()
        {
            return m_prefabObj;
        }

        override public void unload()
        {
            Resources.UnloadAsset(m_prefabObj);
            //Resources.UnloadUnusedAssets();
            //GC.Collect();
        }

        override public GameObject InstantiateObject(string resname)
        {
            return GameObject.Instantiate(m_prefabObj) as GameObject;
        }

        override public UnityEngine.Object getObject(string resname)
        {
            return m_prefabObj;
        }
    }
}