MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.AuxComponent.AuxLoader.AuxPrefabLoader");

local M = GlobalNS.Class(GlobalNS.AuxPrefabLoader);
M.clsName = "AuxUIPrefabLoader";
GlobalNS[M.clsName] = M;

function M:ctor(path, isNeedInsPrefab, isInsNeedCoroutine)
    self.mFormId = 0;
end

function M:dtor()
    
end

function M:setFormId(value)
	self.mFormId = value;
end

function M:getFormId()
	return self.mFormId;
end

return M;