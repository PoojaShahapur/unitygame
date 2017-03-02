namespace SDK.Lib
{
    /**
     * @brief 引用计数资源加载结果通知
     */
    public class RefCountResLoadResultNotify : ResLoadResultNotify
    {
        protected RefCount mRefCount;                  // 引用计数

        public RefCountResLoadResultNotify()
        {
            this.mRefCount = new RefCount();
        }

        public RefCount refCount
        {
            get
            {
                return this.mRefCount;
            }
            set
            {
                this.mRefCount = value;
            }
        }

        override public void copyFrom(ResLoadResultNotify rhv)
        {
            base.copyFrom(rhv);

            this.mRefCount.copyFrom((rhv as RefCountResLoadResultNotify).refCount);
        }
    }
}