using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 自己拥有资源 
     */
    public class AuxResComponent : AuxParentComponent
    {
        protected GameObject m_selfLocalGo;
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

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            m_res = dispObj as ModelRes;
            if (m_res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

                m_selfGo = m_res.InstantiateObject(m_path);
                m_selfGo.transform.SetParent(m_selfLocalGo.transform, false);

                // 不是使用 m_resLoadMgr.load 接口加载的资源，不要使用 m_resLoadMgr.unload 去卸载资源
                // 卸载资源
                //Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
            }
            else if (m_res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                m_res = dispObj as ModelRes;
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, m_res.GetPath());

                // 卸载资源
                //Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
            }
        }

        public virtual void unload()
        {
            if (m_selfGo != null)
            {
                UtilApi.Destroy(m_selfGo);
                m_selfGo = null;
                Ctx.m_instance.m_modelMgr.unload(m_path, null);
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
                    param.m_loadEventHandle = onLoadEventHandle;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }
    }
}