namespace SDK.Lib
{
    /**
     *@breif 全局事件 Id
     */
    public enum eGlobalEventType
    {
        eGlobalTest,        // 全局事件测试
        eGlobalTotal,
    }

    public class LuaCSBridgeDispatch : LuaCSBridge
    {
        public const string LUA_DISPATCH_TABLE_NAME = "GlobalEventMgr";     // 接收全局事件的 Lua 表名字
        public const string LUA_DISPATCH_FUNC_NAME = "handleGlobalEvent";   // 处理全局事件的函数

        public LuaCSBridgeDispatch(string tableName)
            : base("", tableName)
        {
            Ctx.m_instance.m_luaSystem.DoFile("GlobalEvent/GlobalEventMgr.lua");
        }

        // 事件分发
        public void handleGlobalEvent(int eventId, IDispatchObject dispObj)
        {
            // 处理各种事件
            CallMethod(LuaCSBridgeDispatch.LUA_DISPATCH_FUNC_NAME, eventId, "bbb");
        }
    }
}