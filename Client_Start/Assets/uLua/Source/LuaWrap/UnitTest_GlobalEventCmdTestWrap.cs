using System;
using LuaInterface;

public class UnitTest_GlobalEventCmdTestWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("onTestProtoBuf", onTestProtoBuf),
			new LuaMethod("onTestProtoBufBuffer", onTestProtoBufBuffer),
			new LuaMethod("New", _CreateUnitTest_GlobalEventCmdTest),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UnitTest.GlobalEventCmdTest", typeof(UnitTest.GlobalEventCmdTest), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnitTest_GlobalEventCmdTest(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UnitTest.GlobalEventCmdTest obj = new UnitTest.GlobalEventCmdTest();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UnitTest.GlobalEventCmdTest.New");
		}

		return 0;
	}

	static Type classType = typeof(UnitTest.GlobalEventCmdTest);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onTestProtoBuf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		UnitTest.GlobalEventCmdTest.onTestProtoBuf(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onTestProtoBufBuffer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		ushort arg0 = (ushort)LuaScriptMgr.GetNumber(L, 1);
		LuaStringBuffer arg1 = LuaScriptMgr.GetStringBuffer(L, 2);
		UnitTest.GlobalEventCmdTest.onTestProtoBufBuffer(arg0,arg1);
		return 0;
	}
}

