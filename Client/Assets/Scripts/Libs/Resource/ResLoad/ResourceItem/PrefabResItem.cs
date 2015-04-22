using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预设资源，通常就一个资源，主要是 Resources 目录下的资源加载
     */
    public class PrefabResItem : ResItem
    {
        protected UnityEngine.Object m_prefabObj;   // 加载完成的 Prefab 对象
        protected GameObject m_retGO;       // 方便调试的临时对象
        protected string m_prefabName;      // 预制名字

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

        public override string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return m_prefabName;
        }

        override public void unload()
        {
            //Resources.UnloadAsset(m_prefabObj);   // 这个是同步卸载
            m_prefabObj = null;
            //Resources.UnloadUnusedAssets();         // 这个事异步卸载
            //GC.Collect();
        }

        override public GameObject InstantiateObject(string resname)
        {
            m_retGO = null;

            if (null == m_prefabObj)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_prefabObj) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }
            return m_retGO;
        }

        override public UnityEngine.Object getObject(string resname)
        {
            return m_prefabObj;
        }

        override public byte[] getBytes(string resname)            // 获取字节数据
        {
            if(m_prefabObj != null && (m_prefabObj as TextAsset) != null)
            {
                return (m_prefabObj as TextAsset).bytes;
            }

            return null;
        }

        override public string getText(string resname)
        {
            if (m_prefabObj != null && (m_prefabObj as TextAsset) != null)
            {
                return (m_prefabObj as TextAsset).text;
            }

            return null;
        }
    }
}