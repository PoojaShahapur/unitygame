using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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