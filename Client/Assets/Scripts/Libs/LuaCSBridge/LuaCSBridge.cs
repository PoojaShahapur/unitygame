using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 逻辑 Lua 和 CS 之间的交互，是以表为单位进行交互的，一定要注意表名正确，每一个表就是一个功能
     */
    public class LuaCSBridge
    {
        protected string m_tableName;   // 表的名字
        //protected LuaTable m_moduleEnv;     // 执行模块的环境

        /**
         * @brief 表的名字
         */
        public LuaCSBridge(string tableName)
        {
            m_tableName = tableName;
        }

        virtual public void init()
        {
            
        }

        virtual public void dispose()
        {
            Util.ClearMemory();
            Ctx.m_instance.m_logSys.log(string.Format("~ {0} was destroy!", m_tableName));
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
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

        // 直接从 Lua 脚本添加函数或者变量到表中，执行后不会有任何返回值，不知道为什么
        public object[] DoFile(string fileName)
        {
            //if (m_moduleEnv == null)
            //{
            //    m_moduleEnv = LuaScriptMgr.Instance.lua.NewTable();
            //    // 获取注册表的全局环境 _G 
            //    int oldTop = LuaDLL.lua_gettop(LuaScriptMgr.Instance.lua.L);
            //    int globalIndex = LuaDLL.lua_getmetatable(LuaScriptMgr.Instance.lua.L, LuaIndexes.LUA_REGISTRYINDEX);
            //    if (globalIndex != 0)
            //    {
            //        LuaTable globalTable = LuaScriptMgr.SelfToLuaTable(LuaScriptMgr.Instance.lua.L, globalIndex);
            //        m_moduleEnv.SetMetaTable(globalTable);
            //        LuaDLL.lua_settop(LuaScriptMgr.Instance.lua.L, oldTop);
            //    }
            //}
            //return LuaScriptMgr.Instance.lua.DoFile(fileName, m_moduleEnv);
            //return LuaScriptMgr.Instance.lua.DoFile(fileName);
            return LuaScriptMgr.Instance.lua.DoFile(fileName);
        }

        /**
         * @brief 执行Lua方法
         * @param funcName_ 函数名字
         * @example CallMethod("OnClick");  CallMethod("OnClick", GameObject go_);
         * @example 表中需要这么写 TableName.FunctionName()
         */
        public object[] CallMethod(string funcName_, params object[] args)
        {
            string fullFuncName = "";               // 完全的有表的完全名字
            if (String.IsNullOrEmpty(m_tableName))  // 如果在 _G 表中
            {
                fullFuncName = funcName_;
            }
            else    // 在一个 _G 的一个表中
            {
                fullFuncName = m_tableName + "." + funcName_;
            }
            return LuaScriptMgr.Instance.CallLuaFunction(fullFuncName, args);
        }

        /**
         * @brief 调用类方法
         * @example 表中需要这么写 TableName:FunctionName()， 需要把这个表作为第二个参数传递进入，在 Lua 函数中就直接可以使用 self 了
         */
        public object[] CallClassMethod(string funcName_, params object[] args)
        {
            string fullFuncName = "";               // 完全的有表的完全名字
            if (!String.IsNullOrEmpty(m_tableName))  // 如果在 _G 表中
            {
                fullFuncName = m_tableName + "." + funcName_;
                LuaTable luaTable = LuaScriptMgr.Instance.GetLuaTable(m_tableName);

                return LuaScriptMgr.Instance.CallLuaFunction(fullFuncName, luaTable, args);
            }

            return null;
        }

        /**
         * @brief 获取 Lua 表中的数据
         * @param member_ 表中成员的名字
         */
        public object GetMember(string memberName_)
        {
            string fullMemberName = "";             // 有表前缀的成员的名字
            if (String.IsNullOrEmpty(m_tableName))  // 如果在 _G 表中
            {
                fullMemberName = memberName_;
            }
            else    // 在一个 _G 的一个表中
            {
                fullMemberName = m_tableName + "." + memberName_;
            }

            return LuaScriptMgr.Instance.lua[fullMemberName];
        }

        /**
         * @brief 强制调用 _G 中的函数
         */
        public object[] CallGlobalMethod(string funcName_, params object[] args)
        {
            return LuaScriptMgr.Instance.CallLuaFunction(funcName_, args);
        }

        /**
         * @brief 强制从 _G 中获取数据
         */
        public object GetGlobalMember(string memberName_)
        {
            return LuaScriptMgr.Instance.lua[memberName_];
        }

        // 获取一个表，然后转换成数组
        public string[] getTable2StrArray(string tableName)
        {
            string fullTableName = "";              // 有表前缀的成员名字
            if(String.IsNullOrEmpty(m_tableName))   // 如果在 _G 表中
            {
                fullTableName = tableName;
            }
            else        // 在一个 _G 的一个表中
            {
                fullTableName = m_tableName + "." + tableName;
            }

            LuaTable luaTable = LuaScriptMgr.Instance.GetLuaTable(fullTableName);
            string[] strArray = luaTable.ToArray<string>();
            return strArray;
        }

        // 获取一个表，然后转换成数组
        public int[] getTable2IntArray(string tableName)
        {
            string fullTableName = "";                      // 有表前缀的成员的名字
            if (String.IsNullOrEmpty(m_tableName))          // 如果在 _G 表中 
            {
                fullTableName = tableName;
            }
            else            // 在一个 _G 的一个表中
            {
                fullTableName = m_tableName + "." + tableName;
            }

            LuaTable luaTable = LuaScriptMgr.Instance.GetLuaTable(fullTableName);
            int[] strArray = luaTable.ToArray<int>();
            return strArray;
        }
    }
}