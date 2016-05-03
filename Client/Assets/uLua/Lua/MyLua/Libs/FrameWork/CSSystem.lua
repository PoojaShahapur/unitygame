require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

-- CS 中的绑定
local M = {};
M.clsName = "CSSystem";
GlobalNS[M.clsName] = M;
local this = M;

function M.init()
    this.Ctx = SDK.Lib.Ctx;
    this.MsgLocalStorage = SDK.Lib.MsgLocalStorage;
end

function M.setNeedUpdate(value)
    
end

return M;