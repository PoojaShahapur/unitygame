using LuaInterface;

namespace SDK.Lib
{
    public class AddOnceAndCallOnceEventDispatch : EventDispatch
    {
        override public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            if (!this.isExistEventHandle(pThis, handle, luaTable, luaFunction))
            {
                base.addEventHandle(pThis, handle, luaTable, luaFunction);
            }
        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            base.dispatchEvent(dispatchObject);

            this.clearEventHandle();
        }
    }
}