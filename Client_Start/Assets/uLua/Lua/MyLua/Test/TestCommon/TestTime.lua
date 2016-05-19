require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestTime";
GlobalNS[M.clsName] = M;

function M:run()
    self:test();
end

function M:test()
    local time = 0;
    local str = "";

    time = 366 * 24 * 60 * 60;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 360 * 24 * 60 * 60;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 25 * 60 * 60;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 20 * 60 * 60;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 80 * 60;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 50 * 60;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 80;
    str = GlobalNS.UtilApi.formatTime(time);

    time = 20;
    str = GlobalNS.UtilApi.formatTime(time);
end

return M;