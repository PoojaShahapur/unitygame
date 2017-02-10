MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxImage";
GlobalNS[M.clsName] = M;

function M:ctor(...)
	self.mNativeImage = nil;
	self.mScale = 1;
	self.mAuxSpriteAtlasLoader = nil;
	self.mSpritePath = "";
	self.mSprite = nil;
end

function M:dtor()
	
end

function M:dispose()
	self.mSpritePath = "";
	self.mSprite = nil;
	
	if(nil ~= self.mAuxSpriteAtlasLoader) then
		self.mAuxSpriteAtlasLoader:dispose();
		self.mAuxSpriteAtlasLoader = nil;
	end
end

function M:setScale(value)
	self.mScale = value;
	self:setGoRectScale();
end

function M:onSelfChanged()
	M.super.onSelfChanged(self);
	
	self.mNativeImage = GlobalNS.UtilApi.getImageCompNoPath(self.mSelfGo);
	self:updateSprite();
	
	self:setGoRectScale();
end

function M:setGoRectScale()
	if(nil ~= self.mSelfGo) then
		GlobalNS.UtilApi.setGoRectScale(self.mSelfGo, Vector3.New(self.mScale , 1.0, 1.0));
	end
end

--精灵都同步加载
function M:setSpritePath(path)
	if(not GlobalNS.UtilStr.IsNullOrEmpty(path)) then
		if(nil ~= self.mAuxSpriteAtlasLoader) then
			self.mAuxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
		end
		
		self.mAuxSpriteAtlasLoader.syncLoad("", self, self.onSpriteLoaded, nil);
	end
end

function M:onSpriteLoaded(dispObj)
	self.mSprite = self.mAuxSpriteAtlasLoader:getSprite(self.mSpritePath);
	
	self:updateSprite();
end

function M:updateSprite()
	if(nil ~= self.mNativeImage and nil ~= self.mSprite) then
		GlobalNS.UtilApi.setImageSpriteBySprite(self.mNativeImage, self.mSprite);
	end
end

return M;