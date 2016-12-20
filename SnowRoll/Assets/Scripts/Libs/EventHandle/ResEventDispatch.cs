namespace SDK.Lib
{
    public class ResEventDispatch : EventDispatch
    {
        public ResEventDispatch()
        {

        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            base.dispatchEvent(dispatchObject);
            clearEventHandle();
        }
    }
}