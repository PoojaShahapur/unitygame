using System;
using LuaInterface;

public class SDK_Lib_LuaToCSWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateSDK_Lib_LuaToCS),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.LuaToCS", typeof(SDK.Lib.LuaToCS), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_LuaToCS(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.LuaToCS obj = new SDK.Lib.LuaToCS();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.LuaToCS.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.LuaToCS);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}
}

