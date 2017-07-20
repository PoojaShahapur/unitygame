MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Core.ClassLoader");
MLoader("MyLua.Libs.DataStruct.MStack");
MLoader("MyLua.Libs.DataStruct.MDictionary");
MLoader("MyLua.Libs.UI.UICore.UIFormId");
MLoader("MyLua.Libs.UI.UICore.UICanvas");
MLoader("MyLua.Libs.Auxiliary.AuxLoader.AuxUIPrefabLoader");

--[[
	@brief UI 当前只支持每一次只打开一个界面，在当前界面资源还没有加载完成的时候，需要转圈等待，直到加载完成，否则会有显示上先后顺序的问题，目前最后一个资源加载完成的界面永远在最上面，可能这个界面是最先打开的，但是由于是最后一个资源加载完成的，因此会显示在最上面
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UIMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mId2FormDic = GlobalNS.new(GlobalNS.MDictionary);
    self.mCurFormIndex = GlobalNS.UtilMath.InvalidIndex;
    self.mFormIdStack = GlobalNS.new(GlobalNS.MStack);
	self.mFormId2LoadItemDic = GlobalNS.new(GlobalNS.MDictionary);
	
	self.mFormBaseId = 10000;		-- Form Base 起始的 Id,需要与 CS 中 Form Id 不一样，否则会覆盖 CS 中的设置
	self.mUniqueNumIdGen = GlobalNS.new(GlobalNS.UniqueNumIdGen, self.mFormBaseId); 	-- FormId 唯一 Id 生成
end

function M:dtor()

end

function M:init()
	-- UIFormId 初始化构造操作
	GlobalNS.UIFormId.init();
	-- 属性系统初始化
	GlobalNS.UIAttrSystem.init();
	self:initCanvas();
end

function M:dispose()
	self:exitAllForm();
end

function M:initCanvas()
    if(self.mCanvasList == nil) then
        self.mCanvasList = GlobalNS.new(GlobalNS.MList);
        
        local canvas;
        -- eBtnCanvas，原来默认的放在这个上
        canvas = GlobalNS.new(GlobalNS.UICanvas);
        self.mCanvasList:add(canvas);
        canvas:setGoName(GlobalNS.NoDestroyId.ND_CV_UIFirstCanvas);
        canvas:init();
        
        -- eFirstCanvas
        canvas = GlobalNS.new(GlobalNS.UICanvas);
        self.mCanvasList:add(canvas);
        canvas:setGoName(GlobalNS.NoDestroyId.ND_CV_UISecondCanvas);
        canvas:init();
    end
end

function M:getLayerGo(canvasId, layerId)
    -- 默认放在最底下的 Canvas，第二层
    if(canvasId == nil) then
        canvasId = GlobalNS.UICanvasId.eUIFirstCanvas;
    end
	
    if(layerId == nil) then
        layerId = GlobalNS.UILayerID.eUISecondLayer;
    end
	
    GlobalNS.UtilApi.assert(canvasId < self.mCanvasList:Count());
    return self.mCanvasList:get(canvasId):getLayerGo(layerId);
end

function M:showForm(formId)
    -- 如果当前显示的不是需要显示的
	-- 保证没有在显示之前删除
	if(self:hasForm(formId)) then
		if(self.mCurFormIndex ~= formId) then
			local curFormIndex_ = self.mCurFormIndex;
			self:showFormNoClosePreForm(formId);
			self.mCurFormIndex = curFormIndex_;
			
			self:pushAndHideForm(formId);
			self.mCurFormIndex = formId;
		end
	end
end

function M:showFormNoClosePreForm(formId)
	local form = self.mId2FormDic:value(formId);
	
    if(form ~= nil) then
		if(not form:isVisible()) then
			--[[
			if(form:isReady()) then
				GlobalNS.UtilApi.SetActive(form.mGuiWin, true);
			end
			]]
			
			form:onShow();
			form:syncVisible();
		end
		
        self.mCurFormIndex = formId;
    end
    
    self.mFormIdStack:removeAllEqual(formId);
end

-- 仅仅加载 lua 脚本，不加载资源
function M:loadFormScript(formId, param)
    if(not self:hasForm(formId)) then
        local codePath = GlobalNS.UIAttrSystem[formId].mLuaScriptPath;
        local formCls = GlobalNS.ClassLoader.loadClass(codePath);
		
		local form = GlobalNS.new(formCls, param);
        self.mId2FormDic:add(formId, form);
		
		form:setId(formId);
        form:init();
    end
end

-- 加载脚本并且加载资源
function M:loadForm(formId, param)
    if(not self:hasForm(formId)) then
        self:loadFormScript(formId, param);
    end
    
    if(not self:hasLoadItem(formId)) then
		local uiPrefabLoader = GlobalNS.new(GlobalNS.AuxUIPrefabLoader);
		uiPrefabLoader:setIsNeedInsPrefab(true);
		uiPrefabLoader:setIsInsNeedCoroutine(false);
		
		self.mFormId2LoadItemDic:Add(formId, uiPrefabLoader);
		uiPrefabLoader:setFormId(formId);
		uiPrefabLoader:asyncLoad(GlobalNS.UIAttrSystem[formId].mWidgetPath, self, self.onFormPrefabLoaded, nil);
    end
end

function M:loadAndShow(formId, param)
    if(not self:hasForm(formId)or not self:hasLoadItem(formId)) then
        self:loadForm(formId, param);
    end
	
	if(self:hasForm(formId) and not self.mId2FormDic:value(formId):isHideOnCreate()) then
		self:showForm(formId);
	end
	
    return self.mId2FormDic:value(formId);
end

function M:hideForm(formId)
    local bFormVisible = false;
    local form = self.mId2FormDic:value(formId);
	
    if(form ~= nil) then
        bFormVisible = form:isVisible();
    end
    
    self:hideFormNoOpenPreForm(formId);
    
    -- 只有当前界面是显示的时候，关闭这个界面才打开之前的界面
    if(bFormVisible) then
        -- 显示之前隐藏的窗口
        self:popAndShowForm(formId);
    end
end

function M:hideFormNoOpenPreForm(formId)
    local form = self.mId2FormDic:value(formId);
	
    --if(form.mGuiWin ~= nil and GlobalNS.UtilApi.IsActive(form.mGuiWin)) then
	if(form:isVisible()) then
        form:onHide();
        form:syncVisible();
        self.mCurFormIndex = GlobalNS.UtilMath.InvalidIndex;
    end
end

function M:exitForm(formId)
    local isFormVisible = false;
    local form = self.mId2FormDic:value(formId);
	
    if(form ~= nil) then
        isFormVisible = form:isVisible();
		
		self:exitFormNoOpenPreForm(formId);
    
		-- 只有当前界面是显示的时候，关闭这个界面才打开之前的界面
		if(isFormVisible) then
			-- 显示之前隐藏的窗口
			self:popAndShowForm(formId);
		end
    end
end

-- 关闭当前窗口，不用打开之前的窗口
function M:exitFormNoOpenPreForm(formId)
    local form = self.mId2FormDic:value(formId);
	
    -- 关闭当前窗口
    if(form ~= nil) then
        form:onHide();
        form:onExit();
        GlobalNS.delete(form);
		
        self:unloadLoadItem(formId);
        self.mId2FormDic:Remove(formId);
        self.mCurFormIndex = GlobalNS.UtilMath.InvalidIndex;
    end
    
    self.mFormIdStack:removeAllEqual(formId);
end

-- 弹出并且显示界面
function M:popAndShowForm(formId)
    -- 显示之前隐藏的窗口
    if(GlobalNS.UIAttrSystem[formId].mPreFormModeWhenClose == GlobalNS.PreFormModeWhenClose.eSHOW) then
        local curFormIndex_ = self.mFormIdStack:pop();
		
        if(curFormIndex_ == nil) then
            self.mCurFormIndex = GlobalNS.UtilMath.InvalidIndex;
        else
            self:showFormNoClosePreForm(curFormIndex_);
        end
    end
end

function M:pushAndHideForm(formId)
    if(GlobalNS.UIAttrSystem[formId].mPreFormModeWhenOpen == GlobalNS.PreFormModeWhenOpen.eCLOSE) then
        if(self.mCurFormIndex ~= GlobalNS.UtilMath.InvalidIndex) then
            self:exitFormNoOpenPreForm(self.mCurFormIndex);
        end
    elseif(GlobalNS.UIAttrSystem[formId].mPreFormModeWhenOpen == GlobalNS.PreFormModeWhenOpen.eHIDE) then
        if(self.mCurFormIndex ~= GlobalNS.UtilMath.InvalidIndex) then
            -- 将当前窗口 Id 保存，当前窗口 FormId 是不在堆栈中的
            self.mFormIdStack:push(self.mCurFormIndex);
            -- 隐藏当前窗口
            self:hideFormNoOpenPreForm(self.mCurFormIndex);
        end
    end
end

function M:getForm(formId)
    return self.mId2FormDic:value(formId);
end

function M:hasForm(formId)
    local has = false;
	has = self.mId2FormDic:ContainsKey(formId);
    return has;
end

function M:isFormVisible(formId)
	local ret = false;
	local form = self:getForm(formId);
	
	if(nil ~= form and form:isVisible()) then
		ret = true;
	end
	
	return ret;
end

function M:hasLoadItem(formId)
	return self.mFormId2LoadItemDic:ContainsKey(formId);
end

function M:unloadLoadItem(formId)
	if(self.mFormId2LoadItemDic:ContainsKey(formId)) then
		self.mFormId2LoadItemDic:value(formId):dispose();
		self.mFormId2LoadItemDic:Remove(formId);
	end
end

-- dispObj : AuxUIPrefabLoader
function M:onFormPrefabLoaded(dispObj)
	local formId = dispObj:getFormId();
	local form = self.mId2FormDic:value(formId);
	
	if(form ~= nil) then
		local parent = self:getLayerGo(GlobalNS.UIAttrSystem[form.mId].mCanvasId, GlobalNS.UIAttrSystem[form.mId].mLayerId);
        form.mGuiWin = self.mFormId2LoadItemDic:value(formId):getSelfGo();
		
		GlobalNS.UtilApi.SetParent(form.mGuiWin, parent, false);
        --GlobalNS.UtilApi.SetActive(form.mGuiWin, false);     -- 加载完成后先隐藏，否则后面 showForm 判断会有问题
        form:onReady();
		form:syncVisible();
	else
		self:unloadLoadItem(formId);
	end
end

function M:exitAllForm()
	local resUniqueIdList = GlobalNS.new(GlobalNS.MList);

	for key, value in pairs(self.mId2FormDic:getData()) do
		resUniqueIdList:Add(key);
	end

	local idx = 0;
	local listLen = resUniqueIdList:length();

	while (idx < listLen) do
		self:exitForm(resUniqueIdList:get(idx));
		idx = idx + 1;
	end

	resUniqueIdList:Clear();
	resUniqueIdList = nil;

	self.mId2FormDic:clear();
end

return M;