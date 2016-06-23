using System;
using LuaInterface;

public class SDK_Lib_MFileSysWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("init", init),
			new LuaMethod("getLocalDataDir", getLocalDataDir),
			new LuaMethod("getLocalReadDir", getLocalReadDir),
			new LuaMethod("getLocalWriteDir", getLocalWriteDir),
			new LuaMethod("getWWWPersistentDataPath", getWWWPersistentDataPath),
			new LuaMethod("getAssetBundlesPersistentDataPath", getAssetBundlesPersistentDataPath),
			new LuaMethod("getDataStreamStreamingAssetsPath", getDataStreamStreamingAssetsPath),
			new LuaMethod("getDataStreamPersistentDataPath", getDataStreamPersistentDataPath),
			new LuaMethod("getWorkPath", getWorkPath),
			new LuaMethod("getDebugWorkPath", getDebugWorkPath),
			new LuaMethod("getAbsPathByRelPath", getAbsPathByRelPath),
			new LuaMethod("readFileAllBytes", readFileAllBytes),
			new LuaMethod("readLuaBufferToFile", readLuaBufferToFile),
			new LuaMethod("New", _CreateSDK_Lib_MFileSys),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("msStreamingAssetsPath", get_msStreamingAssetsPath, set_msStreamingAssetsPath),
			new LuaField("msPersistentDataPath", get_msPersistentDataPath, set_msPersistentDataPath),
			new LuaField("msWWWStreamingAssetsPath", get_msWWWStreamingAssetsPath, set_msWWWStreamingAssetsPath),
			new LuaField("msAssetBundlesStreamingAssetsPath", get_msAssetBundlesStreamingAssetsPath, set_msAssetBundlesStreamingAssetsPath),
			new LuaField("msWWWPersistentDataPath", get_msWWWPersistentDataPath, set_msWWWPersistentDataPath),
			new LuaField("msAssetBundlesPersistentDataPath", get_msAssetBundlesPersistentDataPath, set_msAssetBundlesPersistentDataPath),
			new LuaField("msDataStreamResourcesPath", get_msDataStreamResourcesPath, set_msDataStreamResourcesPath),
			new LuaField("msDataStreamStreamingAssetsPath", get_msDataStreamStreamingAssetsPath, set_msDataStreamStreamingAssetsPath),
			new LuaField("msDataStreamPersistentDataPath", get_msDataStreamPersistentDataPath, set_msDataStreamPersistentDataPath),
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
	static int get_msStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msStreamingAssetsPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msPersistentDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msWWWStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msWWWStreamingAssetsPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msAssetBundlesStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msAssetBundlesStreamingAssetsPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msWWWPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msWWWPersistentDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msAssetBundlesPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msAssetBundlesPersistentDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msDataStreamResourcesPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msDataStreamResourcesPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msDataStreamStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msDataStreamStreamingAssetsPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_msDataStreamPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.MFileSys.msDataStreamPersistentDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msStreamingAssetsPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msStreamingAssetsPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msPersistentDataPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msPersistentDataPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msWWWStreamingAssetsPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msWWWStreamingAssetsPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msAssetBundlesStreamingAssetsPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msAssetBundlesStreamingAssetsPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msWWWPersistentDataPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msWWWPersistentDataPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msAssetBundlesPersistentDataPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msAssetBundlesPersistentDataPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msDataStreamResourcesPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msDataStreamResourcesPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msDataStreamStreamingAssetsPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msDataStreamStreamingAssetsPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_msDataStreamPersistentDataPath(IntPtr L)
	{
		SDK.Lib.MFileSys.msDataStreamPersistentDataPath = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SDK.Lib.MFileSys.init();
		return 0;
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
	static int getWWWPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getWWWPersistentDataPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getAssetBundlesPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getAssetBundlesPersistentDataPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getDataStreamStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getDataStreamStreamingAssetsPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getDataStreamPersistentDataPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.MFileSys.getDataStreamPersistentDataPath();
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
	static int readLuaBufferToFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaStringBuffer o = SDK.Lib.MFileSys.readLuaBufferToFile(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

