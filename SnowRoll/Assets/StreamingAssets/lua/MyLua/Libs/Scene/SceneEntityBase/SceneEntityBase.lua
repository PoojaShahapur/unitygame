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
    self.mRender = nil;
    self.mIsClientDispose = false;
end

function M:init()

end

function M:show()
    if (nil ~= self.mRender) then
        self.mRender:show();
    end
end

function M:hide()
    if (nil ~= self.mRender) then
        self.mRender:hide();
    end
end

function M:IsVisible()
    if (nil ~= self.mRender) then
        return self.mRender:IsVisible();
    end

    return true;
end

function M:dispose()
    if(nil ~= self.mRender) then
        self.mRender:dispose();
        self.mRender = nil;
    end
end

function M:setClientDispose()
    self.mIsClientDispose = true;
    if(nil ~= self.mRender) then
        self.mRender.setClientDispose();
    end
end

function M:getClientDispose()
    return self.mIsClientDispose;
end

function M:onTick(delta)

end

function M:gameObject()
    return self.mRender:gameObject();
end

function M:setGameObject(rhv)
    self.mRender.setGameObject(rhv);
end

function M:transform()
    return self.mRender:transform();
end

function M:setPnt(pntGO_)

end

function M:getPnt()
    return self.mRender:getPnt();
end

function M:checkRender()
    return self.mRender:checkRender();
end