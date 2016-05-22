local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxComponent";
GlobalNS[M.clsName] = M;

function M:AuxComponent()
    self.m_selfGo = nil;                  -- 自己节点
    self.m_pntGo = nil;                   -- 指向父节点
    self.m_placeHolderGo = nil;           -- 自己节点，资源挂在 m_placeHolderGo 上， m_placeHolderGo 挂在 m_pntGo 上
    self.m_bNeedPlaceHolderGo = nil;      -- 是否需要占位 GameObject
    self.m_visible = true;
end

function M:setSelfName(name_)
    this.selfGo.name = name_;
end

function M:setSelfGo(value)
    local bPntChange = self:bChange(self.m_selfGo, value);
    self.m_selfGo = value;
    if (bPntChange) then
        self:onSelfChanged();
    end
    
    GlobalNS.UtilApi.SetActive(self.m_sekfGo, self.m_visible);
end

function M:getSelfGo()
    return self.m_selfGo;
end

function M:setPntGo(value)
    local bPntChange = self:bChange(self.m_pntGo, value);
    self.m_pntGo = value;
    if (bPntChange) then
        self:onPntChanged();
    end
end

function M:getPntGo()
    return self.m_pntGo;
end

function M:getNeedPlaceHolderGo()
    return self.m_bNeedPlaceHolderGo;
end

function M:setNeedPlaceHolderGo(value)
    self.m_bNeedPlaceHolderGo = value;
    if(self.m_bNeedPlaceHolderGo) then
        if (self.m_placeHolderGo == nil) then
            self.m_placeHolderGo = GlobalNS.UtilApi.createGameObject("PlaceHolderGO");
        end
    end
end

function M:getPlaceHolderGo()
    return self.m_placeHolderGo;
end

function M:setPlaceHolderGo(value)
    self.m_placeHolderGo = value;
end

function M:isSelfValid()
    return self.m_selfGo ~= nil;
end

function M:dispose()
    if (self.m_bNeedPlaceHolderGo ~= nil and self.m_placeHolderGo ~= nil) then
        GlobalNS.UtilApi.Destroy(self.m_placeHolderGo);
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
    if (self.m_placeHolderGo == nil) then
        self.m_placeHolderGo = GlobalNS.UtilApi.createGameObject("PlaceHolderGO");
    end
    GlobalNS.UtilApi.SetParent(self.m_placeHolderGo, self.m_pntGo, false);
end

function M:linkSelf2Parent()
    if (self.m_selfGo ~= nil and self.m_pntGo ~= nil) then   -- 现在可能还没有创建
        GlobalNS.UtilApi.SetParent(self.m_selfGo, self.m_pntGo, false);
    end
end

function M:show()
    if (self.m_selfGo ~= nil) then
        GlobalNS.UtilApi.SetActive(self.m_selfGo, true);
    end
end

function M:hide()
    if (self.m_selfGo ~= nil and self:IsVisible()) then
        GlobalNS.UtilApi.SetActive(self.m_selfGo, false);
    end
end

function M:IsVisible()
    return GlobalNS.UtilApi.IsActive(self.m_selfGo);
end