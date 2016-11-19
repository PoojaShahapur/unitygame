--整个程序的入口
--加载宏定义
require "MyLua.Libs.FrameWork.MacroDef"
--启动调试服务器连接
if(MacroDef.UNITY_EDITOR) then
    require("mobdebug").start()
end

local _appSys = require "MyLua.Module.App.AppSys";
_appSys.ctor();
_appSys.init();
_appSys.run();