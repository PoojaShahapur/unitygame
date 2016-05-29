require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

-- CS 中的绑定
local M = GlobalNS.StaticClass();
M.clsName = "CSSystemTest";
GlobalNS[M.clsName] = M;
local this = M;

function M.init()
    this.GlobalEventCmdTest = UnitTest.GlobalEventCmdTest;
end

-- GlobalEventCmd 交互区域
function M.onTestProtoBuf(msg)
    this.GlobalEventCmdTest.onTestProtoBuf(msg);
end

function M.onTestProtoBufBuffer(commandID, buffer)
    this.GlobalEventCmdTest.onTestProtoBufBuffer(commandID, buffer);
end

return M;