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
        public UIGrid m_grid;                          // 放在格子数据中
        public GameObject m_go = null;                 // 显示的内容
        public string m_path;                          // 目录
        public ModelRes m_res;
        public bool m_bNorm = true;

        public virtual void onLoaded(IDispatchObject resEvt)            // 资源加载成功
        {
            m_res = resEvt as ModelRes;
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

            m_go = m_res.InstantiateObject(m_path);
            if (m_tran != null)     // 很可能 UIGrid 处理位置了
            {
                m_go.transform.parent = m_tran;
            }
            else if(m_grid != null)
            {
                m_grid.AddChild(m_go.transform);
            }
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
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.m_path = m_path;
                    param.m_loaded = onLoaded;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                    m_res = null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(m_path))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.m_path = m_path;
                    param.m_loaded = onLoaded;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }

        public void setDefaultRes()
        {
            m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + "pack.prefab";
        }
    }
}