using System;
using LuaInterface;
using SDK.Lib;

public class SDK_Lib_MFileSysWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("getLocalDataDir", getLocalDataDir),
			new LuaMethod("getLocalReadDir", getLocalReadDir),
			new LuaMethod("getLocalWriteDir", getLocalWriteDir),
			new LuaMethod("getWorkPath", getWorkPath),
			new LuaMethod("getDebugWorkPath", getDebugWorkPath),
			new LuaMethod("getAbsPathByRelPath", getAbsPathByRelPath),
			new LuaMethod("readFileAllBytes", readFileAllBytes),
			new LuaMethod("writeBytesToFile", writeBytesToFile),
			new LuaMethod("readFileAllText", readFileAllText),
			new LuaMethod("writeTextToFile", writeTextToFile),
			new LuaMethod("readLuaBufferToFile", readLuaBufferToFile),
			new LuaMethod("writeLuaBufferToFile", writeLuaBufferToFile),
			new LuaMethod("New", _CreateSDK_Lib_MFileSys),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.MFileSys", typeof(SDK.Lib.MFileSys), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_MFileSys(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.MFileSys obj = new SDK.Lib.MFileSys();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.MFileSys.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.MFileSys);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLocalDataDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getLocalDataDir();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLocalReadDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getLocalReadDir();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getLocalWriteDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getLocalWriteDir();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getWorkPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getWorkPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getDebugWorkPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getDebugWorkPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getAbsPathByRelPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = (string)LuaScriptMgr.GetNetObject(L, 1, typeof(string));
		SDK.Lib.ResLoadType arg1 = (SDK.Lib.ResLoadType)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.ResLoadType));
		string o = SDK.Lib.MFileSys.getAbsPathByRelPath(ref arg0,ref arg1);
		LuaScriptMgr.Push(L, o);
		LuaScriptMgr.Push(L, arg0);
		LuaScriptMgr.Push(L, arg1);
		return 3;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int readFileAllBytes(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		byte[] o = SDK.Lib.MFileSys.readFileAllBytes(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int writeBytesToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		byte[] objs1 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
		SDK.Lib.MFileSys.writeBytesToFile(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int readFileAllText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.MFileSys.readFileAllText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int writeTextToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.MFileSys.writeTextToFile(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int readLuaBufferToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
        LuaStringBuffer o = SDK.Lib.MFileSys.readLuaBufferToFile(arg0);
        LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int writeLuaBufferToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaStringBuffer arg1 = LuaScriptMgr.GetStringBuffer(L, 2);
		SDK.Lib.MFileSys.writeLuaBufferToFile(arg0,arg1);
		return 0;
	}
}

