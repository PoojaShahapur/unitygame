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
    GCtx.m_timerMgr:Advance(delta);
end

-- 刷新更新标志
function M:refreshUpdateFlag()
    if(GCtx.m_cofig:isAllowCallCS()) then
        if(GCtx.m_timerMgr:getCount() > 0) then
            Ctx.mInstance.mLuaSystem:setNeedUpdate(true);
        else
            Ctx.mInstance.mLuaSystem:setNeedUpdate(false);
        end
    end
end

return M;