namespace SDK.Lib
{
    /**
     * @brief 网络分发处理器
     */
    public class LuaCSBridgeNetDispHandle : LuaCSBridge
    {
        public LuaCSBridgeNetDispHandle()
            : base("NetDispHandle")
        {
            // LuaScriptMgr.Instance.DoString();
        }

        // Lua 脚本处理消息
        public void handleMsg(ByteBuffer ba, byte byCmd, byte byParam)
        {

        }
    }
}