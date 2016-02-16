require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

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
            Ctx.m_luaSystem:setNeedUpdate(true);
        else
            Ctx.m_luaSystem:setNeedUpdate(false);
        end
    end
end

return M;