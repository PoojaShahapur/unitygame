using System;
using LuaInterface;

public class SDK_Lib_TestStaticHandleWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("log", log),
			new LuaMethod("New", _CreateSDK_Lib_TestStaticHandle),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.TestStaticHandle", typeof(SDK.Lib.TestStaticHandle), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_TestStaticHandle(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.TestStaticHandle obj = new SDK.Lib.TestStaticHandle();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.TestStaticHandle.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.TestStaticHandle);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int log(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.TestStaticHandle.log(arg0);
		return 0;
	}
}

