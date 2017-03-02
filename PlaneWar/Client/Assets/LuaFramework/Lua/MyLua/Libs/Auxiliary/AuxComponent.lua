local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxComponent";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mSelfGo = nil;                  -- 自己节点
    self.mPntGo = nil;                   -- 指向父节点
    self.mPlaceHolderGo = nil;           -- 自己节点，资源挂在 mPlaceHolderGo 上， mPlaceHolderGo 挂在 mPntGo 上
    self.mIsNeedPlaceHolderGo = nil;      -- 是否需要占位 GameObject
    self.mIsVisible = true;
end

function M:dtor()
	
end

function M:setSelfName(name_)
    this.selfGo.name = name_;
end

function M:setSelfGo(value)
    local bPntChange = self:bChange(self.mSelfGo, value);
    self.mSelfGo = value;
	
    if (bPntChange) then
        self:onSelfChanged();
    end
    
    GlobalNS.UtilApi.SetActive(self.mSelfGo, self.mIsVisible);
end

function M:getSelfGo()
    return self.mSelfGo;
end

function M:setPntGo(value)
    local bPntChange = self:bChange(self.mPntGo, value);
    self.mPntGo = value;
    if (bPntChange) then
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
    self.mIsNeedPlaceHolderGo = value;
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

function M:dispose()
    if (self.mIsNeedPlaceHolderGo ~= nil and self.mPlaceHolderGo ~= nil) then
        GlobalNS.UtilApi.Destroy(self.mPlaceHolderGo);
    end
end

function M:bChange(srcGO, destGO)
    if (srcGO == nil or srcGO ~= destGO) then
        return true;
    end

    return false;
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
	self.mIsVisible = true;
	
    if (self.mSelfGo ~= nil) then
        GlobalNS.UtilApi.SetActive(self.mSelfGo, self.mIsVisible);
    end
end

function M:hide()
	self.mIsVisible = false;
	
    if (self.mSelfGo ~= nil) then
        GlobalNS.UtilApi.SetActive(self.mSelfGo, self.mIsVisible);
    end
end

function M:IsVisible()
    -- return GlobalNS.UtilApi.IsActive(self.mSelfGo);
	return self.mIsVisible;
end