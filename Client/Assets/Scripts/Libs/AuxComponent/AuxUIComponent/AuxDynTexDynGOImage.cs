using SDK.Lib;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxDynTexDynGOImage : AuxDynTexImage
    {
        protected string m_prefabPath;      // Prefab 目录
        protected UIPrefabRes m_prefabRes;  // Prefab 资源
        protected bool m_bNeedReload = false;

        public AuxDynTexDynGOImage(bool bNeedPlaceHolderGo = false)
        {
            m_bNeedPlaceHolderGo = bNeedPlaceHolderGo;
        }

        override public void dispose()
        {
            if (m_selfGo != null)
            {
                UtilApi.Destroy(m_selfGo);
            }

            base.dispose();
            
            if(m_prefabRes != null)
            {
                Ctx.m_instance.m_uiPrefabMgr.unload(m_prefabRes.GetPath(), null);
            }
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
        protected void loadPrefab()
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
                m_prefabRes = Ctx.m_instance.m_uiPrefabMgr.getAndSyncLoad<UIPrefabRes>(m_prefabPath);
                m_selfGo = m_prefabRes.InstantiateObject(m_prefabPath);

                if (m_bNeedPlaceHolderGo && m_placeHolderGo != null)
                {
                    UtilApi.SetParent(m_selfGo, m_placeHolderGo, false);
                }
                else if (m_pntGo != null)
                {
                    UtilApi.SetParent(m_selfGo, m_pntGo, false);
                }

                findWidget();
                m_bImageGoChange = true;      // 设置重新更新图像
            }
            m_bNeedReload = false;
        }

        override public void syncUpdateCom()
        {
            loadPrefab();
            base.syncUpdateCom();
        }
    }
}