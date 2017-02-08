MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxLoaderBase");

local M = GlobalNS.Class(GlobalNS.AuxLoaderBase);
M.clsName = "AuxPrefabLoader";
GlobalNS[M.clsName] = M;

function M:ctor(path, isNeedInsPrefab, isInsNeedCoroutine)
    self.mSelfGo = nil;
	self.mNativePrefabLoader = nil;

	if(GlobalNS.UtilApi.isTrue(isNeedInsPrefab)) then
		self.mIsNeedInsPrefab = isNeedInsPrefab;
	else
		self.mIsNeedInsPrefab = false;
	end
	
	if(GlobalNS.UtilApi.isTrue(isNeedInsPrefab)) then
		self.mIsInsNeedCoroutine = isInsNeedCoroutine;
	else
		self.mIsInsNeedCoroutine = false;
	end
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

function M:asyncLoad(path, pThis, handle, progressHandle)
	M.super.asyncLoad(self, path, pThis, handle, progressHandle);
	
	if (self:isInvalid()) then
		self.mNativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New("", self.mIsNeedInsPrefab, self.mIsInsNeedCoroutine);
		
		if(nil == progressHandle) then
			self.mNativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded, nil);
		else
			self.mNativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded, self.onProgressEventHandle);
		end
	elseif (self:hasLoadEnd()) then
		self:onPrefabLoaded(self.mNativePrefabLoader);
	end
end

function M:onPrefabLoaded(dispObj)
	self.mNativePrefabLoader = dispObj[0];
	--self.mNativePrefabLoader = dispObj;
	--local typeId = self.mNativePrefabLoader:getTypeId();
    self:setSelfGo(self.mNativePrefabLoader:getGameObject());
    self.mEvtHandle:dispatchEvent(self);
end

return M;