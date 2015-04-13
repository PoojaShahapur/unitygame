using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 自己拥有资源 
     */
    public class AuxResComponent : AuxComponent
    {
        protected GameObject m_selfLocalGo;
        protected GameObject m_selfGo;
        protected string m_path;                          // 目录
        protected ModelRes m_res;

        public AuxResComponent()
        {
            m_selfLocalGo = new GameObject();
            m_selfLocalGo.name = "m_selfLocalGo";
        }

        public GameObject selfLocalGo
        {
            get
            {
                return m_selfLocalGo;
            }
            set
            {
                m_selfLocalGo = value;
            }
        }

        public GameObject selfGo
        {
            get
            {
                return m_selfGo;
            }
            set
            {
                m_selfGo = value;
            }
        }

        public string path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }

        public override void setPntGo(GameObject go)
        {
            base.setPntGo(go);
            m_selfLocalGo.transform.SetParent(pntGo.transform, false);
        }

        public virtual void onloaded(IDispatchObject resEvt)            // 资源加载成功
        {
            m_res = resEvt as ModelRes;
            m_selfGo = m_res.InstantiateObject(m_path);
            m_selfGo.transform.SetParent(m_selfLocalGo.transform, false);
        }

        public virtual void unload()
        {
            if (m_selfGo != null)
            {
                UtilApi.Destroy(m_selfGo);
                m_selfGo = null;
                Ctx.m_instance.m_modelMgr.unload(m_path);
                m_res = null;
            }
        }

        public virtual void load()
        {
            bool needLoad = true;

            if (m_res != null)
            {
                if (m_res.GetPath() != m_path)
                {
                    unload();
                }
                else
                {
                    needLoad = false;
                }
            }
            if (needLoad)
            {
                if (!string.IsNullOrEmpty(m_path))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.m_path = m_path;
                    param.m_loaded = onloaded;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }
    }
}