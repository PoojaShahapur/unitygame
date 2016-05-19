local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxPrefabLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()
    
end

function M:dispose()
    if(self.nativePrefabLoader ~= nil) then
        self.nativePrefabLoader:dispose();
        self.nativePrefabLoader = nil;
    end
end

function M:asyncLoad(path, pThis, handle)
    self.mEvtHandle = GLobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle.addEventHandle(pThis, handle);
    self.nativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New(false);
    self.nativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded);
end

function M:onPrefabLoaded(dispObj)
    self:setSelfGo(self.nativePrefabLoader:getGameObject());
    self.mEvtHandle:dispatchEvent(self);
end

return M;