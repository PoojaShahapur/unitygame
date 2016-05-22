using System;
using LuaInterface;

public class SDK_Lib_GObjectWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("getTypeId", getTypeId),
			new LuaMethod("New", _CreateSDK_Lib_GObject),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.GObject", typeof(SDK.Lib.GObject), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_GObject(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.GObject obj = new SDK.Lib.GObject();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.GObject.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.GObject);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getTypeId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.GObject obj = (SDK.Lib.GObject)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.GObject");
		string o = obj.getTypeId();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

