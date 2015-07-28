using SDK.Common;

namespace SDK.Lib
{
    public class LuaCSBridgeDispatch : LuaCSBridge
    {
        public LuaCSBridgeDispatch(string tableName)
            : base(tableName)
        {
            Ctx.m_instance.m_luaMgr.DoFile("LuaScript/GlobalEventMgr.lua");
        }

        public void handleGlobalEvent(int eventId, IDispatchObject dispObj)
        {
            // 处理各种事件
            CallMethod("handleGlobalEvent", eventId, "bbb");
        }
    }
}