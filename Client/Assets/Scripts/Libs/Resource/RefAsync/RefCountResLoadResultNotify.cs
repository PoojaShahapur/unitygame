namespace SDK.Lib
{
    /**
     * @brief 引用计数资源加载结果通知
     */
    public class RefCountResLoadResultNotify : ResLoadResultNotify
    {
        protected RefCount m_refCount;                  // 引用计数

        public RefCount refCount
        {
            get
            {
                return m_refCount;
            }
            set
            {
                m_refCount = value;
            }
        }

        override public void copyFrom(ResLoadResultNotify rhv)
        {
            base.copyFrom(rhv);
            m_refCount.copyFrom((rhv as RefCountResLoadResultNotify).refCount);
        }
    }
}