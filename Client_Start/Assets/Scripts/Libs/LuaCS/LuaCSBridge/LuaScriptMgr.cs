using LuaInterface;

namespace SDK.Lib
{
    public class LuaScriptMgr
    {
        public static LuaScriptMgr Instance
        {
            get;
            protected set;
        }

        protected LuaState luaState = null;

        protected virtual LuaFileUtils InitLoader()
        {
            return new LuaFileUtils();
        }

        protected virtual void OpenLibs()
        {
            luaState.OpenLibs(LuaDLL.luaopen_pb);
            luaState.OpenLibs(LuaDLL.luaopen_sproto_core);
            luaState.OpenLibs(LuaDLL.luaopen_protobuf_c);
            luaState.OpenLibs(LuaDLL.luaopen_lpeg);
            luaState.OpenLibs(LuaDLL.luaopen_cjson);
            luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
            luaState.OpenLibs(LuaDLL.luaopen_bit);
            luaState.OpenLibs(LuaDLL.luaopen_socket_core);
        }

        protected virtual void Bind()
        {
            LuaBinder.Bind(luaState);
        }

        // 调用初始化函数
        protected void Init()
        {
            InitLoader();
            luaState = new LuaState();
            OpenLibs();
            luaState.LuaSetTop(0);
            Bind();
        }

        public void InitStart()
        {
            this.Init();
            luaState.Start();
        }

        // lua 虚拟机销毁
        protected void Destroy()
        {
            if (luaState != null)
            {
                luaState.Dispose();
                luaState = null;
                Instance = null;
            }
        }

        public LuaState GetMainState()
        {
            return Instance.luaState;
        }

        ///// <summary>
        ///// 初始化Lua代码加载路径
        ///// </summary>
        //void InitLuaPath()
        //{
        //    if (AppConst.DebugMode)
        //    {
        //        string rootPath = AppConst.FrameworkRoot;
        //        lua.AddSearchPath(rootPath + "/Lua");
        //        lua.AddSearchPath(rootPath + "/ToLua/Lua");
        //    }
        //    else {
        //        lua.AddSearchPath(Util.DataPath + "lua");
        //    }
        //}

        ///// <summary>
        ///// 初始化LuaBundle
        ///// </summary>
        //void InitLuaBundle()
        //{
        //    if (loader.beZip)
        //    {
        //        loader.AddBundle("Lua/Lua.unity3d");
        //        loader.AddBundle("Lua/Lua_math.unity3d");
        //        loader.AddBundle("Lua/Lua_system.unity3d");
        //        loader.AddBundle("Lua/Lua_u3d.unity3d");
        //        loader.AddBundle("Lua/Lua_Common.unity3d");
        //        loader.AddBundle("Lua/Lua_Logic.unity3d");
        //        loader.AddBundle("Lua/Lua_View.unity3d");
        //        loader.AddBundle("Lua/Lua_Controller.unity3d");
        //        loader.AddBundle("Lua/Lua_Misc.unity3d");

        //        loader.AddBundle("Lua/Lua_protobuf.unity3d");
        //        loader.AddBundle("Lua/Lua_3rd_cjson.unity3d");
        //        loader.AddBundle("Lua/Lua_3rd_luabitop.unity3d");
        //        loader.AddBundle("Lua/Lua_3rd_pbc.unity3d");
        //        loader.AddBundle("Lua/Lua_3rd_pblua.unity3d");
        //        loader.AddBundle("Lua/Lua_3rd_sproto.unity3d");
        //    }
        //}

        public object[] DoFile(string fileName)
        {
            fileName = Util.convPackagePath2DiscPath(fileName);
            return luaState.DoFile(fileName);
        }

        public LuaTable GetLuaTable(string tableName)
        {
            return luaState.GetTable(tableName);
        }

        public object[] CallLuaFunction(string funcName, params object[] args)
        {
            //LuaFunction func = luaState.GetFunction(name);
            //object[] ret = null;
            //ret = func.Call(args);
            //func.Dispose();
            //return ret;

            LuaFunction func = luaState.GetFunction(funcName);
            if (func != null)
            {
                return func.Call(args);
            }
            return null;
        }

        public void LuaGC(params string[] param)
        {
            luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
        }
    }
}