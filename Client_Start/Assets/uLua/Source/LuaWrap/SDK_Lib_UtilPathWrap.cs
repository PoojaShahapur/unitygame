using System;
using UnityEngine;
using LuaInterface;

public class SDK_Lib_UtilPathWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("normalPath", normalPath),
			new LuaMethod("deleteDirectory", deleteDirectory),
			new LuaMethod("existDirectory", existDirectory),
			new LuaMethod("existFile", existFile),
			new LuaMethod("move", move),
			new LuaMethod("deleteFile", deleteFile),
			new LuaMethod("copyFile", copyFile),
			new LuaMethod("createDirectory", createDirectory),
			new LuaMethod("renameFile", renameFile),
			new LuaMethod("combine", combine),
			new LuaMethod("getFileExt", getFileExt),
			new LuaMethod("getFileNameWithExt", getFileNameWithExt),
			new LuaMethod("getFileNameNoExt", getFileNameNoExt),
			new LuaMethod("getFilePathNoName", getFilePathNoName),
			new LuaMethod("getAllFile", getAllFile),
			new LuaMethod("versionPath", versionPath),
			new LuaMethod("delFileNoVer", delFileNoVer),
			new LuaMethod("fileExistNoVer", fileExistNoVer),
			new LuaMethod("saveTex2File", saveTex2File),
			new LuaMethod("saveStr2File", saveStr2File),
			new LuaMethod("saveByte2File", saveByte2File),
			new LuaMethod("copyDirectory", copyDirectory),
			new LuaMethod("traverseDirectory", traverseDirectory),
			new LuaMethod("deleteFiles", deleteFiles),
			new LuaMethod("deleteSubDirsAndFiles", deleteSubDirsAndFiles),
			new LuaMethod("isSubStrInList", isSubStrInList),
			new LuaMethod("isEqualStrInList", isEqualStrInList),
			new LuaMethod("modifyFileNameToCapital", modifyFileNameToCapital),
			new LuaMethod("toLower", toLower),
			new LuaMethod("recureCreateSubDir", recureCreateSubDir),
			new LuaMethod("isAndroidRuntime", isAndroidRuntime),
			new LuaMethod("isStreamingAssetsPath", isStreamingAssetsPath),
			new LuaMethod("getRuntimeWWWStreamingAssetsPath", getRuntimeWWWStreamingAssetsPath),
			new LuaMethod("getCurrentDirectory", getCurrentDirectory),
			new LuaMethod("New", _CreateSDK_Lib_UtilPath),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.UtilPath", typeof(SDK.Lib.UtilPath), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_UtilPath(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.UtilPath obj = new SDK.Lib.UtilPath();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilPath.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.UtilPath);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int normalPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.normalPath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int deleteDirectory(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilPath.deleteDirectory(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int existDirectory(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = SDK.Lib.UtilPath.existDirectory(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int existFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = SDK.Lib.UtilPath.existFile(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int move(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilPath.move(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int deleteFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = SDK.Lib.UtilPath.deleteFile(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int copyFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		SDK.Lib.UtilPath.copyFile(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int createDirectory(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilPath.createDirectory(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int renameFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool o = SDK.Lib.UtilPath.renameFile(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int combine(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		string[] objs0 = LuaScriptMgr.GetParamsString(L, 1, count);
		string o = SDK.Lib.UtilPath.combine(objs0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getFileExt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.getFileExt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getFileNameWithExt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.getFileNameWithExt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getFileNameNoExt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.getFileNameNoExt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getFilePathNoName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.getFilePathNoName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getAllFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.MList<string> arg1 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.MList<string>));
		SDK.Lib.MList<string> arg2 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MList<string>));
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		SDK.Lib.MList<string> o = SDK.Lib.UtilPath.getAllFile(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int versionPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string o = SDK.Lib.UtilPath.versionPath(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int delFileNoVer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.UtilPath.delFileNoVer(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int fileExistNoVer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = SDK.Lib.UtilPath.fileExistNoVer(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int saveTex2File(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilPath.saveTex2File(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int saveStr2File(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		System.Text.Encoding arg2 = (System.Text.Encoding)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Text.Encoding));
		SDK.Lib.UtilPath.saveStr2File(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int saveByte2File(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		byte[] objs1 = LuaScriptMgr.GetArrayNumber<byte>(L, 2);
		SDK.Lib.UtilPath.saveByte2File(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int copyDirectory(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		SDK.Lib.UtilPath.copyDirectory(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int traverseDirectory(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Action<string,string,string> arg2 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action<string,string,string>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<string,string,string>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg2 = (param0, param1, param2) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				LuaScriptMgr.Push(L, param2);
				func.PCall(top, 3);
				func.EndPCall(top);
			};
		}

		Action<string,string,string> arg3 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg3 = (Action<string,string,string>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<string,string,string>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
			arg3 = (param0, param1, param2) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				LuaScriptMgr.Push(L, param2);
				func.PCall(top, 3);
				func.EndPCall(top);
			};
		}

		bool arg4 = LuaScriptMgr.GetBoolean(L, 5);
		bool arg5 = LuaScriptMgr.GetBoolean(L, 6);
		SDK.Lib.UtilPath.traverseDirectory(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int deleteFiles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.MList<string> arg1 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.MList<string>));
		SDK.Lib.MList<string> arg2 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MList<string>));
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		SDK.Lib.UtilPath.deleteFiles(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int deleteSubDirsAndFiles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.MList<string> arg1 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.MList<string>));
		SDK.Lib.MList<string> arg2 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MList<string>));
		SDK.Lib.UtilPath.deleteSubDirsAndFiles(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isSubStrInList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.MList<string> arg1 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.MList<string>));
		bool o = SDK.Lib.UtilPath.isSubStrInList(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isEqualStrInList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		SDK.Lib.MList<string> arg1 = (SDK.Lib.MList<string>)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.MList<string>));
		bool o = SDK.Lib.UtilPath.isEqualStrInList(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int modifyFileNameToCapital(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilPath.modifyFileNameToCapital(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int toLower(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.toLower(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int recureCreateSubDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		SDK.Lib.UtilPath.recureCreateSubDir(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isAndroidRuntime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = SDK.Lib.UtilPath.isAndroidRuntime();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = SDK.Lib.UtilPath.isStreamingAssetsPath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getRuntimeWWWStreamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = SDK.Lib.UtilPath.getRuntimeWWWStreamingAssetsPath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getCurrentDirectory(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.UtilPath.getCurrentDirectory();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

