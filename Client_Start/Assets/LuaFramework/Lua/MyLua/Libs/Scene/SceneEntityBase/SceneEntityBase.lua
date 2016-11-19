MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 场景中的实体，没有什么功能，就是基本循环
]]
 
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SceneEntityBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_render = nil;
    self.m_bClientDispose = false;
end

function M:init()

end

function M:show()
    if (nil ~= self.m_render) then
        self.m_render:show();
    end
end

function M:hide()
    if (nil ~= self.m_render) then
        self.m_render:hide();
    end
end

function M:IsVisible()
    if (nil ~= self.m_render) then
        return self.m_render:IsVisible();
    end

    return true;
end

function M:dispose()
    if(nil ~= self.m_render) then
        self.m_render:dispose();
        self.m_render = nil;
    end
end

function M:setClientDispose()
    self.m_bClientDispose = true;
    if(nil ~= self.m_render) then
        self.m_render.setClientDispose();
    end
end

function M:getClientDispose()
    return self.m_bClientDispose;
end

function M:onTick(delta)

end

function M:gameObject()
    return self.m_render:gameObject();
end

function M:setGameObject(rhv)
    self.m_render.setGameObject(rhv);
end

function M:transform()
    return self.m_render:transform();
end

function M:setPnt(pntGO_)

end

function M:getPnt()
    return self.m_render:getPnt();
end

function M:checkRender()
    return self.m_render:checkRender();
end