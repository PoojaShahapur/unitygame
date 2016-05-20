using System;
using LuaInterface;

public class SDK_Lib_CtxWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("instance", instance),
			new LuaMethod("postInit", postInit),
			new LuaMethod("init", init),
			new LuaMethod("setNoDestroyObject", setNoDestroyObject),
			new LuaMethod("unloadAll", unloadAll),
			new LuaMethod("New", _CreateSDK_Lib_Ctx),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("m_instance", get_m_instance, set_m_instance),
			new LuaField("m_netMgr", get_m_netMgr, set_m_netMgr),
			new LuaField("m_cfg", get_m_cfg, set_m_cfg),
			new LuaField("m_logSys", get_m_logSys, set_m_logSys),
			new LuaField("m_resLoadMgr", get_m_resLoadMgr, set_m_resLoadMgr),
			new LuaField("m_inputMgr", get_m_inputMgr, set_m_inputMgr),
			new LuaField("m_gameSys", get_m_gameSys, set_m_gameSys),
			new LuaField("m_sceneSys", get_m_sceneSys, set_m_sceneSys),
			new LuaField("m_tickMgr", get_m_tickMgr, set_m_tickMgr),
			new LuaField("m_processSys", get_m_processSys, set_m_processSys),
			new LuaField("m_timerMgr", get_m_timerMgr, set_m_timerMgr),
			new LuaField("m_frameTimerMgr", get_m_frameTimerMgr, set_m_frameTimerMgr),
			new LuaField("m_uiMgr", get_m_uiMgr, set_m_uiMgr),
			new LuaField("m_resizeMgr", get_m_resizeMgr, set_m_resizeMgr),
			new LuaField("m_cbUIEvent", get_m_cbUIEvent, set_m_cbUIEvent),
			new LuaField("m_coroutineMgr", get_m_coroutineMgr, set_m_coroutineMgr),
			new LuaField("m_engineLoop", get_m_engineLoop, set_m_engineLoop),
			new LuaField("m_gameAttr", get_m_gameAttr, set_m_gameAttr),
			new LuaField("m_fObjectMgr", get_m_fObjectMgr, set_m_fObjectMgr),
			new LuaField("m_npcMgr", get_m_npcMgr, set_m_npcMgr),
			new LuaField("m_playerMgr", get_m_playerMgr, set_m_playerMgr),
			new LuaField("m_monsterMgr", get_m_monsterMgr, set_m_monsterMgr),
			new LuaField("m_spriteAniMgr", get_m_spriteAniMgr, set_m_spriteAniMgr),
			new LuaField("m_shareData", get_m_shareData, set_m_shareData),
			new LuaField("m_layerMgr", get_m_layerMgr, set_m_layerMgr),
			new LuaField("m_sceneEventCB", get_m_sceneEventCB, set_m_sceneEventCB),
			new LuaField("m_camSys", get_m_camSys, set_m_camSys),
			new LuaField("m_sceneLogic", get_m_sceneLogic, set_m_sceneLogic),
			new LuaField("m_sysMsgRoute", get_m_sysMsgRoute, set_m_sysMsgRoute),
			new LuaField("m_netCmdNotify", get_m_netCmdNotify, set_m_netCmdNotify),
			new LuaField("m_msgRouteNotify", get_m_msgRouteNotify, set_m_msgRouteNotify),
			new LuaField("m_moduleSys", get_m_moduleSys, set_m_moduleSys),
			new LuaField("m_tableSys", get_m_tableSys, set_m_tableSys),
			new LuaField("m_fileSys", get_m_fileSys, set_m_fileSys),
			new LuaField("m_factoryBuild", get_m_factoryBuild, set_m_factoryBuild),
			new LuaField("m_langMgr", get_m_langMgr, set_m_langMgr),
			new LuaField("m_dataPlayer", get_m_dataPlayer, set_m_dataPlayer),
			new LuaField("m_xmlCfgMgr", get_m_xmlCfgMgr, set_m_xmlCfgMgr),
			new LuaField("m_matMgr", get_m_matMgr, set_m_matMgr),
			new LuaField("m_modelMgr", get_m_modelMgr, set_m_modelMgr),
			new LuaField("m_texMgr", get_m_texMgr, set_m_texMgr),
			new LuaField("m_skelAniMgr", get_m_skelAniMgr, set_m_skelAniMgr),
			new LuaField("m_skinResMgr", get_m_skinResMgr, set_m_skinResMgr),
			new LuaField("m_prefabMgr", get_m_prefabMgr, set_m_prefabMgr),
			new LuaField("m_controllerMgr", get_m_controllerMgr, set_m_controllerMgr),
			new LuaField("m_bytesResMgr", get_m_bytesResMgr, set_m_bytesResMgr),
			new LuaField("mSpriteMgr", get_mSpriteMgr, set_mSpriteMgr),
			new LuaField("m_systemSetting", get_m_systemSetting, set_m_systemSetting),
			new LuaField("m_coordConv", get_m_coordConv, set_m_coordConv),
			new LuaField("m_pFlyNumMgr", get_m_pFlyNumMgr, set_m_pFlyNumMgr),
			new LuaField("m_pTimerMsgHandle", get_m_pTimerMsgHandle, set_m_pTimerMsgHandle),
			new LuaField("m_poolSys", get_m_poolSys, set_m_poolSys),
			new LuaField("m_loginSys", get_m_loginSys, set_m_loginSys),
			new LuaField("m_wordFilterManager", get_m_wordFilterManager, set_m_wordFilterManager),
			new LuaField("m_versionSys", get_m_versionSys, set_m_versionSys),
			new LuaField("m_pAutoUpdateSys", get_m_pAutoUpdateSys, set_m_pAutoUpdateSys),
			new LuaField("m_TaskQueue", get_m_TaskQueue, set_m_TaskQueue),
			new LuaField("m_TaskThreadPool", get_m_TaskThreadPool, set_m_TaskThreadPool),
			new LuaField("m_pRandName", get_m_pRandName, set_m_pRandName),
			new LuaField("m_pPakSys", get_m_pPakSys, set_m_pPakSys),
			new LuaField("m_gameRunStage", get_m_gameRunStage, set_m_gameRunStage),
			new LuaField("m_soundMgr", get_m_soundMgr, set_m_soundMgr),
			new LuaField("m_mapCfg", get_m_mapCfg, set_m_mapCfg),
			new LuaField("m_autoUpdate", get_m_autoUpdate, set_m_autoUpdate),
			new LuaField("m_atlasMgr", get_m_atlasMgr, set_m_atlasMgr),
			new LuaField("m_auxUIHelp", get_m_auxUIHelp, set_m_auxUIHelp),
			new LuaField("m_widgetStyleMgr", get_m_widgetStyleMgr, set_m_widgetStyleMgr),
			new LuaField("m_sceneEffectMgr", get_m_sceneEffectMgr, set_m_sceneEffectMgr),
			new LuaField("m_systemFrameData", get_m_systemFrameData, set_m_systemFrameData),
			new LuaField("m_systemTimeData", get_m_systemTimeData, set_m_systemTimeData),
			new LuaField("m_scriptDynLoad", get_m_scriptDynLoad, set_m_scriptDynLoad),
			new LuaField("m_scenePlaceHolder", get_m_scenePlaceHolder, set_m_scenePlaceHolder),
			new LuaField("m_luaSystem", get_m_luaSystem, set_m_luaSystem),
			new LuaField("m_movieMgr", get_m_movieMgr, set_m_movieMgr),
			new LuaField("m_nativeInterface", get_m_nativeInterface, set_m_nativeInterface),
			new LuaField("m_gcAutoCollect", get_m_gcAutoCollect, set_m_gcAutoCollect),
			new LuaField("m_memoryCheck", get_m_memoryCheck, set_m_memoryCheck),
			new LuaField("m_depResMgr", get_m_depResMgr, set_m_depResMgr),
			new LuaField("m_terrainGroup", get_m_terrainGroup, set_m_terrainGroup),
			new LuaField("m_textResMgr", get_m_textResMgr, set_m_textResMgr),
			new LuaField("m_sceneManager", get_m_sceneManager, set_m_sceneManager),
			new LuaField("m_terrainBufferSys", get_m_terrainBufferSys, set_m_terrainBufferSys),
			new LuaField("mTerrainGlobalOption", get_mTerrainGlobalOption, set_mTerrainGlobalOption),
			new LuaField("mCoroutineTaskMgr", get_mCoroutineTaskMgr, set_mCoroutineTaskMgr),
			new LuaField("mSceneNodeGraph", get_mSceneNodeGraph, set_mSceneNodeGraph),
			new LuaField("mTerrainEntityMgr", get_mTerrainEntityMgr, set_mTerrainEntityMgr),
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.Ctx", typeof(SDK.Lib.Ctx), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_Ctx(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.Ctx obj = new SDK.Lib.Ctx();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.Ctx.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.Ctx);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, SDK.Lib.Ctx.m_instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_netMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_netMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_netMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_netMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_cfg(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_cfg");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_cfg on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_cfg);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_logSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_logSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_logSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_logSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_resLoadMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_resLoadMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_resLoadMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_resLoadMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_inputMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_inputMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_inputMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_inputMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_gameSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gameSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gameSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_gameSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_sceneSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_sceneSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_tickMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_tickMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_tickMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_tickMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_processSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_processSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_processSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_processSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_timerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_timerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_timerMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_timerMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_frameTimerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_frameTimerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_frameTimerMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_frameTimerMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_uiMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_uiMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_uiMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_uiMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_resizeMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_resizeMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_resizeMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_resizeMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_cbUIEvent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_cbUIEvent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_cbUIEvent on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_cbUIEvent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_coroutineMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_coroutineMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_coroutineMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_coroutineMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_engineLoop(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_engineLoop");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_engineLoop on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_engineLoop);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_gameAttr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gameAttr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gameAttr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_gameAttr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_fObjectMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_fObjectMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_fObjectMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_fObjectMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_npcMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_npcMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_npcMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_npcMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_playerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_playerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_playerMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_playerMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_monsterMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_monsterMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_monsterMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_monsterMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_spriteAniMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_spriteAniMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_spriteAniMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_spriteAniMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_shareData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_shareData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_shareData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_shareData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_layerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_layerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_layerMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_layerMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_sceneEventCB(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneEventCB");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneEventCB on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_sceneEventCB);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_camSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_camSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_camSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_camSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_sceneLogic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneLogic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneLogic on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_sceneLogic);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_sysMsgRoute(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sysMsgRoute");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sysMsgRoute on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_sysMsgRoute);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_netCmdNotify(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_netCmdNotify");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_netCmdNotify on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_netCmdNotify);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_msgRouteNotify(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_msgRouteNotify");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_msgRouteNotify on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_msgRouteNotify);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_moduleSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_moduleSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_moduleSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_moduleSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_tableSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_tableSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_tableSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_tableSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_fileSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_fileSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_fileSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_fileSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_factoryBuild(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_factoryBuild");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_factoryBuild on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_factoryBuild);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_langMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_langMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_langMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_langMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_dataPlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_dataPlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_dataPlayer on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_dataPlayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_xmlCfgMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_xmlCfgMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_xmlCfgMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_xmlCfgMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_matMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_matMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_matMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_matMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_modelMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_modelMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_modelMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_modelMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_texMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_texMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_texMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_texMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_skelAniMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_skelAniMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_skelAniMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_skelAniMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_skinResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_skinResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_skinResMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_skinResMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_prefabMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_prefabMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_prefabMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_prefabMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_controllerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_controllerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_controllerMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_controllerMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_bytesResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_bytesResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_bytesResMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_bytesResMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mSpriteMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mSpriteMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mSpriteMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mSpriteMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_systemSetting(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_systemSetting");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_systemSetting on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_systemSetting);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_coordConv(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_coordConv");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_coordConv on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_coordConv);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_pFlyNumMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pFlyNumMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pFlyNumMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_pFlyNumMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_pTimerMsgHandle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pTimerMsgHandle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pTimerMsgHandle on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_pTimerMsgHandle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_poolSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_poolSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_poolSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_poolSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_loginSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_loginSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_loginSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_loginSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_wordFilterManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_wordFilterManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_wordFilterManager on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_wordFilterManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_versionSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_versionSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_versionSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_versionSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_pAutoUpdateSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pAutoUpdateSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pAutoUpdateSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_pAutoUpdateSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_TaskQueue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_TaskQueue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_TaskQueue on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_TaskQueue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_TaskThreadPool(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_TaskThreadPool");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_TaskThreadPool on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_TaskThreadPool);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_pRandName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pRandName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pRandName on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_pRandName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_pPakSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pPakSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pPakSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_pPakSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_gameRunStage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gameRunStage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gameRunStage on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_gameRunStage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_soundMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_soundMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_soundMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_soundMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_mapCfg(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_mapCfg");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_mapCfg on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_mapCfg);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_autoUpdate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_autoUpdate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_autoUpdate on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_autoUpdate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_atlasMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_atlasMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_atlasMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_atlasMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_auxUIHelp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_auxUIHelp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_auxUIHelp on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_auxUIHelp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_widgetStyleMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_widgetStyleMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_widgetStyleMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_widgetStyleMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_sceneEffectMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneEffectMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneEffectMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_sceneEffectMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_systemFrameData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_systemFrameData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_systemFrameData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_systemFrameData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_systemTimeData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_systemTimeData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_systemTimeData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_systemTimeData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_scriptDynLoad(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_scriptDynLoad");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_scriptDynLoad on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_scriptDynLoad);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_scenePlaceHolder(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_scenePlaceHolder");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_scenePlaceHolder on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_scenePlaceHolder);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_luaSystem(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_luaSystem");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_luaSystem on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_luaSystem);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_movieMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_movieMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_movieMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_movieMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_nativeInterface(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_nativeInterface");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_nativeInterface on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_nativeInterface);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_gcAutoCollect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gcAutoCollect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gcAutoCollect on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_gcAutoCollect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_memoryCheck(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_memoryCheck");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_memoryCheck on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_memoryCheck);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_depResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_depResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_depResMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_depResMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_terrainGroup(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_terrainGroup");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_terrainGroup on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_terrainGroup);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_textResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_textResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_textResMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_textResMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_sceneManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneManager on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_sceneManager);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_terrainBufferSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_terrainBufferSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_terrainBufferSys on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.m_terrainBufferSys);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mTerrainGlobalOption(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mTerrainGlobalOption");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mTerrainGlobalOption on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mTerrainGlobalOption);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mCoroutineTaskMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mCoroutineTaskMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mCoroutineTaskMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mCoroutineTaskMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mSceneNodeGraph(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mSceneNodeGraph");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mSceneNodeGraph on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mSceneNodeGraph);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mTerrainEntityMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mTerrainEntityMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mTerrainEntityMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.mTerrainEntityMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_instance(IntPtr L)
	{
		SDK.Lib.Ctx.m_instance = (SDK.Lib.Ctx)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.Ctx));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_netMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_netMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_netMgr on a nil value");
			}
		}

		obj.m_netMgr = (SDK.Lib.NetworkMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.NetworkMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_cfg(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_cfg");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_cfg on a nil value");
			}
		}

		obj.m_cfg = (SDK.Lib.Config)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.Config));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_logSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_logSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_logSys on a nil value");
			}
		}

		obj.m_logSys = (SDK.Lib.LogSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LogSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_resLoadMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_resLoadMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_resLoadMgr on a nil value");
			}
		}

		obj.m_resLoadMgr = (SDK.Lib.ResLoadMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ResLoadMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_inputMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_inputMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_inputMgr on a nil value");
			}
		}

		obj.m_inputMgr = (SDK.Lib.InputMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.InputMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_gameSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gameSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gameSys on a nil value");
			}
		}

		obj.m_gameSys = (SDK.Lib.IGameSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.IGameSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_sceneSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneSys on a nil value");
			}
		}

		obj.m_sceneSys = (SDK.Lib.SceneSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SceneSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_tickMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_tickMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_tickMgr on a nil value");
			}
		}

		obj.m_tickMgr = (SDK.Lib.TickMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TickMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_processSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_processSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_processSys on a nil value");
			}
		}

		obj.m_processSys = (SDK.Lib.ProcessSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ProcessSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_timerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_timerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_timerMgr on a nil value");
			}
		}

		obj.m_timerMgr = (SDK.Lib.TimerMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TimerMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_frameTimerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_frameTimerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_frameTimerMgr on a nil value");
			}
		}

		obj.m_frameTimerMgr = (SDK.Lib.FrameTimerMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.FrameTimerMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_uiMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_uiMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_uiMgr on a nil value");
			}
		}

		obj.m_uiMgr = (SDK.Lib.UIMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.UIMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_resizeMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_resizeMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_resizeMgr on a nil value");
			}
		}

		obj.m_resizeMgr = (SDK.Lib.ResizeMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ResizeMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_cbUIEvent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_cbUIEvent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_cbUIEvent on a nil value");
			}
		}

		obj.m_cbUIEvent = (SDK.Lib.IUIEvent)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.IUIEvent));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_coroutineMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_coroutineMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_coroutineMgr on a nil value");
			}
		}

		obj.m_coroutineMgr = (SDK.Lib.CoroutineMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.CoroutineMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_engineLoop(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_engineLoop");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_engineLoop on a nil value");
			}
		}

		obj.m_engineLoop = (SDK.Lib.EngineLoop)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.EngineLoop));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_gameAttr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gameAttr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gameAttr on a nil value");
			}
		}

		obj.m_gameAttr = (SDK.Lib.GameAttr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.GameAttr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_fObjectMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_fObjectMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_fObjectMgr on a nil value");
			}
		}

		obj.m_fObjectMgr = (SDK.Lib.FObjectMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.FObjectMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_npcMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_npcMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_npcMgr on a nil value");
			}
		}

		obj.m_npcMgr = (SDK.Lib.NpcMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.NpcMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_playerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_playerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_playerMgr on a nil value");
			}
		}

		obj.m_playerMgr = (SDK.Lib.PlayerMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.PlayerMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_monsterMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_monsterMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_monsterMgr on a nil value");
			}
		}

		obj.m_monsterMgr = (SDK.Lib.MonsterMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MonsterMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_spriteAniMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_spriteAniMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_spriteAniMgr on a nil value");
			}
		}

		obj.m_spriteAniMgr = (SDK.Lib.SpriteAniMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SpriteAniMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_shareData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_shareData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_shareData on a nil value");
			}
		}

		obj.m_shareData = (SDK.Lib.ShareData)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ShareData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_layerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_layerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_layerMgr on a nil value");
			}
		}

		obj.m_layerMgr = (SDK.Lib.LayerMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LayerMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_sceneEventCB(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneEventCB");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneEventCB on a nil value");
			}
		}

		obj.m_sceneEventCB = (SDK.Lib.ISceneEventCB)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ISceneEventCB));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_camSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_camSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_camSys on a nil value");
			}
		}

		obj.m_camSys = (SDK.Lib.CamSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.CamSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_sceneLogic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneLogic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneLogic on a nil value");
			}
		}

		obj.m_sceneLogic = (SDK.Lib.ISceneLogic)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ISceneLogic));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_sysMsgRoute(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sysMsgRoute");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sysMsgRoute on a nil value");
			}
		}

		obj.m_sysMsgRoute = (SDK.Lib.SysMsgRoute)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SysMsgRoute));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_netCmdNotify(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_netCmdNotify");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_netCmdNotify on a nil value");
			}
		}

		obj.m_netCmdNotify = (SDK.Lib.NetCmdNotify)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.NetCmdNotify));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_msgRouteNotify(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_msgRouteNotify");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_msgRouteNotify on a nil value");
			}
		}

		obj.m_msgRouteNotify = (SDK.Lib.MsgRouteNotify)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MsgRouteNotify));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_moduleSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_moduleSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_moduleSys on a nil value");
			}
		}

		obj.m_moduleSys = (SDK.Lib.IModuleSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.IModuleSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_tableSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_tableSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_tableSys on a nil value");
			}
		}

		obj.m_tableSys = (SDK.Lib.TableSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TableSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_fileSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_fileSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_fileSys on a nil value");
			}
		}

		obj.m_fileSys = (SDK.Lib.MFileSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MFileSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_factoryBuild(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_factoryBuild");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_factoryBuild on a nil value");
			}
		}

		obj.m_factoryBuild = (SDK.Lib.FactoryBuild)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.FactoryBuild));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_langMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_langMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_langMgr on a nil value");
			}
		}

		obj.m_langMgr = (SDK.Lib.LangMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LangMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_dataPlayer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_dataPlayer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_dataPlayer on a nil value");
			}
		}

		obj.m_dataPlayer = (SDK.Lib.DataPlayer)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.DataPlayer));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_xmlCfgMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_xmlCfgMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_xmlCfgMgr on a nil value");
			}
		}

		obj.m_xmlCfgMgr = (SDK.Lib.XmlCfgMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.XmlCfgMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_matMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_matMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_matMgr on a nil value");
			}
		}

		obj.m_matMgr = (SDK.Lib.MaterialMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MaterialMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_modelMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_modelMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_modelMgr on a nil value");
			}
		}

		obj.m_modelMgr = (SDK.Lib.ModelMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ModelMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_texMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_texMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_texMgr on a nil value");
			}
		}

		obj.m_texMgr = (SDK.Lib.TextureMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TextureMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_skelAniMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_skelAniMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_skelAniMgr on a nil value");
			}
		}

		obj.m_skelAniMgr = (SDK.Lib.SkelAniMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SkelAniMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_skinResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_skinResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_skinResMgr on a nil value");
			}
		}

		obj.m_skinResMgr = (SDK.Lib.SkinResMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SkinResMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_prefabMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_prefabMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_prefabMgr on a nil value");
			}
		}

		obj.m_prefabMgr = (SDK.Lib.PrefabMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.PrefabMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_controllerMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_controllerMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_controllerMgr on a nil value");
			}
		}

		obj.m_controllerMgr = (SDK.Lib.ControllerMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ControllerMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_bytesResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_bytesResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_bytesResMgr on a nil value");
			}
		}

		obj.m_bytesResMgr = (SDK.Lib.BytesResMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.BytesResMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mSpriteMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mSpriteMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mSpriteMgr on a nil value");
			}
		}

		obj.mSpriteMgr = (SDK.Lib.SpriteMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SpriteMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_systemSetting(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_systemSetting");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_systemSetting on a nil value");
			}
		}

		obj.m_systemSetting = (SDK.Lib.SystemSetting)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SystemSetting));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_coordConv(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_coordConv");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_coordConv on a nil value");
			}
		}

		obj.m_coordConv = (SDK.Lib.CoordConv)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.CoordConv));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_pFlyNumMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pFlyNumMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pFlyNumMgr on a nil value");
			}
		}

		obj.m_pFlyNumMgr = (SDK.Lib.FlyNumMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.FlyNumMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_pTimerMsgHandle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pTimerMsgHandle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pTimerMsgHandle on a nil value");
			}
		}

		obj.m_pTimerMsgHandle = (SDK.Lib.TimerMsgHandle)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TimerMsgHandle));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_poolSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_poolSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_poolSys on a nil value");
			}
		}

		obj.m_poolSys = (SDK.Lib.PoolSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.PoolSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_loginSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_loginSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_loginSys on a nil value");
			}
		}

		obj.m_loginSys = (SDK.Lib.ILoginSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ILoginSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_wordFilterManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_wordFilterManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_wordFilterManager on a nil value");
			}
		}

		obj.m_wordFilterManager = (SDK.Lib.WordFilterManager)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.WordFilterManager));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_versionSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_versionSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_versionSys on a nil value");
			}
		}

		obj.m_versionSys = (SDK.Lib.VersionSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.VersionSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_pAutoUpdateSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pAutoUpdateSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pAutoUpdateSys on a nil value");
			}
		}

		obj.m_pAutoUpdateSys = (SDK.Lib.AutoUpdateSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.AutoUpdateSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_TaskQueue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_TaskQueue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_TaskQueue on a nil value");
			}
		}

		obj.m_TaskQueue = (SDK.Lib.TaskQueue)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TaskQueue));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_TaskThreadPool(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_TaskThreadPool");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_TaskThreadPool on a nil value");
			}
		}

		obj.m_TaskThreadPool = (SDK.Lib.TaskThreadPool)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TaskThreadPool));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_pRandName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pRandName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pRandName on a nil value");
			}
		}

		obj.m_pRandName = (SDK.Lib.RandName)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.RandName));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_pPakSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_pPakSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_pPakSys on a nil value");
			}
		}

		obj.m_pPakSys = (SDK.Lib.PakSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.PakSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_gameRunStage(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gameRunStage");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gameRunStage on a nil value");
			}
		}

		obj.m_gameRunStage = (SDK.Lib.GameRunStage)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.GameRunStage));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_soundMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_soundMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_soundMgr on a nil value");
			}
		}

		obj.m_soundMgr = (SDK.Lib.SoundMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SoundMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_mapCfg(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_mapCfg");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_mapCfg on a nil value");
			}
		}

		obj.m_mapCfg = (SDK.Lib.MapCfg)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MapCfg));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_autoUpdate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_autoUpdate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_autoUpdate on a nil value");
			}
		}

		obj.m_autoUpdate = (SDK.Lib.IAutoUpdate)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.IAutoUpdate));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_atlasMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_atlasMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_atlasMgr on a nil value");
			}
		}

		obj.m_atlasMgr = (SDK.Lib.AtlasMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.AtlasMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_auxUIHelp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_auxUIHelp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_auxUIHelp on a nil value");
			}
		}

		obj.m_auxUIHelp = (SDK.Lib.AuxUIHelp)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.AuxUIHelp));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_widgetStyleMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_widgetStyleMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_widgetStyleMgr on a nil value");
			}
		}

		obj.m_widgetStyleMgr = (SDK.Lib.WidgetStyleMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.WidgetStyleMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_sceneEffectMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneEffectMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneEffectMgr on a nil value");
			}
		}

		obj.m_sceneEffectMgr = (SDK.Lib.SceneEffectMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SceneEffectMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_systemFrameData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_systemFrameData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_systemFrameData on a nil value");
			}
		}

		obj.m_systemFrameData = (SDK.Lib.SystemFrameData)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SystemFrameData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_systemTimeData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_systemTimeData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_systemTimeData on a nil value");
			}
		}

		obj.m_systemTimeData = (SDK.Lib.SystemTimeData)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SystemTimeData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_scriptDynLoad(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_scriptDynLoad");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_scriptDynLoad on a nil value");
			}
		}

		obj.m_scriptDynLoad = (SDK.Lib.ScriptDynLoad)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ScriptDynLoad));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_scenePlaceHolder(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_scenePlaceHolder");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_scenePlaceHolder on a nil value");
			}
		}

		obj.m_scenePlaceHolder = (SDK.Lib.ScenePlaceHolder)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.ScenePlaceHolder));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_luaSystem(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_luaSystem");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_luaSystem on a nil value");
			}
		}

		obj.m_luaSystem = (SDK.Lib.LuaSystem)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.LuaSystem));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_movieMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_movieMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_movieMgr on a nil value");
			}
		}

		obj.m_movieMgr = (SDK.Lib.MovieMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MovieMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_nativeInterface(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_nativeInterface");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_nativeInterface on a nil value");
			}
		}

		obj.m_nativeInterface = (SDK.Lib.NativeInterface)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.NativeInterface));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_gcAutoCollect(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_gcAutoCollect");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_gcAutoCollect on a nil value");
			}
		}

		obj.m_gcAutoCollect = (SDK.Lib.GCAutoCollect)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.GCAutoCollect));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_memoryCheck(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_memoryCheck");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_memoryCheck on a nil value");
			}
		}

		obj.m_memoryCheck = (SDK.Lib.MemoryCheck)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MemoryCheck));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_depResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_depResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_depResMgr on a nil value");
			}
		}

		obj.m_depResMgr = (SDK.Lib.DepResMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.DepResMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_terrainGroup(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_terrainGroup");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_terrainGroup on a nil value");
			}
		}

		obj.m_terrainGroup = (SDK.Lib.MTerrainGroup)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MTerrainGroup));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_textResMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_textResMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_textResMgr on a nil value");
			}
		}

		obj.m_textResMgr = (SDK.Lib.TextResMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TextResMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_sceneManager(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_sceneManager");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_sceneManager on a nil value");
			}
		}

		obj.m_sceneManager = (SDK.Lib.MSceneManager)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.MSceneManager));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_terrainBufferSys(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name m_terrainBufferSys");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index m_terrainBufferSys on a nil value");
			}
		}

		obj.m_terrainBufferSys = (SDK.Lib.TerrainBufferSys)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TerrainBufferSys));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mTerrainGlobalOption(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mTerrainGlobalOption");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mTerrainGlobalOption on a nil value");
			}
		}

		obj.mTerrainGlobalOption = (SDK.Lib.TerrainGlobalOption)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TerrainGlobalOption));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mCoroutineTaskMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mCoroutineTaskMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mCoroutineTaskMgr on a nil value");
			}
		}

		obj.mCoroutineTaskMgr = (SDK.Lib.CoroutineTaskMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.CoroutineTaskMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mSceneNodeGraph(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mSceneNodeGraph");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mSceneNodeGraph on a nil value");
			}
		}

		obj.mSceneNodeGraph = (SDK.Lib.SceneNodeGraph)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.SceneNodeGraph));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mTerrainEntityMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mTerrainEntityMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mTerrainEntityMgr on a nil value");
			}
		}

		obj.mTerrainEntityMgr = (SDK.Lib.TerrainEntityMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(SDK.Lib.TerrainEntityMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int instance(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SDK.Lib.Ctx o = SDK.Lib.Ctx.instance();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int postInit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.Ctx");
		obj.postInit();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.Ctx");
		obj.init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setNoDestroyObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.Ctx");
		obj.setNoDestroyObject();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int unloadAll(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.Ctx obj = (SDK.Lib.Ctx)LuaScriptMgr.GetNetObjectSelf(L, 1, "SDK.Lib.Ctx");
		obj.unloadAll();
		return 0;
	}
}

