using LuaInterface;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief ByteBuffer 和 Lua 沟通
     */
    public class LuaCSBridgeByteBuffer : LuaCSBridge
    {
        public const string CLEAR = "clear";

        protected LuaTable m_luaTable;      // LuaTable

        public LuaCSBridgeByteBuffer()
            : base ("NetMsgData")
        {
            m_luaTable = Ctx.m_instance.m_luaMgr.GetLuaTable(m_tableName);
        }

        // 更新 Lua 中表的数据
        public void updateLuaTable(ByteBuffer ba)
        {
            CallClassMethod(LuaCSBridgeByteBuffer.CLEAR);       // 清除字节缓冲区
            for(int idx = 0; idx < ba.dynBuff.size; ++idx)
            {
                m_luaTable[idx] = ba.dynBuff.buff[idx];
            }
        }
    }
}