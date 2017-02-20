MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIConsoleDlg.ConsoleDlgNS");

--数据区
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ConsoleDlgData";
GlobalNS.ConsoleDlgNS[M.clsName] = M;

function M:ctor()
	self.mId2HandleDic = GlobalNS.new(GlobalNS.MDictionary);
end

function M:init()
    self.mId2HandleDic:Add("loadfile", self.loadfile);
    self.mId2HandleDic:Add("sendshit", self.sendShit);
end

function M:loadfile(params)
    MReload(params);
end

function M:sendShit(params)
    GlobalNS.CSSystem.emitSnowBlock();
end

function M:dtor()
	
end

return M;