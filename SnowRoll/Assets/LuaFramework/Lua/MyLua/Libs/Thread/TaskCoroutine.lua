MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

-- 携程
M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TaskCoroutine";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

-- 通过重写协程函数
function M:run()
    M.super.run(self);
end

return M;