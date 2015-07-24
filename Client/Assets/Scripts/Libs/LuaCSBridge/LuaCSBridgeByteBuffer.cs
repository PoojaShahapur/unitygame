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
            string path = "LuaScript/DataStruct/GlobalByteBuffer.lua";
            Ctx.m_instance.m_luaMgr.DoFile(path);
            m_luaTable = Ctx.m_instance.m_luaMgr.GetLuaTable(m_tableName);
        }

        // 更新 Lua 中表的数据
        public void updateLuaTable(ByteBuffer ba)
        {
            CallClassMethod(LuaCSBridgeByteBuffer.CLEAR);       // 清除字节缓冲区
            for(int idx = 0; idx < ba.dynBuff.size; ++idx)
            {
                //m_luaTable[idx] = ba.dynBuff.buff[idx];               // 这样是直接加入表中
                CallClassMethod("writeInt8", ba.dynBuff.buff[idx]);         // 写入每一个字节到缓冲区中
            }
        }
    }
}