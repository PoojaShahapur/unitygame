MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.FrameHandle.TickMode");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ProcessSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:advance(delta, tickMode)
    --print("ProcessSys:advance");
	if(GlobalNS.TickMode.eTM_Update == tickMode) then
		--定时器只处理这种模式
		GCtx.mTickMgr:Advance(delta, tickMode);
		GCtx.mTimerMgr:Advance(delta);
	end
end

return M;