﻿namespace SDK.Lib
{
    /**
     * @brief 网络分发处理器
     */
    public class LuaCSBridgeNetDispHandle : LuaCSBridge
    {
        public LuaCSBridgeNetDispHandle()
            : base("NetDispHandle")
        {
            // Ctx.m_instance.m_luaScriptMgr.DoString();
        }

        // Lua 脚本处理消息
        public void handleMsg(ByteBuffer bu, byte byCmd, byte byParam)
        {

        }
    }
}