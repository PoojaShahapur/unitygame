using LuaInterface;
using System;

namespace SDK.Lib
{
    public class AddOnceAndCallOnceEventDispatch : EventDispatch
    {
        override public void addEventHandle(Action<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            if (!existEventHandle(handle, luaTable, luaFunction))
            {
                base.addEventHandle(handle, luaTable, luaFunction);
            }
        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            base.dispatchEvent(dispatchObject);
            clearEventHandle();
        }
    }
}
