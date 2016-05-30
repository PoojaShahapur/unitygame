using System;
using UnityEngine;
using LuaInterface;

public class SDK_Lib_LuaSystemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("init", init),
			new LuaMethod("getLuaScriptMgr", getLuaScriptMgr),
			new LuaMethod("getLuaClassLoader", getLuaClassLoader),
			new LuaMethod("setNeedUpdate", setNeedUpdate),
			new LuaMethod("CallLuaFunction", CallLuaFunction),
			new LuaMethod("GetLuaTable", GetLuaTable),
			new LuaMethod("GetLuaMember", GetLuaMember),
			new LuaMethod("DoFile", DoFile),
			new LuaMethod("DoString", DoString),
			new LuaMethod("sendFromLua", sendFromLua),
			new LuaMethod("sendFromLuaRpc", sendFromLuaRpc),
			new LuaMethod("receiveToLua", receiveToLua),
			new LuaMethod("onSceneLoaded", onSceneLoaded),
			new LuaMethod("onSocketConnected", onSocketConnected),
			new LuaMethod("loadModule", loadModule),
			new LuaMethod("malloc", malloc),
			new LuaMethod("Advance", Advance),
			new LuaMethod("AddClick", AddClick),
			new LuaMethod("getTable2StrArray", getTable2StrArray),
			new LuaMethod("getTable2IntArray", getTable2IntArray),
			new LuaMethod("IsSystemAttr", IsSystemAttr),
			new LuaMethod("New", _CreateSDK_Lib_LuaSystem),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("lua", get_lua, null),
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.LuaSystem", typeof(SDK.Lib.LuaSystem), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_LuaSystem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.LuaSystem obj = new SDK.Lib.LuaSystem();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.LuaSystem.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.LuaSystem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lua(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lua");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lua on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.lua);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		obj.init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLuaScriptMgr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		LuaScriptMgr o = obj.getLuaScriptMgr();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLuaClassLoader(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		SDK.Lib.LuaCSBridgeClassLoader o = obj.getLuaClassLoader();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setNeedUpdate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.setNeedUpdate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallLuaFunction(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
		object[] o = obj.CallLuaFunction(arg0,objs1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaTable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		LuaInterface.LuaTable o = obj.GetLuaTable(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaMember(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object o = obj.GetLuaMember(arg0);
		LuaScriptMgr.PushVarObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] o = obj.DoFile(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] o = obj.DoString(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int sendFromLua(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		ushort arg0 = (ushort)LuaScriptMgr.GetNumber(L, 2);
		LuaStringBuffer arg1 = LuaScriptMgr.GetStringBuffer(L, 3);
		obj.sendFromLua(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int sendFromLuaRpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		LuaStringBuffer arg0 = LuaScriptMgr.GetStringBuffer(L, 2);
		obj.sendFromLuaRpc(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int receiveToLua(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(SDK.Lib.LuaSystem), typeof(byte[])))
		{
			SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
			byte[] objs0 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
			obj.receiveToLua(objs0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(SDK.Lib.LuaSystem), typeof(SDK.Lib.ByteBuffer)))
		{
			SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
			SDK.Lib.ByteBuffer arg0 = (SDK.Lib.ByteBuffer)LuaScriptMgr.GetLuaObject(L, 2);
			obj.receiveToLua(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.LuaSystem.receiveToLua");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onSceneLoaded(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		obj.onSceneLoaded();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onSocketConnected(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		obj.onSocketConnected();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int loadModule(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		LuaInterface.LuaTable o = obj.loadModule(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int malloc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 2);
		LuaInterface.LuaTable o = obj.malloc(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Advance(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		obj.Advance(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.AddClick(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getTable2StrArray(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		string[] o = obj.getTable2StrArray(arg0,arg1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getTable2IntArray(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		int[] o = obj.getTable2IntArray(arg0,arg1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsSystemAttr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.LuaSystem obj = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.LuaSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool o = obj.IsSystemAttr(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

