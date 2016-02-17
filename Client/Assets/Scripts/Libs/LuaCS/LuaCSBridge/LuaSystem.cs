using LuaInterface;
using System;

namespace SDK.Lib
{
    /**
     * @brief Lua 系统
     */
    public class LuaSystem
    {
        protected LuaScriptMgr m_luaScriptMgr;
        protected LuaCSBridgeClassLoader m_luaClassLoader;  // Lua 类文件加载器
        protected LuaCSBridgeMalloc m_luaCSBridgeMalloc;

        protected LuaTable m_luaCtx;
        protected LuaTable m_processSys;
        protected bool m_bNeedUpdate;           // 是否需要更新 Lua

        public LuaSystem()
        {
            m_luaScriptMgr = new LuaScriptMgr();
            m_bNeedUpdate = true;
        }

        public void init()
        {
            m_luaScriptMgr.Start();
            DoFile("MyLua.Libs.Core.Prequisites");  // 一次性加载所有文件
            m_luaClassLoader = new LuaCSBridgeClassLoader();
            m_luaCSBridgeMalloc = new LuaCSBridgeMalloc("MyLua.Libs.Core.Malloc", "GlobalNS");
            m_luaCtx = loadModule("MyLua.Libs.FrameWork.GCtx");     // 初始化 Lua 脚本
            m_processSys = m_luaCtx["m_processSys"] as LuaTable;
        }

        public LuaScriptMgr getLuaScriptMgr()
        {
            return m_luaScriptMgr;
        }

        public LuaState lua
        {
            get
            {
                return m_luaScriptMgr.lua;
            }
        }

        public LuaCSBridgeClassLoader getLuaClassLoader()
        {
            return m_luaClassLoader;
        }

        public void setNeedUpdate(bool value)
        {
            m_bNeedUpdate = value;
        }

        public object[] CallLuaFunction(string name, params object[] args)
        {
            return m_luaScriptMgr.CallLuaFunction(name, args);
        }

        public LuaTable GetLuaTable(string tableName)
        {
            return m_luaScriptMgr.GetLuaTable(tableName);
        }

        public object[] DoFile(string fileName)
        {
            return m_luaScriptMgr.DoFile(fileName);
        }

        public object[] DoString(string str)
        {
            return m_luaScriptMgr.DoFile(str);
        }

        // 从 Lua 中发送 pb 消息
        public void SendFromLua(UInt16 commandID, LuaStringBuffer buffer)
        {
            Ctx.m_instance.m_shareData.m_tmpBA = Ctx.m_instance.m_netMgr.getSendBA();
            Ctx.m_instance.m_shareData.m_tmpBA.writeBytes(buffer.buffer, 0, (uint)buffer.buffer.Length);
        }

        public void ReceiveViaLua(ByteBuffer msg)
        {
            LuaStringBuffer buffer = new LuaStringBuffer(msg.dynBuff.m_buff);
            this.CallLuaFunction("NetManager.receiveMsg", 0, buffer);
        }

        public LuaTable loadModule(string file)
        {
            return m_luaClassLoader.loadModule(file);
        }

        public LuaTable malloc(LuaTable table)
        {
            return m_luaCSBridgeMalloc.malloc(table);
        }

        public void Advance(float delta)
        {
            if (m_bNeedUpdate)
            {
                this.CallLuaFunction("GlobalNS.ProcessSys.advance", m_processSys, delta);
            }
        }
    }
}