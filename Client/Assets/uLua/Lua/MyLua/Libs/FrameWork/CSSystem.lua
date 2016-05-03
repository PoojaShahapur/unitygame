require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

-- CS 中的绑定
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CSSystem";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:init()
    self.mCtx = SDK.Lib.Ctx;
    self.mMsgLocalStorage = SDK.Lib.MsgLocalStorage;
end

function M:setNeedUpdate(value)
    
end

return M;