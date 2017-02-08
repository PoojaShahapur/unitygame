MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxSpriteAtlasLoader";
GlobalNS[M.clsName] = M;

function M:ctor(path)
	self.mNativeSpriteAtlasLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeSpriteAtlasLoader ~= nil) then
        self.mNativeSpriteAtlasLoader:dispose();
        self.mNativeSpriteAtlasLoader = nil;
    end
end

function M:syncLoad(path, pThis, handle)
	
end

function M:asyncLoad(path, pThis, handle)
    self.mEvtHandle = GlobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle:addEventHandle(pThis, handle);
    self.mNativeSpriteAtlasLoader = GlobalNS.CSSystem.AuxSpriteAtlasLoader.New("");
    self.mNativeSpriteAtlasLoader:asyncLoad(path, self, self.onSpriteAtlasLoaded);
end

function M:onSpriteAtlasLoaded(dispObj)
	self.mNativePrefabLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;