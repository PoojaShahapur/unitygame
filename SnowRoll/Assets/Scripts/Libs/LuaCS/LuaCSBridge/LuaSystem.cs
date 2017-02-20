﻿using LuaInterface;
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
        protected LuaScriptMgr mLuaScriptMgr;
        protected LuaCSBridgeClassLoader mLuaClassLoader;  // Lua 类文件加载器
        protected LuaCSBridgeMalloc mLuaCSBridgeMalloc;

        //protected LuaTable mLuaCtx;
        //protected LuaTable mProcessSys;
        protected bool mIsNeedUpdateLua;           // 是否需要更新 Lua
        protected MDataStream mDataStream;
        protected bool mIsLuaInited;            // Lua 脚本是否初始化完成

        public LuaSystem()
        {
            mLuaScriptMgr = LuaScriptMgr.getSinglePtr();
            mIsNeedUpdateLua = true;
            mIsLuaInited = false;
        }

        public void init()
        {
            this.mLuaScriptMgr.init();
            //mLuaCtx = DoFile("MyLua.Libs.FrameWork.GCtx")[0] as LuaTable;  // lua 入口
            this.doFile("MyLua.Module.Entry.MainEntry");        // 启动 Lua AppSys
            this.mLuaClassLoader = new LuaCSBridgeClassLoader();
            this.mLuaCSBridgeMalloc = new LuaCSBridgeMalloc("MyLua.Libs.Core.Malloc", "GlobalNS");
            //mProcessSys = mLuaCtx["mProcessSys"] as LuaTable;
        }

        public LuaScriptMgr getLuaScriptMgr()
        {
            return mLuaScriptMgr;
        }

        public LuaState lua
        {
            get
            {
                return mLuaScriptMgr.GetMainState();
            }
        }

        public LuaCSBridgeClassLoader getLuaClassLoader()
        {
            return mLuaClassLoader;
        }

        public void setNeedUpdateLua(bool value)
        {
            mIsNeedUpdateLua = value;
        }

        public void setLuaInited(bool value)
        {
            this.mIsLuaInited = value;
        }

        public object[] callLuaFunction(string name, params object[] args)
        {
            return mLuaScriptMgr.CallLuaFunction(name, args);
        }

        public LuaTable getLuaTable(string tableName)
        {
            return mLuaScriptMgr.GetLuaTable(tableName);
        }

        public object getLuaMember(string memberName_)
        {
            return lua[memberName_];
        }

        // 轻易不要使用这个接口，因为这个接口最终使用的是 luaL_loadbuffer，不是 require，require 不会重复加载文件，loadfile 和 dofile 都会重复加载文件，不会进行检查，可能会覆盖之前表中的内容
        public object[] doFile(string fileName)
        {
            return mLuaScriptMgr.DoFile(fileName);
        }

        // 轻易不要使用这个接口，这个接口也会重复执行，可能会覆盖之前表中的内容
        public object[] doString(string str)
        {
            return mLuaScriptMgr.DoFile(str);
        }

        public object[] requireFile(string filePath)
        {
            return this.callLuaFunction("GlobalNS.GlobalEventCmd.requireFile", filePath);
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
        //    LuaStringBuffer buffer = new LuaStringBuffer(msg.dynBuff.mBuffer);
        //    this.CallLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLua", 0, buffer);
        //}

        public void receiveToLua(ByteBuffer msg)
        {
            LuaInterface.LuaByteBuffer buffer = new LuaInterface.LuaByteBuffer(msg.dynBuffer.mBuffer);
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLua", 0, buffer);
        }

        public void receiveToLuaRpc(ByteBuffer msg)
        {
            //msg.end();
            // 拷贝数据，因为 LuaStringBuffer 不支持偏移和长度
            byte[] cmdBuf = new byte[msg.length];
            Array.Copy(msg.dynBuffer.mBuffer, 0, cmdBuf, 0, msg.length);
            //LuaStringBuffer buffer = new LuaStringBuffer(msg.dynBuff.mBuffer);
            LuaInterface.LuaByteBuffer buffer = new LuaInterface.LuaByteBuffer(msg.dynBuffer.mBuffer);

            //LuaStringBuffer buffer = new LuaStringBuffer(cmdBuf);
            //MLuaStringBuffer buffer = new MLuaStringBuffer(cmdBuf);
            //MLuaStringBuffer buffer = new MLuaStringBuffer(cmdBuf, cmdBuf.Length);
            //MLuaStringBuffer buffer = new MLuaStringBuffer(msg.dynBuff.mBuffer, (int)msg.length);

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

        public void onPlayerMainLoaded()
        {
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onPlayerMainLoaded");
        }

        public void onSocketConnected()
        {
            
        }

        public LuaTable loadModule(string file)
        {
            return mLuaClassLoader.loadModule(file);
        }

        public LuaTable malloc(LuaTable table)
        {
            return mLuaCSBridgeMalloc.malloc(table);
        }

        public void advance(float delta)
        {
            if (this.mIsLuaInited)
            {
                if (this.mIsNeedUpdateLua)
                {
                    if (Ctx.mInstance.mSystemFrameData.getTotalFrameCount() > 100)
                    {
                        this.callLuaFunction("GlobalNS.GlobalEventCmd.onAdvance", delta);
                    }
                }
            }
        }

        public void openForm(int formId)
        {
            this.callLuaFunction("GlobalNS.GlobalEventCmd.openForm", formId);
        }

        public void exitForm(int formId)
        {
            this.callLuaFunction("GlobalNS.GlobalEventCmd.exitForm", formId);
        }

        // 场景加载进度
        public void onSceneLoadProgress(float progress)
        {
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onSceneLoadProgress", progress);
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

            LuaTable luaTable = Ctx.mInstance.mLuaSystem.getLuaTable(fullTableName);
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
            if (
                "super" == attrName ||
                "dataType" == attrName ||
                "clsCode" == attrName ||
                "clsName" == attrName ||
                "init" == attrName ||
                "dispose" == attrName ||
                "ctor" == attrName ||
                "dtor" == attrName
                )
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

        public byte[] readFile(string fileName)
        {
            //this.mDataStream = new MDataStream(string.Format("{0}/Lua/{1}", MFileSys.msDataStreamStreamingAssetsPath, fileName));
            //return this.mDataStream.readByte();

            byte[] str = null;
            AuxBytesLoader auxBytesLoader = new AuxBytesLoader();

            if (fileName.EndsWith(".lua"))
            {
                int index = fileName.LastIndexOf('.');
                fileName = fileName.Substring(0, index);
            }

            string path = "Lua/" + fileName;
            path = path.Replace('.', '/');
            path = path + ".txt";

            auxBytesLoader.syncLoad(path);
            str = auxBytesLoader.getBytes();
            auxBytesLoader.dispose();

            return str;
        }

        public void receiveToLua_KBE(string msgname, object[] param)
        {
            // 传递参数必须要 (object)param ，不能 param
            this.callLuaFunction("GlobalNS.GlobalEventCmd.onReceiveToLua_KBE", msgname, (object)param);
        }

        public void PrintConsoleMessage(string msg)
        {
            receiveToLua_KBE("handleSendAndGetMessage", new object[] { msg });
        }
    }
}