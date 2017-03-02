-- 下载加载器
MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxDownloader";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeDownloadLoader = nil;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativeDownloadLoader ~= nil) then
        self.mNativeDownloadLoader:dispose();
        self.mNativeDownloadLoader = nil;
    end
	
	M.super.dispose(self);
end

function M:getNativeLoader()
	return self.mNativeDownloadLoader;
end

function M:getTexture()
	return self.mNativeDownloadLoader:getTexture();
end

function M:download(origPath, pThis, evtHandle, progressHandle, fileLen, isWriteFile, downloadType)
	M.super.download(self, origPath, pThis, evtHandle, progressHandle, fileLen, isWriteFile, downloadType);
	
	if (self:isInvalid()) then
		if(nil == self.mNativeDownloadLoader) then
			self.mNativeDownloadLoader = GlobalNS.CSSystem.AuxDownloader.New("");
		end
		
		self.mNativeDownloadLoader:download(origPath, pThis, evtHandle, progressHandle, fileLen, isWriteFile, downloadType);
	elseif (self:hasLoadEnd()) then
		self:onTexLoaded(nil);
	end
end

function M:onDownloaded(dispObj)
	self.mNativeDownloadLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;