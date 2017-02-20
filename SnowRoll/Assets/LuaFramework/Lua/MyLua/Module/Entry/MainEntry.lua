--整个程序的入口
--导入加载模块接口
--自己 lua 代码中的路径都是使用点(.)分割的，并且不要加扩展名字。但是 ToLua 本地加载器使用的是斜杠(/)。加载自己 lua 代码的时候一定要使用 MLoader 去加载，这里面将路径中的点转换成斜杠。但是第一样需要包含 ModuleLoad 模块，因此使用斜杠
require "MyLua/Libs/Core/ModuleLoad";
--加载宏定义
MLoader("MyLua.Libs.FrameWork.MacroDef");
--启动调试服务器连接
if(MacroDef.UNITY_EDITOR) then
    MLoader("mobdebug").start("127.0.0.1");
end

local _appSys = MLoader("MyLua.Module.App.AppSys");

_appSys.ctor();
_appSys.init();
_appSys.run();

--设置 Lua 初始化完成的标志位
GlobalNS.CSSystem.setLuaInited(true);