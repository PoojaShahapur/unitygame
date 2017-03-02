using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief 逻辑 Lua 和 CS 之间的交互，是以表为单位进行交互的，一定要注意表名正确，每一个表就是一个功能
     */
    public class LuaCSBridge
    {
        protected string mLuaFile;         // Lua 文件名字
        protected string mTableName;       // 表的名字
        protected string mFuncName;        // 函数名字
        protected LuaTable mLuaTable;       // Lua 中的 Form
        protected LuaFunction mLuaFunc;    // lua 函数
        //protected LuaTable mModuleEnv;   // 执行模块的环境

        /**
         * @brief 表的名字
         */
        public LuaCSBridge(string luaFile, string tableName, string funcName = "")
        {
            mLuaFile = luaFile;
            mTableName = tableName;
            mFuncName = funcName;
        }

        virtual public void dispose()
        {
            if (null != mLuaTable)
            {
                mLuaTable.Dispose();
                mLuaTable = null;
            }

            if (null != mLuaFunc)
            {
                mLuaFunc.Dispose();
                mLuaFunc = null;
            }

            LuaFramework.Util.ClearMemory();

            //Ctx.mInstance.mLogSys.log(string.Format("~ {0} was destroy!", mTableName));
        }

        public void setTable(LuaTable luaTable)
        {
            mLuaTable = luaTable;
        }

        public void setFunction(LuaFunction function)
        {
            mLuaFunc = function;
        }

        public void setFunctor(LuaTable luaTable, LuaFunction function)
        {
            mLuaTable = luaTable;
            mLuaFunc = function;
        }

        public bool isTableEqual(LuaTable luaTable)
        {
            return mLuaTable.Equals(luaTable);
        }

        public bool isFunctionEqual(LuaFunction luaFunction)
        {
            return mLuaFunc.Equals(luaFunction);
        }

        public bool isFunctorEqual(LuaTable luaTable, LuaFunction function)
        {
            return mLuaTable.Equals(luaTable) && mLuaFunc.Equals(function);
        }

        public bool isValid()
        {
            return mLuaTable != null || mLuaFunc != null;
        }

        public void setLuaFile(string luaFile)
        {
            if (!string.IsNullOrEmpty(luaFile))
            {
                if (mLuaFile != luaFile)
                {
                    mLuaFile = luaFile;
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
                if (mFuncName != funcName)
                {
                    mFuncName = funcName;
                    loadFunction();
                }
            }
        }

        public void setTableAndFunctionName(string tableName, string funcName)
        {
            mTableName = tableName;
            mFuncName = funcName;
            loadTableAndFunction();
        }

        virtual public void init()
        {
            loadTableAndFunction();
        }

        public void loadTable()
        {
            if (!string.IsNullOrEmpty(mLuaFile))
            {
                //this.mLuaTable = this.DoFile(mLuaFile)[0] as LuaTable;        // 加载 lua 脚本
                mLuaTable = Ctx.mInstance.mLuaSystem.loadModule(mLuaFile);   // 加载 lua 脚本
            }
            else if (!string.IsNullOrEmpty(mTableName))
            {
                mLuaTable = Ctx.mInstance.mLuaSystem.getLuaTable(mTableName);
            }
        }

        public void loadFunction()
        {
            if (mLuaTable != null && !string.IsNullOrEmpty(mFuncName))
            {
                mLuaFunc = mLuaTable[mFuncName] as LuaFunction;
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

            if(mLuaFunc != null)
            {
                return mLuaFunc.Call(args);
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
                return Ctx.mInstance.mLuaSystem.CallLuaFunction(fullFuncName, mLuaTable, args);
            }
            else
            {
                LuaFunction luaFunc = mLuaTable["call"] as LuaFunction;
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

            if (mLuaFunc != null && mLuaTable != null)
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
                //return mLuaFunc.Call(mLuaTable, args);
                // 如果这样调用，args 将会以 userdata 的形式作为一个元素压入 lua 的堆栈，在 lua 中获取的时候，使用 function handle(params)，要使用类似 CS 数组访问的形式，例如 params[0]，而不是 Lua 中表的访问形式，例如 params[1] 下表从 1 开始
                return mLuaFunc.Call(mLuaTable, (object)args);


                // 如果这样调用，args 也将会数组中的每一个元素压入 lua 堆栈，而不是将其作为 userdata 压入堆栈
                //object[] oneArgs = new object[args.Length + 1];
                //oneArgs[0] = mLuaTable;
                //int idx = 0;
                //while(idx < args.Length)
                //{
                //    oneArgs[idx + 1] = args[idx];
                //    ++idx;
                //}
                //return mLuaFunc.Call(oneArgs);

                // 这样调用，testArgs 会以 t.IsArray 数组形式，直接作为 userdata 压入 lua ，在 lua 中获取的时候，要使用类似 CS 数组访问的形式，例如 testArgs[0]，而不是 Lua 中表的访问形式，例如 testArgs[1] 下表从 1 开始
                //object testArgs = new object[2];
                //(testArgs as object[])[0] = new AuxPrefabLoader();
                //(testArgs as object[])[1] = new AuxPrefabLoader();
                //return mLuaFunc.Call(testArgs);
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
            if (mLuaTable != null)
            {
                return mLuaTable[memberName_];
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