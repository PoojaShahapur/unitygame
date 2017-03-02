namespace SDK.Lib
{
    public class CallFuncObjectFixParam : CallFuncObjectBase
    {
        protected MAction<IDispatchObject> mHandle;
        protected IDispatchObject mParam;

        public CallFuncObjectFixParam()
        {
            this.mHandle = null;
            this.mParam = null;
        }

        override public void clear()
        {
            this.mThis = null;
        }

        override public bool isValid()
        {
            if (null != this.mHandle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public void setPThisAndHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, IDispatchObject param)
        {
            base.setPThisAndHandle(pThis, handle, param);

            this.mHandle = handle;
            this.mParam = param;
        }

        override public void call()
        {
            if (null != this.mHandle)
            {
                this.mHandle(this.mParam);
            }
        }
    }
}