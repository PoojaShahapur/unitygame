local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxPrefabComponent";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()
    
end

function M:dispose()
    if(self.nativePrefabComponent ~= nil) then
        self.nativePrefabComponent:dispose();
        self.nativePrefabComponent = nil;
    end
end

function M:asyncLoad(path, pThis, handle)
    self.mEvtHandle = GLobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle.addEventHandle(pThis, handle);
    self.nativePrefabComponent = SDK.Lib.AuxPrefabComponent.New(false);
    self.nativePrefabComponent:asyncLoad(path, self, self.onPrefabLoaded);
end

function M:onPrefabLoaded(dispObj)
    self:setSelfGo(self.nativePrefabComponent:getGameObject());
    self.mEvtHandle:dispatchEvent(self);
end

return M;