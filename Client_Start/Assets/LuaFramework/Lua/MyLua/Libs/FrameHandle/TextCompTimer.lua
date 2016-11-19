--[[
    @brief 显示在文本组件上的倒计时定时器
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.FrameHandle.TimerItemBase");

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