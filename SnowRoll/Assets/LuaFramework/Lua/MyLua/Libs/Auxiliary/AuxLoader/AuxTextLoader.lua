-- 文本加载器
MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxTextLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeTextLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeTextLoader ~= nil) then
        self.mNativeTextLoader:dispose();
        self.mNativeTextLoader = nil;
    end
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeTextLoader;
end

function M:getTexture()
	return self.mNativeTextLoader:getTexture();
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeTextLoader) then
			self.mNativeTextLoader = GlobalNS.CSSystem.AuxTextLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeTextLoader:syncLoad(path, self, self.onTextLoaded, nil);
		else
			self.mNativeTextLoader:syncLoad(path, self, self.onTextLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onBytesLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeTextLoader) then
			self.mNativeTextLoader = GlobalNS.CSSystem.AuxTextLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeTextLoader:asyncLoad(path, self, self.onTextLoaded, nil);
		else
			self.mNativeTextLoader:asyncLoad(path, self, self.onTextLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onTexLoaded(nil);
	end
end

function M:onTextLoaded(dispObj)
	self.mNativeTextLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;