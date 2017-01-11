MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ProcessSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:advance(delta)
    --print("ProcessSys:advance");
    GCtx.mTimerMgr:Advance(delta);
end

-- 刷新更新标志
function M:refreshUpdateFlag()
    if(GCtx.mConfig:isAllowCallCS()) then
        if(GCtx.mTimerMgr:getCount() > 0) then
            Ctx.mInstance.mLuaSystem:setNeedUpdate(true);
        else
            Ctx.mInstance.mLuaSystem:setNeedUpdate(false);
        end
    end
end

return M;