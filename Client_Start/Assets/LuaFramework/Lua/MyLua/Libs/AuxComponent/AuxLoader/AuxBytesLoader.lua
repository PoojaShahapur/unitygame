-- 二进制资源加载
MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.AuxComponent.AuxLoader.AuxLoaderBase");

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
end

function M:getNativeLoader()
	return self.mNativeBytesLoader;
end

function M:getBytes()
	return self.mNativeBytesLoader.getBytes();
end

function M:syncLoad(path, pThis, handle)
    self.mEvtHandle = GlobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle:addEventHandle(pThis, handle);
    self.mNativeBytesLoader = GlobalNS.CSSystem.AuxBytesLoader.New("");
    self.mNativeBytesLoader:syncLoad(path, self, self.onBytesLoaded);
end

function M:asyncLoad(path, pThis, handle)
    self.mEvtHandle = GlobalNS.new(GlobalNS.ResEventDispatch);
    self.mEvtHandle:addEventHandle(pThis, handle);
    self.mNativeBytesLoader = GlobalNS.CSSystem.AuxBytesLoader.New("");
    self.mNativeBytesLoader:asyncLoad(path, self, self.onBytesLoaded);
end

function M:onBytesLoaded(dispObj)
	self.mNativeBytesLoader = dispObj[0];
    self.mEvtHandle:dispatchEvent(self);
end

return M;