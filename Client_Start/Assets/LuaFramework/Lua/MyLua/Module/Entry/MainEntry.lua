--整个程序的入口
--导入加载模块接口
require "MyLua.Libs.Core.ModuleLoad";
--加载宏定义
MLoader("MyLua.Libs.FrameWork.MacroDef");
--启动调试服务器连接
if(MacroDef.UNITY_EDITOR) then
    require("mobdebug").start()
end

local _appSys = require "MyLua.Module.App.AppSys";

_appSys.ctor();
_appSys.init();
_appSys.run();