namespace SDK.Lib
{
    /**
     * @brief 一次事件分发，分发一次就清理
     */
    public class CallOnceEventDispatch : EventDispatch
    {
        public CallOnceEventDispatch()
        {

        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            base.dispatchEvent(dispatchObject);
            clearEventHandle();
        }
    }
}