-- 所有全局类都在这里加载
MLoader("MyLua.Libs.Core.GlobalNS");      -- 加载自己的全局表

-- 基础
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.StaticClass");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Core.ClassLoader");
MLoader("MyLua.Libs.Core.Malloc");
MLoader("MyLua.Libs.Core.UniqueNumIdGen");
--MLoader("MyLua.Libs.Core.CSImportToLua");

--MLoader("MyLua.Libs.FrameWork.MacroDef");

-- 数据结构
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DataStruct.MDictionary");


-- 延迟处理器
MLoader("MyLua.Libs.DelayHandle.IDelayHandleItem");
MLoader("MyLua.Libs.DelayHandle.DelayHandleObject");
MLoader("MyLua.Libs.DelayHandle.DelayHandleParamBase");
MLoader("MyLua.Libs.DelayHandle.DelayAddParam");
MLoader("MyLua.Libs.DelayHandle.DelayDelParam");
MLoader("MyLua.Libs.DelayHandle.DelayHandleMgrBase");


-- 事件分发器
MLoader("MyLua.Libs.EventHandle.EventDispatchFunctionObject");
MLoader("MyLua.Libs.EventHandle.IDispatchObject");
MLoader("MyLua.Libs.EventHandle.EventDispatch");
MLoader("MyLua.Libs.EventHandle.EventDispatchGroup");
MLoader("MyLua.Libs.EventHandle.AddOnceAndCallOnceEventDispatch");
MLoader("MyLua.Libs.EventHandle.AddOnceEventDispatch");
MLoader("MyLua.Libs.EventHandle.CallOnceEventDispatch");
MLoader("MyLua.Libs.EventHandle.ResEventDispatch");


-- 帧处理事件
MLoader("MyLua.Libs.FrameHandle.ITickedObject");
MLoader("MyLua.Libs.FrameHandle.TimerItemBase");
MLoader("MyLua.Libs.FrameHandle.FrameTimerItem");
MLoader("MyLua.Libs.FrameHandle.DaoJiShiTimer");
MLoader("MyLua.Libs.FrameHandle.SystemTimeData");
MLoader("MyLua.Libs.FrameHandle.SystemFrameData");
MLoader("MyLua.Libs.FrameHandle.TickProcessObject");
MLoader("MyLua.Libs.FrameHandle.TimerMgr");
MLoader("MyLua.Libs.FrameHandle.FrameTimerMgr");
MLoader("MyLua.Libs.FrameHandle.TickMgr");
MLoader("MyLua.Libs.FrameHandle.TimerFunctionObject");
MLoader("MyLua.Libs.FrameHandle.FrameUpdateStatistics");

-- UI
MLoader("MyLua.Libs.UI.UICore.FormMode");
MLoader("MyLua.Libs.UI.UICore.UIFormId");
MLoader("MyLua.Libs.UI.UICore.UILayer");
MLoader("MyLua.Libs.UI.UICore.UICanvas");
MLoader("MyLua.Libs.UI.UICore.Form");
MLoader("MyLua.Libs.UI.UICore.UIAttrSystem");
MLoader("MyLua.Libs.UI.UICore.UIMgr");

-- 组件类型
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.LabelStyleID");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.BtnStyleID");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyleID");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyle");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.LabelStyleBase");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.ButtonStyleBase");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyleMgr");

-- Aux 组件
MLoader("MyLua.Libs.Auxiliary.AuxComponent");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxInputField");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxLabel");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxImage");

MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxPrefabLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxUIPrefabLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxBytesLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxSpriteAtlasLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxTextureLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxTextLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLevelLoader");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxDownloader");

-- FrameWork 脚本
MLoader("MyLua.Libs.FrameWork.ProcessSys");
MLoader("MyLua.Libs.FrameWork.CSSystem");
MLoader("MyLua.Libs.FrameWork.Config");
MLoader("MyLua.Libs.FrameWork.NoDestroyGo");

-- 工具
MLoader("MyLua.Libs.Tools.EEndian");
MLoader("MyLua.Libs.Tools.GkEncode");
MLoader("MyLua.Libs.Tools.UtilStr");
MLoader("MyLua.Libs.Tools.UtilApi");
MLoader("MyLua.Libs.Tools.UtilPath");
MLoader("MyLua.Libs.Tools.UtilMath");

-- 日志
MLoader("MyLua.Libs.Log.LogTypeId");
MLoader("MyLua.Libs.Log.LogSys");

-- 网络
MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle");
MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle_KBE");
MLoader("MyLua.Libs.Network.CmdDisp.NetModuleDispHandle");
MLoader("MyLua.Libs.Network.CmdDisp.NetCmdNotify");
MLoader("MyLua.Libs.Network.CmdDisp.CmdDispInfo");

MLoader("MyLua.Libs.Network.PBFileList");
MLoader("MyLua.Libs.Network.NetCommand");
MLoader("MyLua.Libs.Network.NetMgr");
MLoader("MyLua.Libs.Tools.UtilMsg");

-- 模块系统
MLoader("MyLua.Libs.Module.IGameSys");
MLoader("MyLua.Libs.Module.ILoginSys");

-- 资源系统
MLoader("MyLua.Libs.Resource.ResLoadData.ResPathType");

-- 表系统
MLoader("MyLua.Libs.Table.TableID");

MLoader("MyLua.Libs.Table.TableItemBodyBase");

MLoader("MyLua.Libs.Table.ItemObject.TableCard");
MLoader("MyLua.Libs.Table.ItemObject.TableJob");
MLoader("MyLua.Libs.Table.ItemObject.TableObject");
MLoader("MyLua.Libs.Table.ItemObject.TableRace");
MLoader("MyLua.Libs.Table.ItemObject.TableSkill");
MLoader("MyLua.Libs.Table.ItemObject.TableSpriteAni");
MLoader("MyLua.Libs.Table.ItemObject.TableState");

MLoader("MyLua.Libs.Table.TableItemHeader");
MLoader("MyLua.Libs.Table.TableItemBase");
MLoader("MyLua.Libs.Table.TableBase");

MLoader("MyLua.Libs.Table.UtilTable");
MLoader("MyLua.Libs.Table.TableSys");

