using System;

namespace SDK.Lib
{
    /**
     * @brief ByteBuffer 和 Lua 沟通
     */
    public class LuaCSBridgeByteBuffer : LuaCSBridge
    {
        public const string CLEAR = "clearFromCS";
        public const string WRITEINT8 = "writeInt8FromCS";
        public const string WRITEMULTIBYTE = "writeMultiByteFromCS";

        public LuaCSBridgeByteBuffer() : 
            base ("", "NetMsgData")
            //base("ByteBuffer")
        {
            string path = "LuaScript/DataStruct/NetMsgData.lua";
            Ctx.mInstance.mLuaSystem.doFile(path);
            m_luaTable = Ctx.mInstance.mLuaSystem.getLuaTable(mTableName);
            // 设置系统字节序
            setSysEndian((int)SystemEndian.msLocalEndian);
        }

        // 更新 Lua 中表的数据
        public void updateLuaByteBuffer(ByteBuffer bu)
        {
            callClassMethod("", LuaCSBridgeByteBuffer.CLEAR);       // 清除字节缓冲区
            for(int idx = 0; idx < bu.dynBuffer.size; ++idx)
            {
                //m_luaTable[idx] = bu.dynBuff.buff[idx];               // 这样是直接加入表中
                //CallClassMethod("writeInt8", bu.dynBuff.buff[idx]);         // 写入每一个字节到缓冲区中，直接传递数字类型调用函数，这个数字会被作为 UserData ，如果传递数字，需要传递字符串才行
                //object ret = CallClassMethod("writeInt8", bu.dynBuff.buff[idx].ToString());
                //int aaa = 10;
                //writeInt8ToLua(mTableName, WRITEINT8, bu.dynBuff.buff[idx]);
            }

            writeByteArrToLua(mTableName, WRITEMULTIBYTE, bu.dynBuffer.buffer, (int)bu.dynBuffer.size);
        }

        // writeInt8 函数调用，写一个字节到 Lua 表中
        protected void writeInt8ToLua(string tableName_, string funcName_, int oneByte)
        {
            //string fullFuncName = "";               // 完全的有表的完全名字
            //if (!String.IsNullOrEmpty(tableName_))  // 如果在 _G 表中
            //{
            //    fullFuncName = tableName_ + "." + funcName_;
            //    LuaTable luaTable = Ctx.mInstance.mLuaSystem.GetLuaTable(tableName_);

            //    IntPtr L = Ctx.mInstance.mLuaSystem.lua.L;
            //    int oldTop = LuaDLL.lua_gettop(L);

            //    // 获取表
            //    LuaDLL.lua_pushstring(L, tableName_);
            //    LuaDLL.lua_rawget(L, LuaIndexes.LUA_GLOBALSINDEX);      // 从 _G 表中获取数据
            //    // 检查类型
            //    LuaTypes type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TTABLE)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }
            //    // 获取函数
            //    LuaDLL.lua_pushstring(L, funcName_);
            //    LuaDLL.lua_rawget(L, -2);
            //    type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TFUNCTION)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }
            //    // 放 Lua 表
            //    luaTable.push(L);
            //    type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TTABLE)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }
            //    // 放数字
            //    LuaDLL.lua_pushinteger(L, oneByte);
            //    type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TNUMBER)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }

            //    int nArgs = 0;
            //    nArgs = 2;
            //    int error = LuaDLL.lua_pcall(L, nArgs, -1, -nArgs - 2);
            //    if (error != 0)
            //    {
            //        string err = LuaDLL.lua_tostring(L, -1);
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }

            //    LuaDLL.lua_settop(L, oldTop);
            //}

            // 新版本
            if (!String.IsNullOrEmpty(tableName_))  // 如果在 _G 表中
            {
                this.callClassMethod(tableName_, funcName_, oneByte);
            }
        }

        // 直接写一个 byte[] 数组到 Lua
        protected void writeByteArrToLua(string tableName_, string funcName_, byte[] bytes, int size_)
        {
            //string fullFuncName = "";               // 完全的有表的完全名字
            //if (!String.IsNullOrEmpty(tableName_))  // 如果在 _G 表中
            //{
            //    fullFuncName = tableName_ + "." + funcName_;
            //    LuaTable luaTable = Ctx.mInstance.mLuaSystem.GetLuaTable(tableName_);

            //    IntPtr L = Ctx.mInstance.mLuaSystem.lua.L;
            //    int oldTop = LuaDLL.lua_gettop(L);

            //    // 获取表
            //    LuaDLL.lua_pushstring(L, tableName_);
            //    LuaDLL.lua_rawget(L, LuaIndexes.LUA_GLOBALSINDEX);      // 从 _G 表中获取数据
            //    // 检查类型
            //    LuaTypes type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TTABLE)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }
            //    // 获取函数
            //    LuaDLL.lua_pushstring(L, funcName_);
            //    LuaDLL.lua_rawget(L, -2);
            //    type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TFUNCTION)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }
            //    // 放 Lua 表
            //    luaTable.push(L);
            //    type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TTABLE)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }
            //    // 放一个字节数组
            //    LuaDLL.lua_pushlstring(L, bytes, size_);
            //    type = LuaDLL.lua_type(L, -1);
            //    if (type != LuaTypes.LUA_TSTRING)
            //    {
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }

            //    int nArgs = 0;
            //    nArgs = 2;
            //    int error = LuaDLL.lua_pcall(L, nArgs, -1, -nArgs - 2);
            //    if (error != 0)
            //    {
            //        string err = LuaDLL.lua_tostring(L, -1);
            //        LuaDLL.lua_settop(L, oldTop);
            //        return;
            //    }

            //    LuaDLL.lua_settop(L, oldTop);
            //}

            // 新版本
            if (!String.IsNullOrEmpty(tableName_))  // 如果在 _G 表中
            {
                this.callClassMethod(tableName_, funcName_, bytes, size_);
            }
        }

        public void setSysEndian(int endian_)
        {
            writeInt8ToLua("ByteBuffer", "setSysEndian", endian_);
        }

        public void updateLuaByteBuffer(byte[] bytes, int bytesLen)
        {
            callClassMethod("", LuaCSBridgeByteBuffer.CLEAR);
            writeByteArrToLua(mTableName, WRITEMULTIBYTE, bytes, bytesLen);
        }
    }
}