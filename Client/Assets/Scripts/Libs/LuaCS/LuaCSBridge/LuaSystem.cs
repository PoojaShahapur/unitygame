using LuaInterface;
using System;
using UnityEngine;
using UnityEngine.UI;

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
            m_luaCtx = DoFile("MyLua.Libs.FrameWork.GCtx")[0] as LuaTable;  // lua 入口
            m_luaClassLoader = new LuaCSBridgeClassLoader();
            m_luaCSBridgeMalloc = new LuaCSBridgeMalloc("MyLua.Libs.Core.Malloc", "GlobalNS");
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

        public object GetLuaMember(string memberName_)
        {
            return lua[memberName_];
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
        public void sendFromLua(UInt16 commandID, LuaStringBuffer buffer)
        {
            Ctx.m_instance.m_shareData.m_tmpBA = Ctx.m_instance.m_netMgr.getSendBA();
            Ctx.m_instance.m_shareData.m_tmpBA.writeBytes(buffer.buffer, 0, (uint)buffer.buffer.Length);
        }

        public void receiveToLua(ByteBuffer msg)
        {
            LuaStringBuffer buffer = new LuaStringBuffer(msg.dynBuff.m_buff);
            this.CallLuaFunction("NetMgr.receiveCmd", 0, buffer);
        }

        public void receiveToLua(Byte[] msg)
        {
            LuaStringBuffer buffer = new LuaStringBuffer(msg);
            this.CallLuaFunction("NetMgr.receiveCmd", 0, buffer);
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

        // 添加单击事件
        public void AddClick(GameObject go, LuaFunction luafunc)
        {
            if (go == null) return;
            //buttons.Add(luafunc);
            go.GetComponent<Button>().onClick.AddListener(
                delegate()
                {
                    luafunc.Call(go);
                }
            );
        }

        // 获取一个表，然后转换成数组
        public string[] getTable2StrArray(string tableName, string parentTable = "")
        {
            string fullTableName = "";              // 有表前缀的成员名字
            if (String.IsNullOrEmpty(parentTable))   // 如果在 _G 表中
            {
                fullTableName = tableName;
            }
            else        // 在一个 _G 的一个表中
            {
                fullTableName = parentTable + "." + tableName;
            }

            LuaTable luaTable = Ctx.m_instance.m_luaSystem.GetLuaTable(fullTableName);
            string[] strArray = luaTable.ToArray<string>();
            return strArray;
        }

        // 获取一个表，然后转换成数组
        public int[] getTable2IntArray(string tableName, string parentTable = "")
        {
            string fullTableName = "";                      // 有表前缀的成员的名字
            if (String.IsNullOrEmpty(parentTable))          // 如果在 _G 表中 
            {
                fullTableName = tableName;
            }
            else            // 在一个 _G 的一个表中
            {
                fullTableName = parentTable + "." + tableName;
            }

            LuaTable luaTable = Ctx.m_instance.m_luaSystem.GetLuaTable(fullTableName);
            int[] strArray = luaTable.ToArray<int>();
            return strArray;
        }

        // 是否是系统属性
        public bool IsSystemAttr(string attrName)
        {
            // 这些属性是自己添加到 Lua 表中的，因此遍历的时候，如果有这些属性就不处理了
            if ("ctor" == attrName ||
               "super" == attrName ||
               "dataType" == attrName ||
               "clsCode" == attrName ||
               "clsName" == attrName)
            {
                return true;
            }

            return false;
        }
    }
}