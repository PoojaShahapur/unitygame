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
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeSpriteAtlasLoader;
end

function M:getSprite(spriteName)
	local sprite = nil;
	
	if(nil ~= self.mNativeSpriteAtlasLoader) then
		sprite = self.mNativeSpriteAtlasLoader:getSprite(spriteName);
	end
	
	return sprite;
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.syncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeSpriteAtlasLoader) then
			self.mNativeSpriteAtlasLoader = GlobalNS.CSSystem.AuxSpriteAtlasLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeSpriteAtlasLoader:syncLoad(path, self, self.onSpriteAtlasLoaded, nil);
		else
			self.mNativeSpriteAtlasLoader:syncLoad(path, self, self.onSpriteAtlasLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onSpriteAtlasLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeSpriteAtlasLoader) then
			self.mNativeSpriteAtlasLoader = GlobalNS.CSSystem.AuxSpriteAtlasLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeSpriteAtlasLoader:asyncLoad(path, self, self.onSpriteAtlasLoaded, nil);
		else
			self.mNativeSpriteAtlasLoader:asyncLoad(path, self, self.onSpriteAtlasLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		--self:onSpriteAtlasLoaded(self.mNativePrefabLoader);
		self:onSpriteAtlasLoaded(nil);
	end
end

function M:onSpriteAtlasLoaded(dispObj)
	if(nil ~= dispObj) then
		self.mNativePrefabLoader = dispObj[0];
		
		if(nil ~= self.mEvtHandle) then
			self.mEvtHandle:dispatchEvent(self);
		end
	else
		if(nil ~= self.mEvtHandle) then
			self.mEvtHandle:dispatchEvent(self);
		end
	end
end

return M;