namespace SDK.Lib
{
    /**
     * @brief 卡牌使用的渲染器，除了 hero
     */
    public class CardRenderBase : EntityRenderBase, IDispatchObject
    {
        protected EventDispatch m_clickEntityDisp;  // 点击事件分发

        public CardRenderBase()
        {
            m_clickEntityDisp = new EventDispatch();
        }

        public EventDispatch clickEntityDisp
        {
            get
            {
                return m_clickEntityDisp;
            }
        }
    }
}