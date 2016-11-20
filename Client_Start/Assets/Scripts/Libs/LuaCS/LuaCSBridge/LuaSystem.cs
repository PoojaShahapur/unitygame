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

        //protected LuaTable m_luaCtx;
        //protected LuaTable m_processSys;
        protected bool m_bNeedUpdate;           // 是否需要更新 Lua

        public LuaSystem()
        {
            m_luaScriptMgr = new LuaScriptMgr();
            m_bNeedUpdate = true;
        }

        public void init()
        {
            this.m_luaScriptMgr.InitStart();
            //m_luaCtx = DoFile("MyLua.Libs.FrameWork.GCtx")[0] as LuaTable;  // lua 入口
            this.doFile("MyLua.Module.Entry.Main");        // 启动 Lua AppSys
            this.m_luaClassLoader = new LuaCSBridgeClassLoader();
            this.m_luaCSBridgeMalloc = new LuaCSBridgeMalloc("MyLua.Libs.Core.Malloc", "GlobalNS");
            //m_processSys = m_luaCtx["m_processSys"] as LuaTable;
        }

        public LuaScriptMgr getLuaScriptMgr()
        {
            return m_luaScriptMgr;
        }

        public LuaState lua
        {
            get
            {
                return m_luaScriptMgr.GetMainState();
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

        public object[] callLuaFunction(string name, params object[] args)
        {
            return m_luaScriptMgr.CallLuaFunction(name, args);
        }

        public LuaTable getLuaTable(string tableName)
        {
            return m_luaScriptMgr.GetLuaTable(tableName);
        }

        public object getLuaMember(string memberName_)
        {
            return lua[memberName_];
        }

        public object[] doFile(string fileName)
        {
            return m_luaScriptMgr.DoFile(fileName);
        }

        public object[] doString(string str)
        {
            return m_luaScriptMgr.DoFile(str);
        }

        // 从 Lua 中发送 pb 消息
        //public void sendFromLua(ushort commandID, LuaStringBuffer buffer)
        //{
        //    UtilMsg.sendMsg(commandID, buffer);
        //}

        public void sendFromLua(ushort commandID, LuaInterface.LuaByteBuffer buffer)
        {
            UtilMsg.sendMsg(commandID, buffer);
        }

        //public void sendFromLuaParam(LuaTable luaTable, LuaStringBuffer buffer)
        //public void sendFromLuaRpc(LuaStringBuffer buffer)
        //{
        //    UtilMsg.sendMsgRpc(buffer);
        //}

        public void sendFromLuaRpc(LuaInterface.LuaByteBuffer buffer)
        {
            UtilMsg.sendMsgRpc(buffer);
        }

        //public void receiveToLua(ByteBuffer msg)
        //{
        //    LuaStringBuffer buffer = new LuaStringBuffer(msg.dynBuff.m_buff);
        //    this.CallLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLua", 0, buffer);
        //}

        public void receiveToLua(ByteBuffer msg)
        {
            LuaInterface.LuaByteBuffer buffer = new LuaInterface.LuaByteBuffer(msg.dynBuff.m_buff);
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLua", 0, buffer);
        }

        public void receiveToLuaRpc(ByteBuffer msg)
        {
            //msg.end();
            // 拷贝数据，因为 LuaStringBuffer 不支持偏移和长度
            byte[] cmdBuf = new byte[msg.length];
            Array.Copy(msg.dynBuff.m_buff, 0, cmdBuf, 0, msg.length);
            //LuaStringBuffer buffer = new LuaStringBuffer(msg.dynBuff.m_buff);
            LuaInterface.LuaByteBuffer buffer = new LuaInterface.LuaByteBuffer(msg.dynBuff.m_buff);

            //LuaStringBuffer buffer = new LuaStringBuffer(cmdBuf);
            //MLuaStringBuffer buffer = new MLuaStringBuffer(cmdBuf);
            //MLuaStringBuffer buffer = new MLuaStringBuffer(cmdBuf, cmdBuf.Length);
            //MLuaStringBuffer buffer = new MLuaStringBuffer(msg.dynBuff.m_buff, (int)msg.length);

            this.callLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLuaRpc", buffer, msg.length);
        }

        public void receiveToLua(Byte[] msg)
        {
            //LuaStringBuffer buffer = new LuaStringBuffer(msg);
            LuaInterface.LuaByteBuffer buffer = new LuaInterface.LuaByteBuffer(msg);
            //this.CallLuaFunction("NetMgr.receiveCmd", 0, buffer);
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLua", 1000, buffer);
        }

        public void onSceneLoaded()
        {
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onSceneLoaded");
        }

        public void onSocketConnected()
        {
            
        }

        public LuaTable loadModule(string file)
        {
            return m_luaClassLoader.loadModule(file);
        }

        public LuaTable malloc(LuaTable table)
        {
            return m_luaCSBridgeMalloc.malloc(table);
        }

        public void advance(float delta)
        {
            if (m_bNeedUpdate)
            {
                this.callLuaFunction("GlobalNS.GlobalEventCmd.onAdvance", delta);
            }
        }

        // 添加单击事件
        public void addClick(GameObject go, LuaFunction luafunc)
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

            LuaTable luaTable = Ctx.m_instance.m_luaSystem.getLuaTable(fullTableName);
            //string[] strArray = luaTable.ToArray<string>();
            string[] strArray = luaTable.ToArray() as string[];
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

            LuaTable luaTable = this.getLuaTable(fullTableName);
            //int[] strArray = luaTable.ToArray<int>();
            object[] objArray = luaTable.ToArray();
            int[] strArray = new int[objArray.Length];
            objArray.CopyTo(strArray, 0);
            return strArray;
        }

        // 是否是系统属性
        public bool isSystemAttr(string attrName)
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

        // 用来传递虚拟机 MLuaStringBuffer 中内容的
        public static void push(IntPtr L, MLuaStringBuffer lsb)
        {
            if (lsb != null && lsb.buffer != null)
            {
                //LuaDLL.lua_pushlstring(L, lsb.buffer, lsb.mLen);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }
        }
    }
}