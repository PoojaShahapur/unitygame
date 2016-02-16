--[[
    @brief 倒计时定时器
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.FrameHandle.TimerItemBase"

local M = GlobalNS.Class(GlobalNS.TimerItemBase);
M.clsName = "TextCompTimer";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_text = nil;
end

function M:preCallBack()
    M.super.preCallBack(self);
    
end

return M;