require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxPrefabLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mSelfGo = nil;
	self.mNativePrefabLoader = nil;
end

function M:dtor()
    
end

function M:dispose()
    if(self.mNativePrefabLoader ~= nil) then
        self.mNativePrefabLoader:dispose();
        self.mNativePrefabLoader = nil;
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
    self.mNativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New(false);
    self.mNativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded);
end

function M:onPrefabLoaded(dispObj)
	self.mNativePrefabLoader = dispObj;
	local typeId = self.mNativePrefabLoader:getTypeId();
	
    self:setSelfGo(self.mNativePrefabLoader:getGameObject());
    self.mEvtHandle:dispatchEvent(self);
end

return M;