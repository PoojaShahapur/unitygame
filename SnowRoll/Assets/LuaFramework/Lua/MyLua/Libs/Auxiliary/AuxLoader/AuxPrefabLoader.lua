MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxPrefabLoader";
GlobalNS[M.clsName] = M;

function M:ctor(path)
    self.mSelfGo = nil;
	self.mNativePrefabLoader = nil;

	--[[
	if(GlobalNS.UtilApi.isTrue(isNeedInsPrefab)) then
		self.mIsNeedInsPrefab = isNeedInsPrefab;
	else
		self.mIsNeedInsPrefab = false;
	end
	]]
	self.mIsNeedInsPrefab = true;
	
	--[[
	if(GlobalNS.UtilApi.isTrue(isNeedInsPrefab)) then
		self.mIsInsNeedCoroutine = isInsNeedCoroutine;
	else
		self.mIsInsNeedCoroutine = false;
	end
	]]
	self.mIsInsNeedCoroutine = true;
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if(self.mNativePrefabLoader ~= nil) then
        self.mNativePrefabLoader:dispose();
        self.mNativePrefabLoader = nil;
    end
	
	self.mSelfGo = nil;
	
	M.super.dispose(self);
end

-- 是否需要实例化预制
function M:setIsNeedInsPrefab(value)
	self.mIsNeedInsPrefab = value;
end

-- 是否需要协程实例化预制
function M:setIsInsNeedCoroutine(value)
	self.mIsInsNeedCoroutine = value;
end

function M:setSelfGo(value)
	self.mSelfGo = value;
end

function M:getSelfGo()
	return self.mSelfGo;
end

function M:getPrefabTmpl()
	return self.mNativePrefabLoader:getPrefabTmpl();
end

function M:syncLoad(path, pThis, handle, progressHandle)
	M.super.syncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativePrefabLoader) then
			self.mNativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New("");
		end
		self.mNativePrefabLoader:setIsNeedInsPrefab(self.mIsNeedInsPrefab);
		self.mNativePrefabLoader:setIsInsNeedCoroutine(self.mIsInsNeedCoroutine);
		
		if(nil == progressHandle) then
			self.mNativePrefabLoader:syncLoad(path, self, self.onPrefabLoaded, nil);
		else
			self.mNativePrefabLoader:syncLoad(path, self, self.onPrefabLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		--self:onPrefabLoaded(self.mNativePrefabLoader);
		self:onPrefabLoaded(nil);
	end
end

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		if(nil == self.mNativePrefabLoader) then
			self.mNativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New("");
		end
		self.mNativePrefabLoader:setIsNeedInsPrefab(self.mIsNeedInsPrefab);
		self.mNativePrefabLoader:setIsInsNeedCoroutine(self.mIsInsNeedCoroutine);
		
		if(nil == progressHandle) then
			self.mNativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded, nil);
		else
			self.mNativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		--self:onPrefabLoaded(self.mNativePrefabLoader);
		self:onPrefabLoaded(nil);
	end
end

function M:onPrefabLoaded(dispObj)
	if(nil ~= dispObj) then
		self.mNativePrefabLoader = dispObj[0];
		--self.mNativePrefabLoader = dispObj;
		--local typeId = self.mNativePrefabLoader:getTypeId();
		self:setSelfGo(self.mNativePrefabLoader:getGameObject());
		
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