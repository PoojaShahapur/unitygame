using System;
using LuaInterface;

public class SDK_Lib_LogSysWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("setEnableLog", setEnableLog),
			new LuaMethod("registerFileLogDevice", registerFileLogDevice),
			new LuaMethod("debugLog_1", debugLog_1),
			new LuaMethod("formatLog", formatLog),
			new LuaMethod("catchLog", catchLog),
			new LuaMethod("fightLog", fightLog),
			new LuaMethod("lua_log", lua_log),
			new LuaMethod("log", log),
			new LuaMethod("lua_warn", lua_warn),
			new LuaMethod("warn", warn),
			new LuaMethod("lua_error", lua_error),
			new LuaMethod("error", error),
			new LuaMethod("logout", logout),
			new LuaMethod("updateLog", updateLog),
			new LuaMethod("closeDevice", closeDevice),
			new LuaMethod("logLoad", logLoad),
			new LuaMethod("New", _CreateSDK_Lib_LogSys),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("m_tmpStr", get_m_tmpStr, set_m_tmpStr),
			new LuaField("m_bOutLog", get_m_bOutLog, set_m_bOutLog),
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.LogSys", typeof(SDK.Lib.LogSys), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_LogSys(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.LogSys obj = new SDK.Lib.LogSys();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.LogSys.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.LogSys);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_tmpStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_tmpStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_tmpStr on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.m_tmpStr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_bOutLog(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_bOutLog");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_bOutLog on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.m_bOutLog);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_tmpStr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_tmpStr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_tmpStr on a nil value");
			}
		}

		obj.m_tmpStr = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_bOutLog(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_bOutLog");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_bOutLog on a nil value");
			}
		}

		obj.m_bOutLog = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setEnableLog(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.setEnableLog(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int registerFileLogDevice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		obj.registerFileLogDevice();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int debugLog_1(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		SDK.Lib.LangItemID arg0 = (SDK.Lib.LangItemID)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.LangItemID));
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		obj.debugLog_1(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int formatLog(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		SDK.Lib.LangTypeId arg0 = (SDK.Lib.LangTypeId)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.LangTypeId));
		SDK.Lib.LangItemID arg1 = (SDK.Lib.LangItemID)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LangItemID));
		string[] objs2 = LuaScriptMgr.GetParamsString(L, 4, count - 3);
		obj.formatLog(arg0,arg1,objs2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int catchLog(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.catchLog(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int fightLog(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.fightLog(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int lua_log(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.lua_log(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int log(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.LogTypeId arg1 = (SDK.Lib.LogTypeId)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LogTypeId));
		obj.log(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int lua_warn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.lua_warn(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int warn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.LogTypeId arg1 = (SDK.Lib.LogTypeId)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LogTypeId));
		obj.warn(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int lua_error(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.lua_error(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int error(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.LogTypeId arg1 = (SDK.Lib.LogTypeId)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LogTypeId));
		obj.error(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int logout(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.LogColor arg1 = (SDK.Lib.LogColor)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LogColor));
		obj.logout(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int updateLog(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		obj.updateLog();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int closeDevice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		obj.closeDevice();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int logLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LogSys obj = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LogSys");
		SDK.Lib.InsResBase arg0 = (SDK.Lib.InsResBase)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.InsResBase));
		obj.logLoad(arg0);
		return 0;
	}
}

