using SDK.Lib;

namespace SDK.Common
{
    public class ModelItem : AuxComponent
    {
        protected string m_resPath;
        protected ModelRes m_modelRes;
        protected bool m_bNeedReloadModel;

        public ModelItem()
        {
            m_bNeedReloadModel = false;
        }

        public string resPath
        {
            get
            {
                return m_resPath;
            }
            set
            {
                if (m_resPath != value)
                {
                    m_bNeedReloadModel = true;
                }
                m_resPath = value;
            }
        }

        override public void dispose()
        {
            if (m_selfGo != null)
            {
                UtilApi.Destroy(m_selfGo);
                m_selfGo = null;
            }

            if(m_modelRes != null)
            {
                Ctx.m_instance.m_modelMgr.unload(m_modelRes.GetPath(), null);
                m_modelRes = null;
            }
            
            base.dispose();
        }

        public void updateModel()
        {
            if (m_bNeedReloadModel)
            {
                if(m_modelRes != null)
                {
                    Ctx.m_instance.m_modelMgr.unload(m_modelRes.GetPath(), null);
                    m_modelRes = null;
                }
                if(m_selfGo != null)
                {
                    UtilApi.Destroy(m_selfGo);
                    m_selfGo = null;
                }

                LoadParam param;
                param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.m_path = m_resPath;

                // 这个需要立即加载
                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;

                m_modelRes = Ctx.m_instance.m_modelMgr.getAndLoad<ModelRes>(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);

                m_selfGo = m_modelRes.InstantiateObject(m_resPath);
                if (m_bNeedPlaceHolderGo)
                {
                    if(m_placeHolderGo == null)
                    {
                        m_placeHolderGo = new UnityEngine.GameObject();
                        UtilApi.SetParent(m_placeHolderGo, m_pntGo, false);
                    }
                    UtilApi.SetParent(m_selfGo, m_placeHolderGo, false);
                }
                else
                {
                    UtilApi.SetParent(m_selfGo, m_pntGo, false);
                }
            }

            m_bNeedReloadModel = false;
        }
    }
}