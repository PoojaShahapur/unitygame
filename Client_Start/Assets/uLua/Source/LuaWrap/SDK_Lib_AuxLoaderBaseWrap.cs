using System;
using LuaInterface;

public class SDK_Lib_AuxLoaderBaseWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("dispose", dispose),
			new LuaMethod("hasSuccessLoaded", hasSuccessLoaded),
			new LuaMethod("hasFailed", hasFailed),
			new LuaMethod("needUnload", needUnload),
			new LuaMethod("setPath", setPath),
			new LuaMethod("isInvalid", isInvalid),
			new LuaMethod("getLogicPath", getLogicPath),
			new LuaMethod("syncLoad", syncLoad),
			new LuaMethod("asyncLoad", asyncLoad),
			new LuaMethod("unload", unload),
			new LuaMethod("New", _CreateSDK_Lib_AuxLoaderBase),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.AuxLoaderBase", typeof(SDK.Lib.AuxLoaderBase), regs, fields, typeof(SDK.Lib.GObject));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_AuxLoaderBase(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			SDK.Lib.AuxLoaderBase obj = new SDK.Lib.AuxLoaderBase(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.AuxLoaderBase.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.AuxLoaderBase);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int dispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		obj.dispose();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int hasSuccessLoaded(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		bool o = obj.hasSuccessLoaded();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int hasFailed(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		bool o = obj.hasFailed();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int needUnload(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool o = obj.needUnload(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.setPath(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isInvalid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		bool o = obj.isInvalid();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLogicPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		string o = obj.getLogicPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int syncLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
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
			SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
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
			SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 3);
			LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
			obj.asyncLoad(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.AuxLoaderBase.asyncLoad");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int unload(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.AuxLoaderBase obj = (SDK.Lib.AuxLoaderBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.AuxLoaderBase");
		obj.unload();
		return 0;
	}
}

