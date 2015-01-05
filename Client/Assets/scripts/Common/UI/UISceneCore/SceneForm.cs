namespace SDK.Common
{
    /**
     * @brief 场景作为 UI 的Form 基类
     */
    public class SceneForm : ISceneForm
    {
        protected bool m_bReady = false;            // 是否调用了 Ready
        protected bool m_bVisible = true;            // 是否可见
        protected UISceneFormID m_id;

        public bool bReady
        {
            get
            {
                return m_bReady;
            }
            set
            {
                m_bReady = value;
            }
        }

        public bool bVisible
        {
            get
            {
                return m_bVisible;
            }
            set
            {
                m_bVisible = value;
            }
        }

        // 显示
        public void show()
        {
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(m_id);
        }

        // 主要是关联事件
        virtual public void onReady()
        {
            m_bReady = true;
        }

        virtual public void onShow()
        {
            m_bVisible = true;
        }

        public bool isVisible()
        {
            return m_bVisible;
        }
    }
}