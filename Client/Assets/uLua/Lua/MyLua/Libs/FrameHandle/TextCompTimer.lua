--[[
    @brief 显示在文本组件上的倒计时定时器
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.FrameHandle.TimerItemBase"

local M = GlobalNS.Class(GlobalNS.DaoJiShiTimer);
M.clsName = "TextCompTimer";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_text = nil;
end

function M:onPreCallBack()
    M.super.onPreCallBack(self);
    
end

return M;