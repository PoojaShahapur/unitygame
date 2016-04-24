require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "Form";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_guiWin = nil;        -- 自己的 GameObject 根节点
    self.m_id = 0;              -- 自己的 Id
    self.m_bExit = true;        -- 点击返回按钮的时候退出还是隐藏
    self.m_bReady = false;      -- 是否 onReady 函数被调用
    self.m_bVisible = false;
    self.m_data = ...;
end

function M:dtor()

end

-- 加载
function M:load()

end

-- 显示
function M:show()
    GCtx.m_uiMgr:showForm(self.m_id);
end

-- 加载并显示
function M:loadAndShow(data)
    GCtx.m_uiMgr:loadAndShow(self.m_id, data);
end

-- 隐藏
function M:hide()
    GCtx.m_uiMgr:hideForm(self.m_id); 
end

-- 退出
function M:exit()
    GCtx.m_uiMgr:exitForm(self.m_id);
end

-- 界面代码创建后就调用
function M:onInit()
    
end

-- 第一次显示之前会调用一次
function M:onReady()
    self.m_bReady = true;
end

-- 每一次显示都会调用一次
function M:onShow()
    self.m_bVisible = true;
end

-- 每一次隐藏都会调用一次
function M:onHide()
    self.m_bVisible = false;
end

-- 每一次关闭都会调用一次
function M:onExit()
    self.m_bReady = false;
end

function M:isReady()
    return self.m_bReady;
end

function M:isVisible()
    return self.m_bVisible;
end

function M:onCloseBtnClk()
    if(self:isVisible()) then
        if(self.m_bExit) then
            self:exit();
        else
            self:hide();
        end
    end
end

return M;