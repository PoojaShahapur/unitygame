namespace SDK.Lib
{
    /**
     * @brief 网络数据和 Lua 通信的数据
     */
    public class LuaCSBridgeNetWork : LuaCSBridge
    {
        public LuaCSBridgeNetWork() : 
            base ("", "LuaCSBridgeNetWork")
        {

        }

        // Lua 脚本处理消息
        public void handleMsg(ByteBuffer bu, byte byCmd, byte byParam)
        {
            MLuaStringBuffer luaBuffer = new MLuaStringBuffer(bu.dynBuff.buff, (int)bu.length);
            CallClassMethod("", "handleMsg", byCmd, byParam, luaBuffer);    // 回调 Lua 函数
        }
    }
}