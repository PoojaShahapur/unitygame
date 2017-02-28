using LuaInterface;

namespace SDK.Lib
{
    public class AddOnceAndCallOnceEventDispatch : EventDispatch
    {
        override public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> handle, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            if (!this.isExistEventHandle(pThis, handle, eventId, luaTable, luaFunction, luaEventId))
            {
                base.addEventHandle(pThis, handle, eventId, luaTable, luaFunction, luaEventId);
            }
        }

        override public void dispatchEvent(IDispatchObject dispatchObject)
        {
            base.dispatchEvent(dispatchObject);

            this.clearEventHandle();
        }
    }
}