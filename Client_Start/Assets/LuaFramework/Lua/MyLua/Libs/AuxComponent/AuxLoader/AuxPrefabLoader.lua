MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.AuxComponent.AuxLoader.AuxLoaderBase");

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

function M:asyncLoad(path, pThis, handle)
    self.mEvtHandle = GlobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle:addEventHandle(pThis, handle);
    self.mNativePrefabLoader = GlobalNS.CSSystem.AuxPrefabLoader.New("", self.mIsNeedInsPrefab, self.mIsInsNeedCoroutine);
    self.mNativePrefabLoader:asyncLoad(path, self, self.onPrefabLoaded);
end

function M:onPrefabLoaded(dispObj)
	self.mNativePrefabLoader = dispObj[0];
	--self.mNativePrefabLoader = dispObj;
	--local typeId = self.mNativePrefabLoader:getTypeId();
    self:setSelfGo(self.mNativePrefabLoader:getGameObject());
    self.mEvtHandle:dispatchEvent(self);
end

return M;