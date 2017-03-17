namespace SDK.Lib
{
    public class CallFuncObjectNoParam : CallFuncObjectBase
    {
        protected MAction mHandleNoParam;

        public CallFuncObjectNoParam()
        {

        }

        override public void setThisAndHandleNoParam(ICalleeObject pThis, MAction handle)
        {
            this.mThis = pThis;
            this.mHandleNoParam = handle;
        }

        override public void clear()
        {
            this.mHandleNoParam = null;

            base.clear();
        }

        override public bool isValid()
        {
            if (null != this.mHandleNoParam)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public void call()
        {
            if (null != this.mHandleNoParam)
            {
                this.mHandleNoParam();
            }
        }
    }
}