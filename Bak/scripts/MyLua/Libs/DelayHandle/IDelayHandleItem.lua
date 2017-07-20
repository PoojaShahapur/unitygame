MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "IDelayHandleItem";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

-- 需要实现的接口
function M:setClientDispose(isDispose)
	
end

function M:isClientDispose()

end

return M;