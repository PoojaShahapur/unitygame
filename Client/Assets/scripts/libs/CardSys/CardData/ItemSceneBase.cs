using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中需要显示，并且需要加载的内容的基类
     */
    public class ItemSceneBase
    {
        public Transform m_tran = null;                // 第一个位置
        public GameObject m_go = null;                 // 显示的内容
        public string m_prefab;                        // 预制名字
        public string m_path;                          // 目录
        public ModelRes m_res;
        public bool m_bNorm = true;

        public virtual void onloaded(IDispatchObject resEvt)            // 资源加载成功
        {
            m_res = resEvt as ModelRes;
            m_go = m_res.InstantiateObject(m_prefab);
            m_go.transform.parent = m_tran;
            if (m_bNorm)
            {
                UtilApi.normalPosScale(m_go.transform);
            }
        }

        public virtual void unload()
        {
            if (m_go != null)
            {
                UtilApi.Destroy(m_go);
                m_go = null;
                Ctx.m_instance.m_resLoadMgr.unload(m_path);
            }
        }

        public virtual void load()
        {
            if (m_res != null)
            {
                if (m_res.GetPath() != m_path)
                {
                    unload();

                    LoadParam param;
                    param = Ctx.m_instance.m_resLoadMgr.getLoadParam();
                    param.m_path = m_path;
                    param.m_prefabName = m_prefab;
                    param.m_loaded = onloaded;
                    //Ctx.m_instance.m_resLoadMgr.loadResources(param);
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    m_res = null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(m_prefab) && !string.IsNullOrEmpty(m_path))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_resLoadMgr.getLoadParam();
                    param.m_path = m_path;
                    param.m_prefabName = m_prefab;
                    param.m_loaded = onloaded;
                    //Ctx.m_instance.m_resLoadMgr.loadResources(param);
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                }
            }
        }

        public void setDefaultRes()
        {
            m_prefab = "pack";
            m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + m_prefab;
        }
    }
}