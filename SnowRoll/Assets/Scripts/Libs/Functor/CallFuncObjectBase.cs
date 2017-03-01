namespace SDK.Lib
{
    public class CallFuncObjectBase
    {
        protected ICalleeObject mThis;
        protected MAction<IDispatchObject> mHandle;
        protected IDispatchObject mParam;

        public CallFuncObjectBase()
        {
            this.mHandle = null;
            this.mThis = null;
            this.mParam = null;
        }

        public void setPThisAndHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, IDispatchObject param)
        {
            this.mHandle = handle;
            this.mThis = pThis;
            this.mParam = param;
        }

        public void clear()
        {
            this.mThis = null;
            this.mHandle = null;
            this.mParam = null;
        }

        public bool isValid()
        {
            if (null != this.mThis && null != this.mHandle)
            {
                return true;
            }
            else if (null != this.mHandle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void call()
        {

        }
    }
}