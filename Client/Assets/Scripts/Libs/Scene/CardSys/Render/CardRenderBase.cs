using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 卡牌使用的渲染器，所有卡牌渲染器的基类
     */
    public class CardRenderBase : EntityRenderBase, IDispatchObject
    {
        protected EventDispatch m_clickEntityDisp;  // 点击事件分发
        protected EventDispatch m_downEntityDisp;  // 按下事件分发
        protected EventDispatch m_upEntityDisp;  // 按下事件分发

        public CardRenderBase(SceneEntityBase entity_) :
            base(entity_)
        {
            m_clickEntityDisp = new AddOnceEventDispatch();
            m_downEntityDisp = new AddOnceEventDispatch();
            m_upEntityDisp = new AddOnceEventDispatch();
        }

        override public void dispose()
        {
            base.dispose();
            m_clickEntityDisp.clearEventHandle();
            m_downEntityDisp.clearEventHandle();
            m_upEntityDisp.clearEventHandle();
        }

        public EventDispatch clickEntityDisp
        {
            get
            {
                return m_clickEntityDisp;
            }
        }

        public EventDispatch downEntityDisp
        {
            get
            {
                return m_downEntityDisp;
            }
        }

        public EventDispatch upEntityDisp
        {
            get
            {
                return m_upEntityDisp;
            }
        }

        public void onEntityClick(GameObject go)
        {
            m_clickEntityDisp.dispatchEvent(this);
        }

        public void onEntityDownUp(GameObject go, bool state)
        {
            if (state)
            {
                m_downEntityDisp.dispatchEvent(this);
            }
            else
            {
                m_upEntityDisp.dispatchEvent(this);
            }
        }
    }
}