using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief UI 场景管理器，场景中的 UI 都是和场景放在一起的，没有额外的资源加载，不卸载，和场景同在
     */
    public class UISceneMgr
    {
        private Dictionary<UISceneFormID, SceneForm> m_dicForm = new Dictionary<UISceneFormID, SceneForm>(); //[id,form]

        public SceneForm loadSceneForm<T>(UISceneFormID ID) where T : SceneForm, new()
        {
            if(!m_dicForm.ContainsKey(ID))
            {
                m_dicForm[ID] = new T();
                m_dicForm[ID].id = ID;
                return m_dicForm[ID];
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

        public SceneForm showSceneForm(UISceneFormID ID)
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

        public SceneForm loadAndShowForm<T>(UISceneFormID ID) where T : SceneForm, new()
        {
            loadSceneForm<T>(ID);
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

        public void exitSceneForm(UISceneFormID ID, bool bremoved = true)
        {
            SceneForm win = null;
            if (m_dicForm.ContainsKey(ID))
            {
                win = m_dicForm[ID];

                if (win != null)
                {
                    win.onExit();
                    if (bremoved)
                    {
                        m_dicForm.Remove(ID);
                    }
                }
            }
        }

        public SceneForm getSceneUI(UISceneFormID ID)
        {
            if (m_dicForm.ContainsKey(ID))
            {
                return m_dicForm[ID];
            }

            return null;
        }

        public void unloadAll()
        {
            foreach (UISceneFormID key in m_dicForm.Keys)
            {
                exitSceneForm(key, false);      // 不能在循环内部删除
            }

            m_dicForm.Clear();
        }
    }
}