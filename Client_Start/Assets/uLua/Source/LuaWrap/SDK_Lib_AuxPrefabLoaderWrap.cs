using System;
using UnityEngine;
using LuaInterface;

public class SDK_Lib_AuxPrefabLoaderWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("dispose", dispose),
			new LuaMethod("isDestroySelf", isDestroySelf),
			new LuaMethod("setDestroySelf", setDestroySelf),
			new LuaMethod("getLogicPath", getLogicPath),
			new LuaMethod("syncLoad", syncLoad),
			new LuaMethod("asyncLoad", asyncLoad),
			new LuaMethod("onPrefabLoaded", onPrefabLoaded),
			new LuaMethod("onPrefabIns", onPrefabIns),
			new LuaMethod("onAllFinish", onAllFinish),
			new LuaMethod("unload", unload),
			new LuaMethod("getGameObject", getGameObject),
			new LuaMethod("setClientDispose", setClientDispose),
			new LuaMethod("getClientDispose", getClientDispose),
			new LuaMethod("New", _CreateSDK_Lib_AuxPrefabLoader),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("selfGo", get_selfGo, set_selfGo),
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.AuxPrefabLoader", typeof(SDK.Lib.AuxPrefabLoader), regs, fields, typeof(SDK.Lib.AuxLoaderBase));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_AuxPrefabLoader(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
			SDK.Lib.AuxPrefabLoader obj = new SDK.Lib.AuxPrefabLoader(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.AuxPrefabLoader.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.AuxPrefabLoader);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selfGo(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selfGo");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selfGo on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.selfGo);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selfGo(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name selfGo");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index selfGo on a nil value");
			}
		}

		obj.selfGo = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int dispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		obj.dispose();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isDestroySelf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		bool o = obj.isDestroySelf();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setDestroySelf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.setDestroySelf(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLogicPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		string o = obj.getLogicPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int syncLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.MAction<SDK.Lib.IDispatchObject> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (SDK.Lib.MAction<SDK.Lib.IDispatchObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MAction<SDK.Lib.IDispatchObject>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushObject(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.syncLoad(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int asyncLoad(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			SDK.Lib.MAction<SDK.Lib.IDispatchObject> arg1 = null;
			LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

			if (funcType3 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (SDK.Lib.MAction<SDK.Lib.IDispatchObject>)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MAction<SDK.Lib.IDispatchObject>));
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
				arg1 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.PushObject(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			obj.asyncLoad(arg0,arg1);
			return 0;
		}
		else if (count == 4)
		{
			SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 3);
			LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
			obj.asyncLoad(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.AuxPrefabLoader.asyncLoad");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onPrefabLoaded(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		SDK.Lib.IDispatchObject arg0 = (SDK.Lib.IDispatchObject)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.IDispatchObject));
		obj.onPrefabLoaded(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onPrefabIns(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		SDK.Lib.IDispatchObject arg0 = (SDK.Lib.IDispatchObject)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.IDispatchObject));
		obj.onPrefabIns(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onAllFinish(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		obj.onAllFinish();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int unload(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		obj.unload();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		GameObject o = obj.getGameObject();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setClientDispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		obj.setClientDispose();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getClientDispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxPrefabLoader obj = (SDK.Lib.AuxPrefabLoader)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxPrefabLoader");
		bool o = obj.getClientDispose();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

