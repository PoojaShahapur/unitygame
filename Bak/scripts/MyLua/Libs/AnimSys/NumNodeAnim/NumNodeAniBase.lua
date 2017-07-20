MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NumNodeAniBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	--self.mBufferObject = new BufferObjectBase();

	self:onGetFromPool();
end

function M:init()
	--self.mBufferObject.init();
end

function M:dispose()
	self:onPutInPool();
	self:onDestroy();
end

function M:onDestroy()

end

function M:setClientDispose(isDispose)

end

function M:isClientDispose()
	return false;
end

function M:getBufferType()
	return 0;
end

function M:onGetFromPool()
	self.mNumIncOrDecAnimMode = GlobalNS.NumIncOrDecAnimMode.eInc;
	--self.mBufferObject.getFromPool();
end

function M:onPutInPool()
	if (nil ~= GCtx.mNumNodeAnimSys) then
		GCtx.mNumNodeAnimSys:removeNodeNumAnim(self);
	end

	--self.mBufferObject.putInPool();
end

function M:putInPool()
	self:onPutInPool();
end

function M:getFromPool()
	self:onGetFromPool();
end

function M:setIsUsePool(value)

end

function M:isUsePool()
	--return self.mBufferObject.isUsePool();
end

function M:startAnim()
	if (nil ~= GCtx.mNumNodeAnimSys) then
		GCtx.mNumNodeAnimSys:addNodeNumAnim(self);
	end
end

function M:stopAnim()
	if (nil ~= GCtx.mNumNodeAnimSys) then
		GCtx.mNumNodeAnimSys:removeNodeNumAnim(self);
	end
end

function M:getNumIncOrDecAnimMode()
	return self.mNumIncOrDecAnimMode;
end

function M:setNumIncOrDecAnimMode(value)
	self.mNumIncOrDecAnimMode = value;
end

function M:onTick(delta, tickMode)

end

-- 需要执行下一个动画
function M:onNextAnim()

end

return M;