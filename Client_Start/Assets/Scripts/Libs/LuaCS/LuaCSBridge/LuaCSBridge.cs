using UnityEngine;
using LuaInterface;
using System;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 逻辑 Lua 和 CS 之间的交互，是以表为单位进行交互的，一定要注意表名正确，每一个表就是一个功能
     */
    public class LuaCSBridge
    {
        protected string m_luaFile;         // Lua 文件名字
        protected string mTableName;       // 表的名字
        protected string m_funcName;        // 函数名字
        protected LuaTable m_luaTable;       // Lua 中的 Form
        protected LuaFunction m_luaFunc;    // lua 函数
        //protected LuaTable m_moduleEnv;   // 执行模块的环境

        /**
         * @brief 表的名字
         */
        public LuaCSBridge(string luaFile, string tableName, string funcName = "")
        {
            m_luaFile = luaFile;
            mTableName = tableName;
            m_funcName = funcName;
        }

        virtual public void dispose()
        {
            if (null != m_luaTable)
            {
                m_luaTable.Dispose();
                m_luaTable = null;
            }

            if (null != m_luaFunc)
            {
                m_luaFunc.Dispose();
                m_luaFunc = null;
            }

            LuaFramework.Util.ClearMemory();

            Ctx.mInstance.mLogSys.log(string.Format("~ {0} was destroy!", mTableName));
        }

        public void setTable(LuaTable luaTable)
        {
            m_luaTable = luaTable;
        }

        public void setFunction(LuaFunction function)
        {
            m_luaFunc = function;
        }

        public void setFunctor(LuaTable luaTable, LuaFunction function)
        {
            m_luaTable = luaTable;
            m_luaFunc = function;
        }

        public bool isTableEqual(LuaTable luaTable)
        {
            return m_luaTable.Equals(luaTable);
        }

        public bool isFunctionEqual(LuaFunction luaFunction)
        {
            return m_luaFunc.Equals(luaFunction);
        }

        public bool isFunctorEqual(LuaTable luaTable, LuaFunction function)
        {
            return m_luaTable.Equals(luaTable) && m_luaFunc.Equals(function);
        }

        public bool isValid()
        {
            return m_luaTable != null || m_luaFunc != null;
        }

        public void setLuaFile(string luaFile)
        {
            if (!string.IsNullOrEmpty(luaFile))
            {
                if (m_luaFile != luaFile)
                {
                    m_luaFile = luaFile;
                    loadTable();
                }
            }
        }

        public void setTableName(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                if (mTableName != tableName)
                {
                    mTableName = tableName;
                    loadTable();
                }
            }
        }

        public void setFunctionName(string funcName)
        {
            if (!string.IsNullOrEmpty(funcName))
            {
                if (m_funcName != funcName)
                {
                    m_funcName = funcName;
                    loadFunction();
                }
            }
        }

        public void setTableAndFunctionName(string tableName, string funcName)
        {
            mTableName = tableName;
            m_funcName = funcName;
            loadTableAndFunction();
        }

        virtual public void init()
        {
            loadTableAndFunction();
        }

        public void loadTable()
        {
            if (!string.IsNullOrEmpty(m_luaFile))
            {
                //this.m_luaTable = this.DoFile(m_luaFile)[0] as LuaTable;        // 加载 lua 脚本
                m_luaTable = Ctx.mInstance.mLuaSystem.loadModule(m_luaFile);   // 加载 lua 脚本
            }
            else if (!string.IsNullOrEmpty(mTableName))
            {
                m_luaTable = Ctx.mInstance.mLuaSystem.getLuaTable(mTableName);
            }
        }

        public void loadFunction()
        {
            if (m_luaTable != null && !string.IsNullOrEmpty(m_funcName))
            {
                m_luaFunc = m_luaTable[m_funcName] as LuaFunction;
            }
        }

        public void loadTableAndFunction()
        {
            loadTable();
            loadFunction();
        }

        /**
         * @brief 执行Lua方法
         * @param funcName_ 函数名字
         * @example CallMethod("OnClick");  CallMethod("OnClick", GameObject go_);
         * @example 表中需要这么写 TableName.FunctionName()
         */
        public object[] callTableMethod(string tableName_, string funcName_, params object[] args)
        {
            /*
            string fullFuncName = "";   // 完全的有表的完全名字
            if (String.IsNullOrEmpty(mTableName))  // 如果在 _G 表中
            {
                fullFuncName = funcName_;
            }
            else    // 在一个 _G 的一个表中
            {
                fullFuncName = mTableName + "." + funcName_;
            }
            return Ctx.mInstance.mLuaSystem.CallLuaFunction(fullFuncName, args);
            */

            setTableName(tableName_);
            setFunctionName(funcName_);

            if(m_luaFunc != null)
            {
                return m_luaFunc.Call(args);
            }

            return null;
        }

        /**
         * @brief 调用类方法
         * @example 表中需要这么写 TableName:FunctionName()， 需要把这个表作为第二个参数传递进入，在 Lua 函数中就直接可以使用 self 了
         */
        virtual public object[] callClassMethod(string tableName_, string funcName_, params object[] args)
        {
            /*
            string fullFuncName = "";               // 完全的有表的完全名字
            if (!String.IsNullOrEmpty(mTableName))  // 如果在 _G 表中
            {
                fullFuncName = mTableName + "." + funcName_;
                return Ctx.mInstance.mLuaSystem.CallLuaFunction(fullFuncName, m_luaTable, args);
            }
            else
            {
                LuaFunction luaFunc = m_luaTable["call"] as LuaFunction;
                if(luaFunc != null)
                {
                    luaFunc.Call(args);
                    luaFunc.Dispose();
                }
            }

            return null;
            */

            setTableName(tableName_);
            setFunctionName(funcName_);

            if (m_luaFunc != null && m_luaTable != null)
            {
                // object[] args
                // (1) 数组类型 args
                // (2) object 类型
                //E:\Self\Self\unity\unitygame\Client_Start\Assets\LuaFramework\ToLua\Core\LuaFunction.cs
                //public void PushArgs(object[] args)
                //{
                //    if (args == null)
                //    {
                //        return;
                //    }

                //    argCount += args.Length; // (1) args.Length (2) 1
                //    luaState.PushArgs(args);
                //}
                //E:\Self\Self\unity\unitygame\Client_Start\Assets\LuaFramework\ToLua\Core\LuaState.cs
                //public void PushArgs(object[] args)
                //{
                //    for (int i = 0; i < args.Length; i++)     // (1) args.Length (2) 1
                //    {
                //        Push(args[i]);
                //    }
                //}
                //E:\Self\Self\unity\unitygame\Client_Start\Assets\LuaFramework\ToLua\Core\ToLua.cs
                //public static void Push(IntPtr L, object obj)
                //{
                //    if (t.IsArray)    // (1) t.IsArray == false (2) t.IsArray == true
                //}


                // 如果这样调用，args 会将每一个元素压入堆栈，在 lua 中访问的时候，需要这样 function handle(...) ，然后获取每一个参数。
                //return m_luaFunc.Call(m_luaTable, args);
                // 如果这样调用，args 将会以 userdata 的形式作为一个元素压入 lua 的堆栈，在 lua 中获取的时候，使用 function handle(params)，要使用类似 CS 数组访问的形式，例如 params[0]，而不是 Lua 中表的访问形式，例如 params[1] 下表从 1 开始
                return m_luaFunc.Call(m_luaTable, (object)args);


                // 如果这样调用，args 也将会数组中的每一个元素压入 lua 堆栈，而不是将其作为 userdata 压入堆栈
                //object[] oneArgs = new object[args.Length + 1];
                //oneArgs[0] = m_luaTable;
                //int idx = 0;
                //while(idx < args.Length)
                //{
                //    oneArgs[idx + 1] = args[idx];
                //    ++idx;
                //}
                //return m_luaFunc.Call(oneArgs);

                // 这样调用，testArgs 会以 t.IsArray 数组形式，直接作为 userdata 压入 lua ，在 lua 中获取的时候，要使用类似 CS 数组访问的形式，例如 testArgs[0]，而不是 Lua 中表的访问形式，例如 testArgs[1] 下表从 1 开始
                //object testArgs = new object[2];
                //(testArgs as object[])[0] = new AuxPrefabLoader();
                //(testArgs as object[])[1] = new AuxPrefabLoader();
                //return m_luaFunc.Call(testArgs);
            }

            return null;
        }

        /**
         * @brief 获取 Lua 表中的数据
         * @param member_ 表中成员的名字
         */
        public object getMember(string memberName_)
        {
            /*
            string fullMemberName = "";             // 有表前缀的成员的名字
            if (String.IsNullOrEmpty(mTableName))  // 如果在 _G 表中
            {
                fullMemberName = memberName_;
            }
            else    // 在一个 _G 的一个表中
            {
                fullMemberName = mTableName + "." + memberName_;
            }

            return Ctx.mInstance.mLuaSystem.lua[fullMemberName];
            */
            if (m_luaTable != null)
            {
                return m_luaTable[memberName_];
            }

            return null;
        }

        /**
         * @brief 强制调用 _G 中的函数
         */
        public object[] callGlobalMethod(string funcName_, params object[] args)
        {
            return Ctx.mInstance.mLuaSystem.callLuaFunction(funcName_, args);
        }

        /**
         * @brief 强制从 _G 中获取数据
         */
        public object getGlobalMember(string memberName_)
        {
            return Ctx.mInstance.mLuaSystem.getLuaMember(memberName_);
        }
    }
}