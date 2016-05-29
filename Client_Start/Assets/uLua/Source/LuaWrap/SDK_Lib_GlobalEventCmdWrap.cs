using System;
using LuaInterface;

public class SDK_Lib_GlobalEventCmdWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("onSample", onSample),
			new LuaMethod("New", _CreateSDK_Lib_GlobalEventCmd),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.GlobalEventCmd", typeof(SDK.Lib.GlobalEventCmd), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_GlobalEventCmd(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.GlobalEventCmd obj = new SDK.Lib.GlobalEventCmd();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.GlobalEventCmd.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.GlobalEventCmd);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onSample(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SDK.Lib.GlobalEventCmd.onSample();
		return 0;
	}
}

