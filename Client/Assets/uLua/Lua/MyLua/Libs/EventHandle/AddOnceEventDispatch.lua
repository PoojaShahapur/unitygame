--[[
    @brief 事件回调函数只能添加一次
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.EventHandle.EventDispatch"

local M = GlobalNS.Class(GlobalNS.EventDispatch);
M.clsName = "AddOnceEventDispatch";
GlobalNS[M.clsName] = M;

function M:ctor(eventId_)
    
end

function M:addEventHandle(handle)
    -- 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
    if not self:existEventHandle(handle) then
        M.super.addEventHandle(self, handle);
    end
end

return M;