namespace SDK.Lib
{
    public class CallFuncObjectBase
    {
        protected ICalleeObject mThis;

        public CallFuncObjectBase()
        {
            this.mThis = null;
        }

        virtual public void setPThisAndHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, IDispatchObject param)
        {
            this.mThis = pThis;
        }

        virtual public void setThisAndHandleNoParam(ICalleeObject pThis, MAction handle)
        {

        }

        virtual public void clear()
        {
            this.mThis = null;
        }

        virtual public bool isValid()
        {
            return false;
        }

        virtual public void call()
        {

        }
    }
}