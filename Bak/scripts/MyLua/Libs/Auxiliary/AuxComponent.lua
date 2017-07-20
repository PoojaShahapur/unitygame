local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxComponent";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mSelfGo = nil;                  -- 自己节点
    self.mPntGo = nil;                   -- 指向父节点
    self.mPlaceHolderGo = nil;           -- 自己节点，资源挂在 mPlaceHolderGo 上， mPlaceHolderGo 挂在 mPntGo 上
    self.mIsNeedPlaceHolderGo = nil;     -- 是否需要占位 GameObject
    self.mIsVisible = true;
	self.mIsVisibleDirty = false;
	self.mIsDestroySelf = true;
	
	self.mIsMoveToTop = false;
	self.mIsMoveToTopDirty = false;
end

function M:dtor()
	
end

function M:init()
	
end

function M:setSelfName(name_)
	GlobalNS.UtilApi.setGoName(self.mSelfGo, name_);
end

function M:setSelfGo(value)
    local isPntChange = self:isChange(self.mSelfGo, value);
	
    self.mSelfGo = value;
	
    if (isPntChange) then
        self:onSelfChanged();
    end
    
    self:syncVisible();
	self:syncIsMoveToTop();
end

function M:getSelfGo()
    return self.mSelfGo;
end

function M:setSelfGoByPath(parentGo, path)
	self:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(parentGo, path));
end

function M:setPntGo(value)
    local isPntChange = self:isChange(self.mPntGo, value);
    self.mPntGo = value;
	
    if (isPntChange) then
        self:onPntChanged();
    end
end

function M:getPntGo()
    return self.mPntGo;
end

function M:getNeedPlaceHolderGo()
    return self.mIsNeedPlaceHolderGo;
end

function M:setNeedPlaceHolderGo(value)
	if(self.mIsNeedPlaceHolderGo ~= value) then
		self.mIsNeedPlaceHolderGo = value;
	end
	
    if(self.mIsNeedPlaceHolderGo) then
        if (self.mPlaceHolderGo == nil) then
            self.mPlaceHolderGo = GlobalNS.UtilApi.createGameObject("PlaceHolderGO");
        end
    end
end

function M:getPlaceHolderGo()
    return self.mPlaceHolderGo;
end

function M:setPlaceHolderGo(value)
    self.mPlaceHolderGo = value;
end

function M:isSelfValid()
    return self.mSelfGo ~= nil;
end

function M:isDestroySelf()
	return self.mIsDestroySelf;
end

function M:setIsDestroySelf(value)
	self.mIsDestroySelf = value;
end

function M:dispose()
    if (self.mIsNeedPlaceHolderGo and self.mPlaceHolderGo ~= nil) then
        GlobalNS.UtilApi.Destroy(self.mPlaceHolderGo);
		self.mPlaceHolderGo = nil;
    end
	
	if (self.mIsDestroySelf) then
		if(nil ~= self.mSelfGo) then
			GlobalNS.UtilApi.Destroy(self.mSelfGo);
			self.mSelfGo = nil;
		end
	end
end

function M:isChange(srcGO, destGO)
	local ret = false;
	
    if(srcGO ~= destGO and nil ~= destGO) then
        ret = true;
    end

    return ret;
end

-- 父节点发生改变
function M:onPntChanged()
    self:linkSelf2Parent();
end

-- 自己发生改变
function M:onSelfChanged()
    self:linkSelf2Parent();
end

function M:linkPlaceHolder2Parent()
    if (self.mPlaceHolderGo == nil) then
        self.mPlaceHolderGo = GlobalNS.UtilApi.createGameObject("PlaceHolderGO");
    end
	
    GlobalNS.UtilApi.SetParent(self.mPlaceHolderGo, self.mPntGo, false);
end

function M:linkSelf2Parent()
    if (self.mSelfGo ~= nil and self.mPntGo ~= nil) then   -- 现在可能还没有创建
        GlobalNS.UtilApi.SetParent(self.mSelfGo, self.mPntGo, false);
    end
end

function M:show()
	-- 如果资源中设置不可见，但是资源还没有加载就设置了这个值，如果进行判断就会有问题
	--if(self.mIsVisible ~= true) then
	self.mIsVisible = true;
	self.mIsVisibleDirty = true;
	--end
	
	self:syncVisible();
end

function M:hide()
	if(self.mIsVisible ~= false) then
		self.mIsVisible = false;
		self.mIsVisibleDirty = true;
	end
	
	self:syncVisible();
end

function M:IsVisible()
    -- return GlobalNS.UtilApi.IsActive(self.mSelfGo);
	return self.mIsVisible;
end

function M:syncVisible()
	if (self.mSelfGo ~= nil) then
		if(self.mIsVisibleDirty) then
			self.mIsVisibleDirty = false;
			GlobalNS.UtilApi.SetActive(self.mSelfGo, self.mIsVisible);
		else
			self.mIsVisible = GlobalNS.UtilApi.IsActive(self.mSelfGo);
		end
    end
end

function M:setVisible(isVisible)
	if(isVisible) then
		self:show();
	else
		self:hide();
	end
end

function M:setIsMoveToTop(value)
	self.mIsMoveToTop = value;
	self.mIsMoveToTopDirty = true;
	
	self:syncIsMoveToTop();
end

function M:syncIsMoveToTop()
	if (self.mSelfGo ~= nil) then
		if(self.mIsMoveToTopDirty) then
			self.mIsMoveToTopDirty = false;
			GlobalNS.UtilApi.SetSiblingIndexToLastTwoByGo(self.mSelfGo);
		end
    end
end

return M;