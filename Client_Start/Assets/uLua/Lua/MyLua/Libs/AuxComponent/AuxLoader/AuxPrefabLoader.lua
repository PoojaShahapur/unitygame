require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxPrefabLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mSelfGo = nil;
end

function M:dtor()
    
end

function M:dispose()
    if(self.nativePrefabLoader ~= nil) then
        self.nativePrefabLoader:dispose();
        self.nativePrefabLoader = nil;
    end
	
	self.mSelfGo = nil;
end

function M:setSelfGo(value)
	self.mSelfGo = value;
end

function M:getSelfGo()
	return self.mSelfGo;
end

function M:asyncLoad(path, pThis, handle)
    self.mEvtHandle = GlobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle:addEventHandle(pThis, handle);
    self.nativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New(false);
    self.nativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded);
end

function M:onPrefabLoaded(dispObj)
    self:setSelfGo(self.nativePrefabLoader:getGameObject());
    self.mEvtHandle:dispatchEvent(self);
end

return M;