local M = GlobalNS.Class(GlobalNS.IDispatchObject);
M.clsName = "CmdDispInfo";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.bu = nil;
    self.byCmd = 0;
    self.byParam = 0;
end

function M:dtor()

end

return M;