using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Common
{
    public class AuxDynImageDynGO : AuxDynImage
    {
        protected GameObject m_selfGo;      // 动态加载的 Prefab 实例化后的 GameObject
        protected string m_prefabPath;      // Prefab 目录
        protected UIPrefabRes m_prefabRes;  // Prefab 资源
        protected bool m_bNeedReload = false;
        protected bool m_bNeedPlaceHolderGo;    // 是否需要占位 GameObject

        public AuxDynImageDynGO(bool bNeedPlaceHolderGo = false)
        {
            m_bNeedPlaceHolderGo = bNeedPlaceHolderGo;
        }

        public string prefabPath
        {
            set
            {
                if (m_prefabPath != value)
                {
                    m_bNeedReload = true;
                }
                m_prefabPath = value;
            }
        }

        public GameObject selfGo
        {
            get
            {
                return m_selfGo;
            }
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (string.IsNullOrEmpty(m_goName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
            {
                m_image = UtilApi.getComByP<Image>(m_selfGo);
            }
            else
            {
                m_image = UtilApi.getComByP<Image>(m_selfGo, m_goName);
            }
        }

        // 加载 Prefab
        public void loadPrefab()
        {
            if (m_bNeedReload)
            {
                if (m_selfGo != null)
                {
                    UtilApi.Destroy(m_selfGo);
                    m_selfGo = null;
                }
                if (m_prefabRes != null)
                {
                    Ctx.m_instance.m_uiPrefabMgr.unload(m_prefabRes.GetPath(), null);
                    m_prefabRes = null;
                }
                m_prefabRes = Ctx.m_instance.m_uiPrefabMgr.syncGet<UIPrefabRes>(m_prefabPath);
                m_selfGo = m_prefabRes.InstantiateObject(m_prefabPath);

                if (m_bNeedPlaceHolderGo)
                {
                    UtilApi.SetParent(m_selfGo, m_placeHolderGo, false);
                }
                else
                {
                    UtilApi.SetParent(m_selfGo, m_pntGo, false);
                }

                findWidget();
                m_bNeedUpdateImage = true;      // 设置重新更新图像
            }
            m_bNeedReload = false;
        }
    }
}