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
            m_prefabObj = item.prefabObj;
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
    }
}
