MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxUnityAtlasLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeUnityAtlasLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeUnityAtlasLoader ~= nil) then
        self.mNativeUnityAtlasLoader:dispose();
        self.mNativeUnityAtlasLoader = nil;
    end
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeUnityAtlasLoader;
end

function M:getSprite(spriteName)
	local sprite = nil;
	
	if(nil ~= self.mNativeUnityAtlasLoader and not GlobalNS.UtilStr.IsNullOrEmpty(spriteName)) then
		sprite = self.mNativeUnityAtlasLoader:getSprite(spriteName);
	end
	
	return sprite;
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.syncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeUnityAtlasLoader) then
			self.mNativeUnityAtlasLoader = GlobalNS.CSSystem.AuxUnityAtlasLoader.New();
		end
		
		if(nil == progressHandle) then
			self.mNativeUnityAtlasLoader:syncLoad(path, self, self.onUnityAtlasLoaded, nil);
		else
			self.mNativeUnityAtlasLoader:syncLoad(path, self, self.onUnityAtlasLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onUnityAtlasLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeUnityAtlasLoader) then
			self.mNativeUnityAtlasLoader = GlobalNS.CSSystem.AuxUnityAtlasLoader.New();
		end
		
		if(nil == progressHandle) then
			self.mNativeUnityAtlasLoader:asyncLoad(path, self, self.onUnityAtlasLoaded, nil);
		else
			self.mNativeUnityAtlasLoader:asyncLoad(path, self, self.onUnityAtlasLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onUnityAtlasLoaded(nil);
	end
end

function M:onUnityAtlasLoaded(dispObj)
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