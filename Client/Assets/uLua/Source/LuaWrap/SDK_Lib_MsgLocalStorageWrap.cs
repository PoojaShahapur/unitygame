using System;
using LuaInterface;

public class SDK_Lib_MsgLocalStorageWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("readFileAllBytes", readFileAllBytes),
			new LuaMethod("writeBytesToFile", writeBytesToFile),
			new LuaMethod("readFileAllText", readFileAllText),
			new LuaMethod("writeTextToFile", writeTextToFile),
			new LuaMethod("readLuaBufferToFile", readLuaBufferToFile),
			new LuaMethod("writeLuaBufferToFile", writeLuaBufferToFile),
			new LuaMethod("New", _CreateSDK_Lib_MsgLocalStorage),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.MsgLocalStorage", typeof(SDK.Lib.MsgLocalStorage), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_MsgLocalStorage(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.MsgLocalStorage obj = new SDK.Lib.MsgLocalStorage();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.MsgLocalStorage.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.MsgLocalStorage);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int readFileAllBytes(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		byte[] o = SDK.Lib.MsgLocalStorage.readFileAllBytes(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int writeBytesToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		byte[] objs1 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
		SDK.Lib.MsgLocalStorage.writeBytesToFile(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int readFileAllText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.MsgLocalStorage.readFileAllText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int writeTextToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.MsgLocalStorage.writeTextToFile(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int readLuaBufferToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaStringBuffer o = SDK.Lib.MsgLocalStorage.readLuaBufferToFile(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int writeLuaBufferToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaStringBuffer arg1 = LuaScriptMgr.GetStringBuffer(L, 2);
		SDK.Lib.MsgLocalStorage.writeLuaBufferToFile(arg0,arg1);
		return 0;
	}
}

