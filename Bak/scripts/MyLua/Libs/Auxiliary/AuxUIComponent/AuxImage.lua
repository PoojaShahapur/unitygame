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
	self.mIsScaleDirty = false;
	self.mAuxSpriteAtlasLoader = nil;
	self.mSpritePath = "";	--精灵目录
	self.mSpriteName = "";	--精灵名字
	self.mIsSpriteDirty = false;
	self.mSprite = nil;
	
	self.mIsNeedHideWhenLoadSprite = false;
end

function M:dtor()
	
end

function M:dispose()
	self.mSpritePath = "";
	self.mSprite = nil;
	self.mIsSpriteDirty = false;
	
	if(nil ~= self.mAuxSpriteAtlasLoader) then
		self.mAuxSpriteAtlasLoader:dispose();
		self.mAuxSpriteAtlasLoader = nil;
	end
end

function M:setScale(value)
	if(self.mScale ~= value) then
		self.mIsScaleDirty = true;
		self.mScale = value;
	end
	
	self:syncRectScale();
end

function M:getScale()
	return self.mScale;
end

function M:onSelfChanged()
	M.super.onSelfChanged(self);
	
	self.mNativeImage = GlobalNS.UtilApi.getImageCompNoPath(self.mSelfGo);
	
	if(nil ~= self.mSprite) then
		self.mIsSpriteDirty = true;
	end

	self:syncSprite();
	self:syncIsNeedHideWhenLoadSprite();
	self:syncRectScale();
end

function M:syncRectScale()
	if(nil ~= self.mSelfGo) then
		if(self.mIsScaleDirty) then
			self.mIsScaleDirty = false;
			GlobalNS.UtilApi.setGoRectScale(self.mSelfGo, Vector3.New(self.mScale, 1.0, 1.0));
		else
			self.mScale = GlobalNS.UtilApi.getGoRectScaleX(self.mSelfGo);
		end
	end
end

--精灵都同步加载
function M:setSpritePath(spritePath, spriteName, isSyncLoad)
	if(not GlobalNS.UtilStr.IsNullOrEmpty(spritePath) and not GlobalNS.UtilStr.IsNullOrEmpty(spriteName)) then
		if(self.mSpritePath ~= spritePath or self.mSpriteName ~= spriteName) then
			self.mIsSpriteDirty = true;
			self.mSprite = nil;
			self.mSpritePath = spritePath;
			self.mSpriteName = spriteName;
			
			if(nil ~= self.mAuxSpriteAtlasLoader) then
				self.mAuxSpriteAtlasLoader:dispose();
				self.mAuxSpriteAtlasLoader = nil;
			end
			
			if(nil == self.mAuxSpriteAtlasLoader) then
				--self.mAuxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxSpriteAtlasLoader);
				self.mAuxSpriteAtlasLoader = GlobalNS.new(GlobalNS.AuxUnityAtlasLoader);
			end
			
			-- 同步加载还是异步加载
			if(isSyncLoad) then
				self.mAuxSpriteAtlasLoader:syncLoad(spritePath, self, self.onSpriteLoaded, nil);
			else
				self.mAuxSpriteAtlasLoader:asyncLoad(spritePath, self, self.onSpriteLoaded, nil);
			end
			
			--如果异步加载立刻加载完成，就不用再更新了，否则需要隐藏一下
			self:syncIsNeedHideWhenLoadSprite();
		end
	end
end

function M:onSpriteLoaded(dispObj)
	self.mSprite = self.mAuxSpriteAtlasLoader:getSprite(self.mSpriteName);

	self:syncSprite();
	self:syncIsNeedHideWhenLoadSprite();
end

function M:syncSprite()
	if(nil ~= self.mNativeImage) then
		if(self.mIsSpriteDirty) then
			if(nil ~= self.mSprite) then
				self.mIsSpriteDirty = false;
				GlobalNS.UtilApi.setImageSpriteBySprite(self.mNativeImage, self.mSprite);
			end
		end
	end
end

function M:clearImage()
	if(nil ~= self.mNativeImage) then
		GlobalNS.UtilApi.setImageSpriteBySprite(self.mNativeImage, nil);
	end
end

function M:setIsNeedHideWhenLoadSprite(value)
	self.mIsNeedHideWhenLoadSprite = value;
end

function M:syncIsNeedHideWhenLoadSprite()
	if(self.mIsNeedHideWhenLoadSprite) then
		if(self.mIsSpriteDirty) then
			if(self:IsVisible() and nil ~= self.mSelfGo) then
				GlobalNS.UtilApi.SetActive(self.mSelfGo, false);
			end
		else
			if(self:IsVisible() and nil ~= self.mSelfGo) then
				GlobalNS.UtilApi.SetActive(self.mSelfGo, true);
			end
		end
	end
end

return M;