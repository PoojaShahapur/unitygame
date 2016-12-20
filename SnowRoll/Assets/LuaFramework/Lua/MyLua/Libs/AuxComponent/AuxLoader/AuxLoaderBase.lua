MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxLoaderBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()
    
end

function M:dispose()
	
end

function M:hasSuccessLoaded()
	return self:getNativeLoader():hasSuccessLoaded();
end

function M:hasFailed()
	return self:getNativeLoader():hasFailed();
end

function M:getLogicPath()
	return self:getNativeLoader():getLogicPath();
end

function M:getNativeLoader()
	return nil;
end

return M;