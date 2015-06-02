using System;

namespace SDK.Lib
{
    public class AddOnceAndCallOnceEventDispatch : EventDispatch
    {
        override public void addEventHandle(Action<IDispatchObject> handle)
        {
            if (!existEventHandle(handle))
            {
                base.addEventHandle(handle);
            }
        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            base.dispatchEvent(dispatchObject);
            clearEventHandle();
        }
    }
}
