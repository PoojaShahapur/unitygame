using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief UI 场景管理器，场景中的 UI 都是和场景放在一起的，没有额外的资源加载，不卸载，和场景同在
     */
    public class UISceneMgr : IUISceneMgr
    {
        private Dictionary<UISceneFormID, SceneForm> m_dicForm = new Dictionary<UISceneFormID, SceneForm>(); //[id,form]
        public IUISceneFactory m_IUISceneFactory;

        public ISceneForm loadSceneForm(UISceneFormID ID)
        {
            if(!m_dicForm.ContainsKey(ID))
            {
                if (m_IUISceneFactory != null)
                {
                    m_dicForm[ID] = m_IUISceneFactory.CreateSceneForm(ID) as SceneForm;
                    m_dicForm[ID].id = ID;
                    return m_dicForm[ID];
                }
            }

            return m_dicForm[ID];
        }

        public void readySceneForm(UISceneFormID ID)
        {
            SceneForm win = null;
            if (m_dicForm.ContainsKey(ID))
            {
                win = m_dicForm[ID];

                if (win != null)
                {
                    if (!win.bReady)
                    {
                        win.onReady();
                    }
                }
            }
        }

        public ISceneForm showSceneForm(UISceneFormID ID)
        {
            SceneForm win = null;
            if(m_dicForm.ContainsKey(ID))
            {
                win = m_dicForm[ID];

                if (win != null)
                {
                    if (!win.bReady)
                    {
                        win.onReady();
                    }
                    if (!win.bVisible)
                    {
                        win.onShow();
                    }
                }
            }

            return win;
        }

        public ISceneForm loadAndShowForm(UISceneFormID ID)
        {
            loadSceneForm(ID);
            return showSceneForm(ID);
        }

        public void hideSceneForm(UISceneFormID ID)
        {
            SceneForm win = null;
            if (m_dicForm.ContainsKey(ID))
            {
                win = m_dicForm[ID];

                if (win != null)
                {
                    if (win.bVisible)
                    {
                        win.onHide();
                    }
                }
            }
        }

        public ISceneForm getSceneUI(UISceneFormID ID)
        {
            if (m_dicForm.ContainsKey(ID))
            {
                return m_dicForm[ID];
            }

            return null;
        }

        public void SetIUISceneFactory(IUISceneFactory value)
        {
            m_IUISceneFactory = value;
        }
    }
}