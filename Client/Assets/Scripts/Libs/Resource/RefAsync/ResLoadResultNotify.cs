namespace SDK.Lib
{
    /**
     * @brief 非引用计数资源加载结果通知
     */
    public class ResLoadResultNotify
    {
        protected ResLoadState m_resLoadState;          // 资源加载状态
        protected EventDispatch m_loadEventDispatch;    // 事件分发器

        public ResLoadState resLoadState
        {
            get
            {
                return m_resLoadState;
            }
            set
            {
                m_resLoadState = value;
            }
        }

        public EventDispatch loadEventDispatch
        {
            get
            {
                return m_loadEventDispatch;
            }
            set
            {
                m_loadEventDispatch = value;
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            m_loadEventDispatch.dispatchEvent(dispObj);
            m_loadEventDispatch.clearEventHandle();
        }

        virtual public void copyFrom(ResLoadResultNotify rhv)
        {
            m_resLoadState.copyFrom(rhv.resLoadState);
            m_loadEventDispatch = rhv.loadEventDispatch;
        }
    }
}