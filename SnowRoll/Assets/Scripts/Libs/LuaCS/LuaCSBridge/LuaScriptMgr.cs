﻿using LuaInterface;
using System;

namespace SDK.Lib
{
    public class LuaScriptMgr
    {
        public static LuaScriptMgr Instance;

        private LuaState lua;
        private MyLuaLoader loader;
        private LuaLooper loop = null;

        public static LuaScriptMgr getSinglePtr()
        {
            if (null == LuaScriptMgr.Instance)
            {
                Instance = new LuaScriptMgr();
            }

            return Instance;
        }

        public LuaScriptMgr()
        {
            
        }

        public void init()
        {
            loader = new MyLuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);

            InitStart();
        }

        public void dispose()
        {

        }

        public void InitStart()
        {
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
        }

        public void OpenZbsDebugger(string ip = "localhost")
        {
            if (!System.IO.Directory.Exists(LuaConst.zbsDir))
            {
                Debugger.LogWarning("ZeroBraneStudio not install or LuaConst.zbsDir not right");
                return;
            }

            if (!LuaConst.openLuaSocket)
            {
                OpenLuaSocket();
            }

            if (!string.IsNullOrEmpty(LuaConst.zbsDir))
            {
                lua.AddSearchPath(LuaConst.zbsDir);
            }

            // 设置全局表的 ip 字段
            lua.LuaDoString(string.Format("DebugServerIp = '{0}'", ip));
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;

            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.RegFunction("mime.core", LuaOpen_Mime_Core);
            lua.EndPreLoad();
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs()
        {
            //lua.OpenLibs(LuaDLL.luaopen_pb);
            //lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);    // C 版本 lua protobuffer
            //lua.OpenLibs(LuaDLL.luaopen_lpeg);
            //lua.OpenLibs(LuaDLL.luaopen_bit);
            //lua.OpenLibs(LuaDLL.luaopen_socket_core);

            //this.OpenCJson();

            if (LuaConst.openLuaSocket)
            {
                OpenLuaSocket();
            }

            // 不用调用这个接口，mobdebug.lua 已经拷贝到 Assets\LuaFramework\Lua 目录下了
            //if (LuaConst.openZbsDebugger)
            //{
            //    OpenZbsDebugger();
            //}
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath()
        {
            if (LuaFramework.AppConst.DebugMode)
            {
                string rootPath = LuaFramework.AppConst.FrameworkRoot;
                lua.AddSearchPath(rootPath + "/Lua");
                lua.AddSearchPath(rootPath + "/ToLua/Lua");
            }
            else
            {
                lua.AddSearchPath(LuaFramework.Util.DataPath + "lua");
            }
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle()
        {
            if (loader.beZip)
            {
                loader.AddBundle("lua/lua.unity3d");
                //loader.AddBundle("lua/lua_math.unity3d"); // 新版本 math 已经没有了
                loader.AddBundle("lua/lua_system.unity3d");
                loader.AddBundle("lua/lua_system_reflection.unity3d");
                loader.AddBundle("lua/lua_unityengine.unity3d");

                //loader.AddBundle("lua/lua_common.unity3d");
                //loader.AddBundle("lua/lua_logic.unity3d");
                //loader.AddBundle("lua/lua_view.unity3d");
                //loader.AddBundle("lua/lua_controller.unity3d");
                loader.AddBundle("lua/lua_misc.unity3d");

                //loader.AddBundle("lua/lua_protobuf.unity3d");
                //loader.AddBundle("lua/lua_3rd_cjson.unity3d");
                //loader.AddBundle("lua/lua_3rd_luabitop.unity3d");
                loader.AddBundle("lua/lua_3rd_pbc.unity3d");
                //loader.AddBundle("lua/lua_3rd_pblua.unity3d");
                //loader.AddBundle("lua/lua_3rd_sproto.unity3d");

                //--------------------------MyLua Start --------------------------------
                loader.AddBundle("lua/lua_mylua_libs_auxcomponent.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_auxcomponent_auxloader.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_auxcomponent_auxuicomponent.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_common.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_core.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_datastruct.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_delayhandle.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_eventhandle.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_framehandle.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_framework.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_functor.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_log.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_module.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_network.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_network_cmddisp.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_protobuf.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_resource_resloaddata.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_scene_being.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_scene_sceneentitybase.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_table.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_table_itemobject.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_thread.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_tools.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_ui_uicore.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_ui_uicore_componentstyle.unity3d");
                loader.AddBundle("lua/lua_mylua_module_app.unity3d");
                loader.AddBundle("lua/lua_mylua_module_entry.unity3d");
                loader.AddBundle("lua/lua_mylua_module_game.unity3d");

                loader.AddBundle("lua/lua_mylua_module_game_eventcb.unity3d");
                loader.AddBundle("lua/lua_mylua_module_game_gamenethandle.unity3d");
                loader.AddBundle("lua/lua_mylua_module_login.unity3d");
                loader.AddBundle("lua/lua_mylua_module_login_eventcb.unity3d");

                loader.AddBundle("lua/lua_mylua_module_login_loginnethandle.unity3d");
                loader.AddBundle("lua/lua_mylua_test.unity3d");
                loader.AddBundle("lua/lua_mylua_test_testcmddisp.unity3d");
                loader.AddBundle("lua/lua_mylua_test_testprotobuf.unity3d");

                loader.AddBundle("lua/lua_mylua_test_teststr.unity3d");
                loader.AddBundle("lua/lua_mylua_test_testtable.unity3d");
                loader.AddBundle("lua/lua_mylua_test_testui.unity3d");

                loader.AddBundle("lua/lua_mylua_ui_uisettingspanel.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uioptionpanel.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uiplayerdatapanel.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uiranklistpanel.unity3d");

                loader.AddBundle("lua/lua_mylua_ui_uirelivepanel.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uistartgame.unity3d");
                //loader.AddBundle("lua/lua_mylua_ui_uitest.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uitopxrankpanel.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uishop_skinpanel.unity3d");

                loader.AddBundle("lua/lua_mylua_ui_uipack.unity3d");
                loader.AddBundle("lua/lua_mylua_ui_uiconsoledlg.unity3d");

                loader.AddBundle("lua/lua_mylua_libs_gamedata.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_uisignpanel.unity3d");
                loader.AddBundle("lua/lua_mylua_libs_uimessagepanel.unity3d");
                //--------------------------MyLua End --------------------------------
            }
        }

        public object[] DoFile(string filename)
        {
            filename = UtilApi.convPackagePath2DiscPath(filename);
            return lua.DoFile(filename);
        }

        public LuaTable GetLuaTable(string tableName)
        {
            return lua.GetTable(tableName);
        }

        public object[] CallLuaFunction(string funcName, params object[] args)
        {
            //LuaFunction func = luaState.GetFunction(name);
            //object[] ret = null;
            //ret = func.Call(args);
            //func.Dispose();
            //return ret;

            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                return func.Call(args);
            }
            return null;
        }

        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close()
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            loader = null;
        }

        public LuaState GetMainState()
        {
            return lua;
        }
    }
}