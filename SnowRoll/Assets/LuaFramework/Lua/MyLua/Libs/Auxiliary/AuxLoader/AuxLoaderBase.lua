MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxLoaderBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self:reset();
end

function M:dtor()
    
end

function M:dispose()
	self:unload();
end

function M:reset()
	self.mPrePath = "";
	self.mPath = "";
	self.mIsInvalid = true;
	self.mEvtHandle = nil;
	self.mProgressEventDispatch = nil;
end

function M:unload()
	if (self.mEvtHandle ~= nil) then
		self.mEvtHandle:clearEventHandle();
		self.mEvtHandle = nil;
	end

	if (self.mProgressEventDispatch ~= nil) then
		self.mProgressEventDispatch:clearEventHandle();
		self.mProgressEventDispatch = nil;
	end

	self:reset();
end

function M:hasSuccessLoaded()
	return self:getNativeLoader():hasSuccessLoaded();
end

function M:hasFailed()
	return self:getNativeLoader():hasFailed();
end

function M:hasLoadEnd()
	return self:getNativeLoader():hasLoadEnd();
end

function M:getLogicPath()
	return self:getNativeLoader():getLogicPath();
end

function M:getNativeLoader()
	return nil;
end

-- 是否需要卸载资源
function M:needUnload(path)
	return self.mPath ~= path and not GlobalNS.UtilStr.IsNullOrEmpty(path) and not GlobalNS.UtilStr.IsNullOrEmpty(self.mPath);
end

function M:setPath(path)
	self.mPrePath = self.mPath;
	self.mPath = path;

	if(self.mPrePath ~= self.mPath and not GlobalNS.UtilStr.IsNullOrEmpty(self.mPath)) then
		self.mIsInvalid = true;
	else
		self.mIsInvalid = false;
	end
end

function M:updatePath(path)
	if (self:needUnload(path)) then
		self:unload();
	end

	self:setPath(path);
end

function M:isInvalid()
	return self.mIsInvalid;
end

-- 添加事件处理器
function M:addEventHandle(pThis, evtHandle, progressHandle)
	if (nil ~= evtHandle) then
		if (nil == self.mEvtHandle) then
			self.mEvtHandle = GlobalNS.new(GlobalNS.ResEventDispatch);
		end

		self.mEvtHandle:addEventHandle(pThis, evtHandle);
	end

	if (nil ~= progressHandle) then
		if (nil == self.mProgressEventDispatch) then
			self.mProgressEventDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
		end

		self.mProgressEventDispatch:addEventHandle(pThis, progressHandle);
	end
end

function M:syncLoad(path, pThis, evtHandle, progressHandle)
	self:updatePath(path);
	self:addEventHandle(pThis, evtHandle, progressHandle);
end

function M:asyncLoad(path, pThis, evtHandle, progressHandle)
	self:updatePath(path);
	self:addEventHandle(pThis, evtHandle, progressHandle);
end

function M:unload()
	if (nil ~= self.mEvtHandle) then
		self.mEvtHandle:clearEventHandle();
		self.mEvtHandle = nil;
	end

	if (self.mProgressEventDispatch ~= nil) then
		self.mProgressEventDispatch:clearEventHandle();
		self.mProgressEventDispatch = nil;
	end

	self:reset();
end

function M:onProgressEventHandle(dispObj)
	if(nil ~= self.mProgressEventDispatch) then
		self.mProgressEventDispatch:dispatchEvent(dispObj);
	end
end

return M;