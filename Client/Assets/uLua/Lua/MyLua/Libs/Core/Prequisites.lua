-- 所有全局类都在这里加载
require "MyLua.Libs.Core.GlobalNS"      -- 加载自己的全局表

-- 基础
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Core.StaticClass"
require "MyLua.Libs.Core.ClassLoader"

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