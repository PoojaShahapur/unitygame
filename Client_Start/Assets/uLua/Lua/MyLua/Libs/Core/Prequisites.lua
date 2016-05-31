-- 所有全局类都在这里加载
require "MyLua.Libs.Core.GlobalNS"      -- 加载自己的全局表

-- 基础
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.StaticClass"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Core.ClassLoader"
require "MyLua.Libs.Core.Malloc"
--require "MyLua.Libs.Core.CSImportToLua"

require "MyLua.Libs.FrameWork.MacroDef"

-- 数据结构
require "MyLua.Libs.DataStruct.MList"
require "MyLua.Libs.DataStruct.Dictionary"


-- 延迟处理器
require "MyLua.Libs.DelayHandle.IDelayHandleItem"
require "MyLua.Libs.DelayHandle.DelayHandleObject"
require "MyLua.Libs.DelayHandle.DelayHandleParamBase"
require "MyLua.Libs.DelayHandle.DelayAddParam"
require "MyLua.Libs.DelayHandle.DelayDelParam"
require "MyLua.Libs.DelayHandle.DelayHandleMgrBase"


-- 事件分发器
require "MyLua.Libs.EventHandle.EventDispatchFunctionObject"
require "MyLua.Libs.EventHandle.IDispatchObject"
require "MyLua.Libs.EventHandle.EventDispatch"
require "MyLua.Libs.EventHandle.EventDispatchGroup"
require "MyLua.Libs.EventHandle.AddOnceAndCallOnceEventDispatch"
require "MyLua.Libs.EventHandle.AddOnceEventDispatch"
require "MyLua.Libs.EventHandle.CallOnceEventDispatch"
require "MyLua.Libs.EventHandle.ResEventDispatch"


-- 帧处理事件
require "MyLua.Libs.FrameHandle.ITickedObject"
require "MyLua.Libs.FrameHandle.TimerItemBase"
require "MyLua.Libs.FrameHandle.FrameTimerItem"
require "MyLua.Libs.FrameHandle.DaoJiShiTimer"
require "MyLua.Libs.FrameHandle.SystemTimeData"
require "MyLua.Libs.FrameHandle.SystemFrameData"
require "MyLua.Libs.FrameHandle.TickProcessObject"
require "MyLua.Libs.FrameHandle.TimerMgr"
require "MyLua.Libs.FrameHandle.FrameTimerMgr"
require "MyLua.Libs.FrameHandle.TickMgr"
require "MyLua.Libs.FrameHandle.TimerFunctionObject"

-- UI
require "MyLua.Libs.UI.UICore.UIFormID"
require "MyLua.Libs.UI.UICore.UILayer"
require "MyLua.Libs.UI.UICore.UICanvas"
require "MyLua.Libs.UI.UICore.Form"
require "MyLua.Libs.UI.UICore.UIAttrSystem"
require "MyLua.Libs.UI.UICore.UIMgr"

-- 组件类型
require "MyLua.Libs.UI.UICore.ComponentStyle.LabelStyleID"
require "MyLua.Libs.UI.UICore.ComponentStyle.BtnStyleID"
require "MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyleID"
require "MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyle"
require "MyLua.Libs.UI.UICore.ComponentStyle.LabelStyleBase"
require "MyLua.Libs.UI.UICore.ComponentStyle.ButtonStyleBase"
require "MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyleMgr"

-- Aux 组件
require "MyLua.Libs.AuxComponent.AuxComponent"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxWindow"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxBasicButton"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxInputField"
require "MyLua.Libs.AuxComponent.AuxUIComponent.AuxLabel"
require "MyLua.Libs.AuxComponent.AuxLoader.AuxLoaderBase"
require "MyLua.Libs.AuxComponent.AuxLoader.AuxPrefabLoader"
require "MyLua.Libs.AuxComponent.AuxLoader.AuxUIPrefabLoader"

-- FrameWork 脚本
require "MyLua.Libs.FrameWork.ProcessSys"
require "MyLua.Libs.FrameWork.CSSystem"
require "MyLua.Libs.FrameWork.Config"
require "MyLua.Libs.FrameWork.UniqueIdGentor"
require "MyLua.Libs.FrameWork.NoDestroyGo"

-- 工具
require "MyLua.Libs.Tools.UtilApi"

-- 日志
require "MyLua.Libs.Log.LogTypeId"
require "MyLua.Libs.Log.LogSys"

-- 网络
require "MyLua.Libs.Network.CmdDisp.NetCmdDispHandle"
require "MyLua.Libs.Network.CmdDisp.NetModuleDispHandle"
require "MyLua.Libs.Network.CmdDisp.NetCmdNotify"
require "MyLua.Libs.Network.CmdDisp.CmdDispInfo"

require "MyLua.Libs.Network.PBFileList"
require "MyLua.Libs.Network.NetCommand"
require "MyLua.Libs.Network.NetMgr"
require "MyLua.Libs.Tools.UtilMsg"

-- 模块系统
require "MyLua.Libs.Module.IGameSys"
require "MyLua.Libs.Module.ILoginSys"

