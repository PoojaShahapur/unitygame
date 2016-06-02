using System;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;
using Object = UnityEngine.Object;

public class SDK_Lib_UtilApiWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("FindGameObjectsWithTag", FindGameObjectsWithTag),
			new LuaMethod("GoFindChildByName", GoFindChildByName),
			new LuaMethod("TransFindChildByPObjAndPath", TransFindChildByPObjAndPath),
			new LuaMethod("setLblStype", setLblStype),
			new LuaMethod("onBtnClkHandle", onBtnClkHandle),
			new LuaMethod("addEventHandle", addEventHandle),
			new LuaMethod("removeEventHandle", removeEventHandle),
			new LuaMethod("addHoverHandle", addHoverHandle),
			new LuaMethod("addPressHandle", addPressHandle),
			new LuaMethod("addDragOverHandle", addDragOverHandle),
			new LuaMethod("addDragOutHandle", addDragOutHandle),
			new LuaMethod("addEventTriggerHandle", addEventTriggerHandle),
			new LuaMethod("RemoveListener", RemoveListener),
			new LuaMethod("RemoveAllListener", RemoveAllListener),
			new LuaMethod("DestroyComponent", DestroyComponent),
			new LuaMethod("Destroy", Destroy),
			new LuaMethod("DestroyImmediate", DestroyImmediate),
			new LuaMethod("DontDestroyOnLoad", DontDestroyOnLoad),
			new LuaMethod("DestroyTexMat", DestroyTexMat),
			new LuaMethod("CleanTex", CleanTex),
			new LuaMethod("SetActive", SetActive),
			new LuaMethod("fakeSetActive", fakeSetActive),
			new LuaMethod("IsActive", IsActive),
			new LuaMethod("Instantiate", Instantiate),
			new LuaMethod("normalRST", normalRST),
			new LuaMethod("normalPosScale", normalPosScale),
			new LuaMethod("normalPos", normalPos),
			new LuaMethod("normalRot", normalRot),
			new LuaMethod("setRot", setRot),
			new LuaMethod("setScale", setScale),
			new LuaMethod("setPos", setPos),
			new LuaMethod("setRectPos", setRectPos),
			new LuaMethod("setRectRotate", setRectRotate),
			new LuaMethod("setRectSize", setRectSize),
			new LuaMethod("adjustEffectRST", adjustEffectRST),
			new LuaMethod("UnloadUnusedAssets", UnloadUnusedAssets),
			new LuaMethod("ImmeUnloadUnusedAssets", ImmeUnloadUnusedAssets),
			new LuaMethod("UnloadAsset", UnloadAsset),
			new LuaMethod("UnloadAssetBundles", UnloadAssetBundles),
			new LuaMethod("removeFromSceneGraph", removeFromSceneGraph),
			new LuaMethod("SetParent", SetParent),
			new LuaMethod("SetRectTransParent", SetRectTransParent),
			new LuaMethod("copyTransform", copyTransform),
			new LuaMethod("setLayer", setLayer),
			new LuaMethod("setGOName", setGOName),
			new LuaMethod("SetNativeSize", SetNativeSize),
			new LuaMethod("setImageType", setImageType),
			new LuaMethod("Create", Create),
			new LuaMethod("createSpriteGameObject", createSpriteGameObject),
			new LuaMethod("createGameObject", createGameObject),
			new LuaMethod("CreatePrimitive", CreatePrimitive),
			new LuaMethod("AddAnimatorComponent", AddAnimatorComponent),
			new LuaMethod("copyBoxCollider", copyBoxCollider),
			new LuaMethod("IsPointerOverGameObject", IsPointerOverGameObject),
			new LuaMethod("IsPointerOverGameObjectRaycast", IsPointerOverGameObjectRaycast),
			new LuaMethod("trimEndSpace", trimEndSpace),
			new LuaMethod("isAddressEqual", isAddressEqual),
			new LuaMethod("isVectorEqual", isVectorEqual),
			new LuaMethod("getUTCSec", getUTCSec),
			new LuaMethod("getUTCFormatText", getUTCFormatText),
			new LuaMethod("Range", Range),
			new LuaMethod("getDataPath", getDataPath),
			new LuaMethod("convPtFromLocal2World", convPtFromLocal2World),
			new LuaMethod("convPtFromWorld2Local", convPtFromWorld2Local),
			new LuaMethod("convPtFromLocal2Local", convPtFromLocal2Local),
			new LuaMethod("PrefetchSocketPolicy", PrefetchSocketPolicy),
			new LuaMethod("SetDirty", SetDirty),
			new LuaMethod("convPosFromSrcToDestCam", convPosFromSrcToDestCam),
			new LuaMethod("set", set),
			new LuaMethod("getChildCount", getChildCount),
			new LuaMethod("setSiblingIndex", setSiblingIndex),
			new LuaMethod("setSiblingIndexToLastTwo", setSiblingIndexToLastTwo),
			new LuaMethod("setTextStr", setTextStr),
			new LuaMethod("enableBtn", enableBtn),
			new LuaMethod("incEulerAngles", incEulerAngles),
			new LuaMethod("decEulerAngles", decEulerAngles),
			new LuaMethod("GetActive", GetActive),
			new LuaMethod("NameToLayer", NameToLayer),
			new LuaMethod("assert", assert),
			new LuaMethod("rangRandom", rangRandom),
			new LuaMethod("GetRelativePath", GetRelativePath),
			new LuaMethod("getRuntimePlatformFolderForAssetBundles", getRuntimePlatformFolderForAssetBundles),
			new LuaMethod("getManifestName", getManifestName),
			new LuaMethod("createMatIns", createMatIns),
			new LuaMethod("convTIdx2OIdx", convTIdx2OIdx),
			new LuaMethod("setStatic", setStatic),
			new LuaMethod("isStatic", isStatic),
			new LuaMethod("setHideFlags", setHideFlags),
			new LuaMethod("getHideFlags", getHideFlags),
			new LuaMethod("drawCombine", drawCombine),
			new LuaMethod("packIndex", packIndex),
			new LuaMethod("unpackIndex", unpackIndex),
			new LuaMethod("getScreenWidth", getScreenWidth),
			new LuaMethod("getScteedHeight", getScteedHeight),
			new LuaMethod("isWWWNoError", isWWWNoError),
			new LuaMethod("New", _CreateSDK_Lib_UtilApi),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("TRUE", get_TRUE, null),
			new LuaField("FALSE", get_FALSE, null),
			new LuaField("PREFAB_DOT_EXT", get_PREFAB_DOT_EXT, null),
			new LuaField("PREFAB", get_PREFAB, null),
			new LuaField("PNG", get_PNG, null),
			new LuaField("JPG", get_JPG, null),
			new LuaField("TGA", get_TGA, null),
			new LuaField("MAT", get_MAT, null),
			new LuaField("UNITY", get_UNITY, null),
			new LuaField("TXT", get_TXT, null),
			new LuaField("BYTES", get_BYTES, null),
			new LuaField("DOTUNITY3D", get_DOTUNITY3D, null),
			new LuaField("UNITY3D", get_UNITY3D, null),
			new LuaField("DOTPNG", get_DOTPNG, null),
			new LuaField("ASSETBUNDLES", get_ASSETBUNDLES, null),
			new LuaField("CR_LF", get_CR_LF, null),
			new LuaField("SEPARATOR", get_SEPARATOR, null),
			new LuaField("FAKE_POS", get_FAKE_POS, set_FAKE_POS),
		};

		LuaScriptMgr.RegisterLib(L, "SDK.Lib.UtilApi", typeof(SDK.Lib.UtilApi), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDK_Lib_UtilApi(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDK.Lib.UtilApi obj = new SDK.Lib.UtilApi();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.New");
		}

		return 0;
	}

	static Type classType = typeof(SDK.Lib.UtilApi);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TRUE(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.TRUE);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FALSE(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.FALSE);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PREFAB_DOT_EXT(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.PREFAB_DOT_EXT);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PREFAB(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.PREFAB);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PNG(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.PNG);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_JPG(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.JPG);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TGA(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.TGA);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MAT(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.MAT);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UNITY(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.UNITY);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TXT(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.TXT);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BYTES(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.BYTES);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DOTUNITY3D(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.DOTUNITY3D);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UNITY3D(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.UNITY3D);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DOTPNG(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.DOTPNG);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ASSETBUNDLES(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.ASSETBUNDLES);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CR_LF(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.CR_LF);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SEPARATOR(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.SEPARATOR);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FAKE_POS(IntPtr L)
	{
		LuaScriptMgr.Push(L, SDK.Lib.UtilApi.FAKE_POS);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FAKE_POS(IntPtr L)
	{
		SDK.Lib.UtilApi.FAKE_POS = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindGameObjectsWithTag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject[] o = SDK.Lib.UtilApi.FindGameObjectsWithTag(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GoFindChildByName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject o = SDK.Lib.UtilApi.GoFindChildByName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TransFindChildByPObjAndPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = SDK.Lib.UtilApi.TransFindChildByPObjAndPath(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setLblStype(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Text arg0 = (Text)LuaScriptMgr.GetUnityObject(L, 1, typeof(Text));
		SDK.Lib.LabelStyleID arg1 = (SDK.Lib.LabelStyleID)LuaScriptMgr.GetNetObject(L, 2, typeof(SDK.Lib.LabelStyleID));
		SDK.Lib.UtilApi.setLblStype(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onBtnClkHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SDK.Lib.BtnStyleID arg0 = (SDK.Lib.BtnStyleID)LuaScriptMgr.GetNetObject(L, 1, typeof(SDK.Lib.BtnStyleID));
		SDK.Lib.UtilApi.onBtnClkHandle(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addEventHandle(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent), typeof(UnityEngine.Events.UnityAction)))
		{
			UnityEngine.Events.UnityEvent arg0 = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Button), typeof(SDK.Lib.MAction<SDK.Lib.IDispatchObject>)))
		{
			Button arg0 = (Button)LuaScriptMgr.GetLuaObject(L, 1);
			SDK.Lib.MAction<SDK.Lib.IDispatchObject> arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (SDK.Lib.MAction<SDK.Lib.IDispatchObject>)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.PushObject(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<GameObject>), typeof(UnityEngine.Events.UnityAction<GameObject>)))
		{
			UnityEngine.Events.UnityEvent<GameObject> arg0 = (UnityEngine.Events.UnityEvent<GameObject>)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction<GameObject> arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction<GameObject>)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Button.ButtonClickedEvent), typeof(UnityEngine.Events.UnityAction)))
		{
			Button.ButtonClickedEvent arg0 = (Button.ButtonClickedEvent)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Button), typeof(UnityEngine.Events.UnityAction)))
		{
			Button arg0 = (Button)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(UIEventListener.VoidDelegate)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			UIEventListener.VoidDelegate arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UIEventListener.VoidDelegate)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(UnityEngine.Events.UnityAction)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Button.ButtonClickedEvent), typeof(LuaInterface.LuaTable), typeof(LuaInterface.LuaFunction)))
		{
			Button.ButtonClickedEvent arg0 = (Button.ButtonClickedEvent)LuaScriptMgr.GetLuaObject(L, 1);
			LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
			LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
			SDK.Lib.UtilApi.addEventHandle(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent<bool>), typeof(LuaInterface.LuaTable), typeof(LuaInterface.LuaFunction)))
		{
			UnityEngine.Events.UnityEvent<bool> arg0 = (UnityEngine.Events.UnityEvent<bool>)LuaScriptMgr.GetLuaObject(L, 1);
			LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
			LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
			SDK.Lib.UtilApi.addEventHandle(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(string), typeof(UIEventListener.VoidDelegate)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			UIEventListener.VoidDelegate arg2 = null;
			LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

			if (funcType3 != LuaTypes.LUA_TFUNCTION)
			{
				 arg2 = (UIEventListener.VoidDelegate)LuaScriptMgr.GetLuaObject(L, 3);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
				arg2 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(string), typeof(UnityEngine.Events.UnityAction)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			UnityEngine.Events.UnityAction arg2 = null;
			LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

			if (funcType3 != LuaTypes.LUA_TFUNCTION)
			{
				 arg2 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 3);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
				arg2 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.addEventHandle(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 4 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(string), typeof(LuaInterface.LuaTable), typeof(LuaInterface.LuaFunction)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			LuaTable arg2 = LuaScriptMgr.GetLuaTable(L, 3);
			LuaFunction arg3 = LuaScriptMgr.GetLuaFunction(L, 4);
			SDK.Lib.UtilApi.addEventHandle(arg0,arg1,arg2,arg3);
			return 0;
		}
		else if (count == 4 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(LuaInterface.LuaTable), typeof(LuaInterface.LuaFunction), typeof(bool)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
			LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
			bool arg3 = LuaDLL.lua_toboolean(L, 4);
			SDK.Lib.UtilApi.addEventHandle(arg0,arg1,arg2,arg3);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.addEventHandle");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int removeEventHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilApi.removeEventHandle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addHoverHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		UIEventListener.BoolDelegate arg1 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (UIEventListener.BoolDelegate)LuaScriptMgr.GetNetObject(L, 2, typeof(UIEventListener.BoolDelegate));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg1 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				func.PCall(top, 2);
				func.EndPCall(top);
			};
		}

		SDK.Lib.UtilApi.addHoverHandle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addPressHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		UIEventListener.BoolDelegate arg1 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (UIEventListener.BoolDelegate)LuaScriptMgr.GetNetObject(L, 2, typeof(UIEventListener.BoolDelegate));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg1 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				func.PCall(top, 2);
				func.EndPCall(top);
			};
		}

		SDK.Lib.UtilApi.addPressHandle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addDragOverHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		UIEventListener.VoidDelegate arg1 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (UIEventListener.VoidDelegate)LuaScriptMgr.GetNetObject(L, 2, typeof(UIEventListener.VoidDelegate));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		SDK.Lib.UtilApi.addDragOverHandle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addDragOutHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		UIEventListener.VoidDelegate arg1 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (UIEventListener.VoidDelegate)LuaScriptMgr.GetNetObject(L, 2, typeof(UIEventListener.VoidDelegate));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		SDK.Lib.UtilApi.addDragOutHandle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addEventTriggerHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		SDK.Lib.UtilApi.addEventTriggerHandle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveListener(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(UnityEngine.Events.UnityEvent), typeof(UnityEngine.Events.UnityAction)))
		{
			UnityEngine.Events.UnityEvent arg0 = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.RemoveListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Button), typeof(SDK.Lib.MAction<SDK.Lib.IDispatchObject>)))
		{
			Button arg0 = (Button)LuaScriptMgr.GetLuaObject(L, 1);
			SDK.Lib.MAction<SDK.Lib.IDispatchObject> arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (SDK.Lib.MAction<SDK.Lib.IDispatchObject>)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.PushObject(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			SDK.Lib.UtilApi.RemoveListener(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Button), typeof(UnityEngine.Events.UnityAction)))
		{
			Button arg0 = (Button)LuaScriptMgr.GetLuaObject(L, 1);
			UnityEngine.Events.UnityAction arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (UnityEngine.Events.UnityAction)LuaScriptMgr.GetLuaObject(L, 2);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = () =>
				{
					func.Call();
				};
			}

			SDK.Lib.UtilApi.RemoveListener(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.RemoveListener");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAllListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UnityEngine.Events.UnityEvent arg0 = (UnityEngine.Events.UnityEvent)LuaScriptMgr.GetNetObject(L, 1, typeof(UnityEngine.Events.UnityEvent));
		SDK.Lib.UtilApi.RemoveAllListener(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		SDK.Lib.UtilApi.DestroyComponent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Destroy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		SDK.Lib.UtilApi.Destroy(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyImmediate(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
			SDK.Lib.UtilApi.DestroyImmediate(arg0);
			return 0;
		}
		else if (count == 3)
		{
			Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
			bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
			SDK.Lib.UtilApi.DestroyImmediate(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.DestroyImmediate");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DontDestroyOnLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		SDK.Lib.UtilApi.DontDestroyOnLoad(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyTexMat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		SDK.Lib.UtilApi.DestroyTexMat(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CleanTex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		SDK.Lib.UtilApi.CleanTex(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilApi.SetActive(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int fakeSetActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilApi.fakeSetActive(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool o = SDK.Lib.UtilApi.IsActive(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Instantiate(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
			Object o = SDK.Lib.UtilApi.Instantiate(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
			Quaternion arg2 = LuaScriptMgr.GetQuaternion(L, 3);
			Object o = SDK.Lib.UtilApi.Instantiate(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.Instantiate");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int normalRST(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		SDK.Lib.UtilApi.normalRST(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int normalPosScale(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		SDK.Lib.UtilApi.normalPosScale(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int normalPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		SDK.Lib.UtilApi.normalPos(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int normalRot(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		SDK.Lib.UtilApi.normalRot(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setRot(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(LuaTable)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			Quaternion arg1 = LuaScriptMgr.GetQuaternion(L, 2);
			SDK.Lib.UtilApi.setRot(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(LuaTable)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
			SDK.Lib.UtilApi.setRot(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.setRot");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setScale(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		SDK.Lib.UtilApi.setScale(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		SDK.Lib.UtilApi.setPos(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setRectPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RectTransform arg0 = (RectTransform)LuaScriptMgr.GetUnityObject(L, 1, typeof(RectTransform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		SDK.Lib.UtilApi.setRectPos(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setRectRotate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RectTransform arg0 = (RectTransform)LuaScriptMgr.GetUnityObject(L, 1, typeof(RectTransform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		SDK.Lib.UtilApi.setRectRotate(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setRectSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RectTransform arg0 = (RectTransform)LuaScriptMgr.GetUnityObject(L, 1, typeof(RectTransform));
		Vector2 arg1 = LuaScriptMgr.GetVector2(L, 2);
		SDK.Lib.UtilApi.setRectSize(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int adjustEffectRST(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		SDK.Lib.UtilApi.adjustEffectRST(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadUnusedAssets(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		AsyncOperation o = SDK.Lib.UtilApi.UnloadUnusedAssets();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ImmeUnloadUnusedAssets(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SDK.Lib.UtilApi.ImmeUnloadUnusedAssets();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAsset(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		SDK.Lib.UtilApi.UnloadAsset(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAssetBundles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AssetBundle arg0 = (AssetBundle)LuaScriptMgr.GetUnityObject(L, 1, typeof(AssetBundle));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilApi.UnloadAssetBundles(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int removeFromSceneGraph(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilApi.removeFromSceneGraph(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetParent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(GameObject), typeof(bool)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			GameObject arg1 = (GameObject)LuaScriptMgr.GetLuaObject(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			SDK.Lib.UtilApi.SetParent(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(Transform), typeof(bool)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			Transform arg1 = (Transform)LuaScriptMgr.GetLuaObject(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			SDK.Lib.UtilApi.SetParent(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.SetParent");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRectTransParent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		SDK.Lib.UtilApi.SetRectTransParent(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int copyTransform(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		SDK.Lib.UtilApi.copyTransform(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setLayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		SDK.Lib.UtilApi.setLayer(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setGOName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilApi.setGOName(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNativeSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 1, typeof(Image));
		SDK.Lib.UtilApi.SetNativeSize(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setImageType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 1, typeof(Image));
		Image.Type arg1 = (Image.Type)LuaScriptMgr.GetNetObject(L, 2, typeof(Image.Type));
		SDK.Lib.UtilApi.setImageType(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Create(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		Texture2D arg0 = (Texture2D)LuaScriptMgr.GetUnityObject(L, 1, typeof(Texture2D));
		Rect arg1 = (Rect)LuaScriptMgr.GetNetObject(L, 2, typeof(Rect));
		Vector2 arg2 = LuaScriptMgr.GetVector2(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 5);
		SpriteMeshType arg5 = (SpriteMeshType)LuaScriptMgr.GetNetObject(L, 6, typeof(SpriteMeshType));
		Sprite o = SDK.Lib.UtilApi.Create(arg0,arg1,arg2,arg3,arg4,arg5);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int createSpriteGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GameObject o = SDK.Lib.UtilApi.createSpriteGameObject();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int createGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject o = SDK.Lib.UtilApi.createGameObject(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreatePrimitive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		PrimitiveType arg0 = (PrimitiveType)LuaScriptMgr.GetNetObject(L, 1, typeof(PrimitiveType));
		GameObject o = SDK.Lib.UtilApi.CreatePrimitive(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddAnimatorComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilApi.AddAnimatorComponent(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int copyBoxCollider(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		SDK.Lib.UtilApi.copyBoxCollider(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsPointerOverGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = SDK.Lib.UtilApi.IsPointerOverGameObject();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsPointerOverGameObjectRaycast(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = SDK.Lib.UtilApi.IsPointerOverGameObjectRaycast();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int trimEndSpace(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = (string)LuaScriptMgr.GetNetObject(L, 1, typeof(string));
		SDK.Lib.UtilApi.trimEndSpace(ref arg0);
		LuaScriptMgr.Push(L, arg0);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isAddressEqual(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(GameObject)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			GameObject arg1 = (GameObject)LuaScriptMgr.GetLuaObject(L, 2);
			bool o = SDK.Lib.UtilApi.isAddressEqual(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(object), typeof(object)))
		{
			object arg0 = LuaScriptMgr.GetVarObject(L, 1);
			object arg1 = LuaScriptMgr.GetVarObject(L, 2);
			bool o = SDK.Lib.UtilApi.isAddressEqual(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.isAddressEqual");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isVectorEqual(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 1);
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		bool o = SDK.Lib.UtilApi.isVectorEqual(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getUTCSec(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = SDK.Lib.UtilApi.getUTCSec();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getUTCFormatText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.UtilApi.getUTCFormatText();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Range(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int o = SDK.Lib.UtilApi.Range(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getDataPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.UtilApi.getDataPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int convPtFromLocal2World(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		Vector3 o = SDK.Lib.UtilApi.convPtFromLocal2World(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int convPtFromWorld2Local(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		Vector3 o = SDK.Lib.UtilApi.convPtFromWorld2Local(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int convPtFromLocal2Local(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		Vector3 arg2 = LuaScriptMgr.GetVector3(L, 3);
		Vector3 o = SDK.Lib.UtilApi.convPtFromLocal2Local(arg0,arg1,arg2);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PrefetchSocketPolicy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		SDK.Lib.UtilApi.PrefetchSocketPolicy(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetDirty(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		SDK.Lib.UtilApi.SetDirty(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int convPosFromSrcToDestCam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Camera arg0 = (Camera)LuaScriptMgr.GetUnityObject(L, 1, typeof(Camera));
		Camera arg1 = (Camera)LuaScriptMgr.GetUnityObject(L, 2, typeof(Camera));
		Vector3 arg2 = LuaScriptMgr.GetVector3(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		Vector3 o = SDK.Lib.UtilApi.convPosFromSrcToDestCam(arg0,arg1,arg2,arg3);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		SDK.Lib.UtilApi.set(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getChildCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		int o = SDK.Lib.UtilApi.getChildCount(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setSiblingIndex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		SDK.Lib.UtilApi.setSiblingIndex(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setSiblingIndexToLastTwo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		SDK.Lib.UtilApi.setSiblingIndexToLastTwo(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setTextStr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilApi.setTextStr(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int enableBtn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		SDK.Lib.UtilApi.enableBtn(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int incEulerAngles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float o = SDK.Lib.UtilApi.incEulerAngles(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int decEulerAngles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float o = SDK.Lib.UtilApi.decEulerAngles(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool o = SDK.Lib.UtilApi.GetActive(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NameToLayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int o = SDK.Lib.UtilApi.NameToLayer(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int assert(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		SDK.Lib.UtilApi.assert(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int rangRandom(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float o = SDK.Lib.UtilApi.rangRandom(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRelativePath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.UtilApi.GetRelativePath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getRuntimePlatformFolderForAssetBundles(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RuntimePlatform arg0 = (RuntimePlatform)LuaScriptMgr.GetNetObject(L, 1, typeof(RuntimePlatform));
		string o = SDK.Lib.UtilApi.getRuntimePlatformFolderForAssetBundles(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getManifestName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = SDK.Lib.UtilApi.getManifestName();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int createMatIns(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Material arg0 = (Material)LuaScriptMgr.GetUnityObject(L, 1, typeof(Material));
		Material arg1 = (Material)LuaScriptMgr.GetUnityObject(L, 2, typeof(Material));
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		HideFlags arg3 = (HideFlags)LuaScriptMgr.GetNetObject(L, 4, typeof(HideFlags));
		SDK.Lib.UtilApi.createMatIns(ref arg0,arg1,arg2,arg3);
		LuaScriptMgr.Push(L, arg0);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int convTIdx2OIdx(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		short arg0 = (short)LuaScriptMgr.GetNumber(L, 1);
		short arg1 = (short)LuaScriptMgr.GetNumber(L, 2);
		uint o = SDK.Lib.UtilApi.convTIdx2OIdx(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setStatic(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		SDK.Lib.UtilApi.setStatic(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isStatic(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool o = SDK.Lib.UtilApi.isStatic(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setHideFlags(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		HideFlags arg1 = (HideFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(HideFlags));
		SDK.Lib.UtilApi.setHideFlags(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getHideFlags(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Object arg0 = (Object)LuaScriptMgr.GetUnityObject(L, 1, typeof(Object));
		HideFlags o = SDK.Lib.UtilApi.getHideFlags(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int drawCombine(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
			SDK.Lib.UtilApi.drawCombine(arg0);
			return 0;
		}
		else if (count == 2)
		{
			GameObject[] objs0 = LuaScriptMgr.GetArrayObject<GameObject>(L, 1);
			GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
			SDK.Lib.UtilApi.drawCombine(objs0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDK.Lib.UtilApi.drawCombine");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int packIndex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		long arg0 = (long)LuaScriptMgr.GetNumber(L, 1);
		long arg1 = (long)LuaScriptMgr.GetNumber(L, 2);
		uint o = SDK.Lib.UtilApi.packIndex(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int unpackIndex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		long arg1 = (long)LuaScriptMgr.GetNetObject(L, 2, typeof(long));
		long arg2 = (long)LuaScriptMgr.GetNetObject(L, 3, typeof(long));
		SDK.Lib.UtilApi.unpackIndex(arg0,ref arg1,ref arg2);
		LuaScriptMgr.Push(L, arg1);
		LuaScriptMgr.Push(L, arg2);
		return 2;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getScreenWidth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = SDK.Lib.UtilApi.getScreenWidth();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getScteedHeight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = SDK.Lib.UtilApi.getScteedHeight();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int isWWWNoError(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		WWW arg0 = (WWW)LuaScriptMgr.GetNetObject(L, 1, typeof(WWW));
		bool o = SDK.Lib.UtilApi.isWWWNoError(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

