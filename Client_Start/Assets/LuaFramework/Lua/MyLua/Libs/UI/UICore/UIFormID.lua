MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UIFormID";
GlobalNS[M.clsName] = M;

function M.ctor()
	
end

function M.init()
	this.eUITest = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
	this.eUIStartGame = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
	this.eUICount = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
end

--静态表直接构造就行了，不会使用 new 操作符
M.ctor();

return M;