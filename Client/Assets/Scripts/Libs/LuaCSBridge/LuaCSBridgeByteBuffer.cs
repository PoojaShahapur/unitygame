using LuaInterface;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief ByteBuffer 和 Lua 沟通
     */
    public class LuaCSBridgeByteBuffer : LuaCSBridge
    {
        public const 

        protected LuaTable m_luaTable;      // LuaTable

        public LuaCSBridgeByteBuffer()
            : base ("NetMsgData")
        {
            m_luaTable = Ctx.m_instance.m_luaMgr.GetLuaTable(m_tableName);
        }

        // 更新 Lua 中表的数据
        public void updateLuaTable(ByteBuffer ba)
        {
            CallGlobalMethod();
        }
    }
}