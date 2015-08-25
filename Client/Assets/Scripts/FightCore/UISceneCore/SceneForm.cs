using SDK.Lib;
namespace FightCore
{
    /**
     * @brief 场景作为 UI 的Form 基类
     */
    public class SceneForm
    {
        protected bool m_bReady = false;            // 是否调用了 Ready
        protected bool m_bVisible = false;            // 是否可见
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

        public UISceneFormID id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        public void ready()
        {
            Ctx.m_instance.m_uiSceneMgr.readySceneForm(m_id);
        }

        // 显示
        public void show()
        {
            Ctx.m_instance.m_uiSceneMgr.showSceneForm(m_id);
        }

        // 隐藏
        public void hide()
        {
            Ctx.m_instance.m_uiSceneMgr.hideSceneForm(m_id);
        }

        public void exit()
        {
            Ctx.m_instance.m_uiSceneMgr.exitSceneForm(m_id);
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

        virtual public void onHide()
        {
            m_bVisible = false;
        }

        virtual public void onExit()
        {
            m_bVisible = false;
            m_bReady = false;
        }

        public bool isVisible()
        {
            return m_bVisible;
        }
    }
}