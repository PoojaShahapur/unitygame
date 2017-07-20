-- 关卡加载器
MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxLevelLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeTextureLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeLevelLoader ~= nil) then
        self.mNativeLevelLoader:dispose();
        self.mNativeLevelLoader = nil;
    end
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeLevelLoader;
end

function M:getTexture()
	return self.mNativeLevelLoader:getTexture();
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeLevelLoader) then
			self.mNativeLevelLoader = GlobalNS.CSSystem.AuxLevelLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeLevelLoader:syncLoad(path, self, self.onLevelLoaded, nil);
		else
			self.mNativeLevelLoader:syncLoad(path, self, self.onLevelLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onBytesLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeLevelLoader) then
			self.mNativeLevelLoader = GlobalNS.CSSystem.AuxLevelLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeLevelLoader:asyncLoad(path, self, self.onLevelLoaded, nil);
		else
			self.mNativeLevelLoader:asyncLoad(path, self, self.onLevelLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onTexLoaded(nil);
	end
end

function M:onLevelLoaded(dispObj)
	self.mNativeLevelLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;