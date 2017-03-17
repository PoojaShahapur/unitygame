-- 二进制资源加载
MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxBytesLoader";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeBytesLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeBytesLoader ~= nil) then
        self.mNativeBytesLoader:dispose();
        self.mNativeBytesLoader = nil;
    end
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeBytesLoader;
end

function M:getBytes()
	return self.mNativeBytesLoader:getBytes();
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeBytesLoader) then
			self.mNativeBytesLoader = GlobalNS.CSSystem.AuxBytesLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeBytesLoader:syncLoad(path, self, self.onBytesLoaded, nil);
		else
			self.mNativeBytesLoader:syncLoad(path, self, self.onBytesLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onBytesLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeBytesLoader) then
			self.mNativeBytesLoader = GlobalNS.CSSystem.AuxBytesLoader.New("");
		end
		
		if(nil == progressHandle) then
			self.mNativeBytesLoader:asyncLoad(path, self, self.onBytesLoaded, nil);
		else
			self.mNativeBytesLoader:asyncLoad(path, self, self.onBytesLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onBytesLoaded(nil);
	end
end

function M:onBytesLoaded(dispObj)
	self.mNativeBytesLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;