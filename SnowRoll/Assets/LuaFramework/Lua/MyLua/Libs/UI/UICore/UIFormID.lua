MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UIFormID";
GlobalNS[M.clsName] = M;

function M.ctor()
	
end

function M.init()
    --10000
	this.eUITest = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --10001
	this.eUIStartGame = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --10002 依次类推... ...
    this.eUIRankListPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIRelivePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIPlayerDataPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIForwardForce = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIOptionPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUITopXRankPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIConsoleDlg = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --[[替换占位符(勿删)--]]
	this.eUICount = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
end

--静态表直接构造就行了，不会使用 new 操作符
M.ctor();

return M;