using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预设资源，通常就一个资源
     */
    public class PrefabResItem : ResItem
    {
        protected UnityEngine.Object m_prefabObj;   // 加载完成的 Prefab 对象
        protected GameObject m_retGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            m_prefabObj = (item as ResourceLoadItem).prefabObj;

            if (onLoaded != null)
            {
                onLoaded(this);
            }

            clearListener();
        }

        public UnityEngine.Object prefabObj()
        {
            return m_prefabObj;
        }

        override public void unload()
        {
            //Resources.UnloadAsset(m_prefabObj);
            m_prefabObj = null;
            Resources.UnloadUnusedAssets();
            //GC.Collect();
        }

        override public GameObject InstantiateObject(string resname)
        {
            m_retGO = null;

            if (null == m_prefabObj)
            {
                Ctx.m_instance.m_log.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_prefabObj) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_log.log("不能实例化数据");
                }
            }
            return m_retGO;
        }

        override public UnityEngine.Object getObject(string resname)
        {
            return m_prefabObj;
        }
    }
}