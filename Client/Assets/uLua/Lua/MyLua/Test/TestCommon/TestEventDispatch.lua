require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Libs.EventHandle.EventDispatch"
require "MyLua.Libs.EventHandle.EventDispatchGroup"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestEventDispatch";
GlobalNS[M.clsName] = M;

function M:run()
    --self:testEventDispatch();
    self:testEventDispatchGroup();
end

function M:testEventDispatch()
    local disp = GlobalNS.new(GlobalNS.EventDispatch);
    disp:addEventHandle(self, self.onHandle);
    disp:dispatchEvent(nil);
end

function M:testEventDispatchGroup()
    local dispGroup = GlobalNS.new(GlobalNS.EventDispatchGroup);
    dispGroup:addEventHandle(2, self, self.onHandle);
    dispGroup:dispatchEvent(2, nil);
end

function M:onHandle()
    
end

return M;