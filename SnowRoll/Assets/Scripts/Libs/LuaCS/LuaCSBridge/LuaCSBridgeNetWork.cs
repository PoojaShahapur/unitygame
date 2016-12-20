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
            //LuaStringBuffer luaBuffer = new LuaStringBuffer(bu.dynBuff.buff);
            LuaInterface.LuaByteBuffer luaBuffer = new LuaInterface.LuaByteBuffer(bu.dynBuffer.buffer);
            callClassMethod("", "handleMsg", byCmd, byParam, luaBuffer);    // 回调 Lua 函数
        }
    }
}