namespace SDK.Lib
{
    /**
     * @brief 非引用计数资源加载结果通知
     */
    public class ResLoadResultNotify
    {
        protected ResLoadState mResLoadState;          // 资源加载状态
        protected ResEventDispatch mLoadResEventDispatch;      // 事件分发器

        public ResLoadResultNotify()
        {
            mResLoadState = new ResLoadState();
            mLoadResEventDispatch = new ResEventDispatch();
        }

        public ResLoadState resLoadState
        {
            get
            {
                return mResLoadState;
            }
            set
            {
                mResLoadState = value;
            }
        }

        public ResEventDispatch loadResEventDispatch
        {
            get
            {
                return mLoadResEventDispatch;
            }
            set
            {
                mLoadResEventDispatch = value;
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            mLoadResEventDispatch.dispatchEvent(dispObj);
            mLoadResEventDispatch.clearEventHandle();
        }

        virtual public void copyFrom(ResLoadResultNotify rhv)
        {
            mResLoadState.copyFrom(rhv.resLoadState);
            mLoadResEventDispatch = rhv.loadResEventDispatch;
        }
    }
}