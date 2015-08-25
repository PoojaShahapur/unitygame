using SDK.Lib;

namespace SDK.Lib
{
    /**
     * @brief 动态模型，材质都是静态的，不会改变，只会加载动态模型
     */
    public class AuxDynModel : AuxComponent
    {
        protected string m_modelResPath;
        protected ModelRes m_modelRes;
        protected bool m_bNeedReloadModel;
        protected EventDispatch m_modelInsDisp;      // 模型加载并且实例化后事件分发

        public AuxDynModel()
        {
            m_modelInsDisp = new AddOnceEventDispatch();
            m_bNeedReloadModel = false;
        }

        public string modelResPath
        {
            get
            {
                return m_modelResPath;
            }
            set
            {
                if (m_modelResPath != value)
                {
                    m_bNeedReloadModel = true;
                }
                m_modelResPath = value;
            }
        }

        public EventDispatch modelInsDisp
        {
            get
            {
                return m_modelInsDisp;
            }
            set
            {
                m_modelInsDisp = value;
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

            m_modelInsDisp.clearEventHandle();
            m_modelInsDisp = null;

            base.dispose();
        }

        public void attach2Parent()
        {
            if (m_pntGo != null)
            {
                if (m_bNeedPlaceHolderGo)
                {
                    if (m_placeHolderGo == null)
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
        }

        public void syncUpdateModel()
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

                m_modelRes = Ctx.m_instance.m_modelMgr.getAndSyncLoad<ModelRes>(m_modelResPath);

                selfGo = m_modelRes.InstantiateObject(m_modelResPath);
                attach2Parent();

                m_modelInsDisp.dispatchEvent(this);
            }

            m_bNeedReloadModel = false;
        }
    }
}