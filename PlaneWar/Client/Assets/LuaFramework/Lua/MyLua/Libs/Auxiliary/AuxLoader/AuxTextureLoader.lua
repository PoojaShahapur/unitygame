-- 纹理加载器
MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxTextureLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeTextureLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeTextureLoader ~= nil) then
        self.mNativeTextureLoader:dispose();
        self.mNativeTextureLoader = nil;
    end
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeTextureLoader;
end

function M:getTexture()
	return self.mNativeTextureLoader:getTexture();
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeTextureLoader) then
			self.mNativeTextureLoader = GlobalNS.CSSystem.AuxTextureLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeTextureLoader:syncLoad(path, self, self.onTextureLoaded, nil);
		else
			self.mNativeTextureLoader:syncLoad(path, self, self.onTextureLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onBytesLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeTextureLoader) then
			self.mNativeTextureLoader = GlobalNS.CSSystem.AuxTextureLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeTextureLoader:asyncLoad(path, self, self.onTextureLoaded, nil);
		else
			self.mNativeTextureLoader:asyncLoad(path, self, self.onTextureLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onTexLoaded(nil);
	end
end

function M:onTextureLoaded(dispObj)
	self.mNativeTextureLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;