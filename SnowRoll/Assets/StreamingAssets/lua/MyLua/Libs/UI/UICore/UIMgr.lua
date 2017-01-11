MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Core.ClassLoader");
MLoader("MyLua.Libs.DataStruct.MStack");
MLoader("MyLua.Libs.DataStruct.MDictionary");
MLoader("MyLua.Libs.UI.UICore.UIFormId");
MLoader("MyLua.Libs.UI.UICore.UICanvas");
MLoader("MyLua.Libs.AuxComponent.AuxLoader.AuxUIPrefabLoader");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UIMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mFormArr = {};
    self.mCurFormIndex = -1;
    self.mFormIdStack = GlobalNS.new(GlobalNS.MStack);
	self.mFormId2LoadItemDic = GlobalNS.new(GlobalNS.MDictionary);
	
	self.mFormBaseI = 10000;		-- Form Base 起始的 Id,需要与 CS 中 Form Id 不一样，否则会覆盖 CS 中的设置
	self.mUniqueNumIdGen = GlobalNS.new(GlobalNS.UniqueNumIdGen, self.mFormBaseI); 	-- FormId 唯一 Id 生成
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

function M:initCanvas()
    if(self.m_canvasList == nil) then
        self.m_canvasList = GlobalNS.new(GlobalNS.MList);
        
        local canvas;
        -- eBtnCanvas，原来默认的放在这个上
        canvas = GlobalNS.new(GlobalNS.UICanvas);
        self.m_canvasList:add(canvas);
        canvas:setGoName(GlobalNS.NoDestroyId.ND_CV_UIFirstCanvas);
        canvas:init();
        
        -- eFirstCanvas
        canvas = GlobalNS.new(GlobalNS.UICanvas);
        self.m_canvasList:add(canvas);
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
    GlobalNS.UtilApi.assert(canvasId < self.m_canvasList:Count());
    return self.m_canvasList:at(canvasId):getLayerGo(layerId);
end

function M:showForm(formId)
    -- 如果当前显示的不是需要显示的
	-- 保证没有在显示之前删除
	if(self.mFormArr[formId] ~= nil) then
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
    if(self.mFormArr[formId] ~= nil) then
		if(not self.mFormArr[formId]:isVisible()) then
			self.mFormArr[formId]:onShow();
			if(self.mFormArr[formId]:isReady()) then
				GlobalNS.UtilApi.SetActive(self.mFormArr[formId].mGuiWin, true);
			end
		end
        self.mCurFormIndex = formId;
    end
    
    self.mFormIdStack:removeAllEqual(formId);
end

-- 仅仅加载 lua 脚本，不加载资源
function M:loadFormScript(formId, param)
    if(self.mFormArr[formId] == nil) then
        local codePath = GlobalNS.UIAttrSystem[formId].mLuaScriptPath;
        local formCls = GlobalNS.ClassLoader.loadClass(codePath);
        self.mFormArr[formId] = GlobalNS.new(formCls, param);
        self.mFormArr[formId]:onInit();
    end
end

-- 加载脚本并且加载资源
function M:loadForm(formId, param)
    if(self.mFormArr[formId] == nil) then
        self:loadFormScript(formId, param);
    end
    
    if(not self:hasLoadItem(formId)) then
		local uiPrefabLoader = GlobalNS.new(GlobalNS.AuxUIPrefabLoader, "", true, true);
		self.mFormId2LoadItemDic:Add(formId, uiPrefabLoader);
		uiPrefabLoader:setFormId(formId);
		uiPrefabLoader:asyncLoad(GlobalNS.UIAttrSystem[formId].mWidgetPath, self, self.onFormPrefabLoaded);
    end
end

function M:loadAndShow(formId, param)
    if(self.mFormArr[formId] == nil or not self:hasLoadItem(formId)) then
        self:loadForm(formId, param);
    end
	if(self.mFormArr[formId] ~= nil and not self.mFormArr[formId]:isHideOnCreate()) then
		self:showForm(formId);
	end
    return self.mFormArr[formId];
end

function M:hideForm(formId)
    local bFormVisible = false;
    local form = self.mFormArr[formId];
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
    local form = self.mFormArr[formId];
    if(form.mGuiWin ~= nil and GlobalNS.UtilApi.IsActive(form.mGuiWin)) then
        form:onHide();
        GlobalNS.UtilApi.SetActive(form.mGuiWin, false);
        self.mCurFormIndex = -1;
    end
end

function M:exitForm(formId)
    local bFormVisible = false;
    local form = self.mFormArr[formId];
    if(form ~= nil) then
        bFormVisible = form:isVisible();
    end
    
    self:exitFormNoOpenPreForm(formId);
    
    -- 只有当前界面是显示的时候，关闭这个界面才打开之前的界面
    if(bFormVisible) then
        -- 显示之前隐藏的窗口
        self:popAndShowForm(formId);
    end
end

-- 关闭当前窗口，不用打开之前的窗口
function M:exitFormNoOpenPreForm(formId)
    local form = self.mFormArr[formId];
    -- 关闭当前窗口
    if(form ~= nil) then
        form:onHide();
        form:onExit();
        GlobalNS.delete(form);
        self:unloadLoadItem(formId);
        self.mFormArr[formId] = nil;
        self.mCurFormIndex = -1;
    end
    
    self.mFormIdStack:removeAllEqual(formId);
end

-- 弹出并且显示界面
function M:popAndShowForm(formId)
    -- 显示之前隐藏的窗口
    if(GlobalNS.UIAttrSystem[formId].mPreFormModeWhenClose == GlobalNS.PreFormModeWhenClose.eSHOW) then
        local curFormIndex_ = self.mFormIdStack:pop();
        if(curFormIndex_ == nil) then
            self.mCurFormIndex = -1;
        else
            self:showFormNoClosePreForm(curFormIndex_);
        end
    end
end

function M:pushAndHideForm(formId)
    if(GlobalNS.UIAttrSystem[formId].mPreFormModeWhenOpen == GlobalNS.PreFormModeWhenOpen.eCLOSE) then
        if(self.mCurFormIndex >= 0) then
            self:exitFormNoOpenPreForm(self.mCurFormIndex);
        end
    elseif(GlobalNS.UIAttrSystem[formId].mPreFormModeWhenOpen == GlobalNS.PreFormModeWhenOpen.eHIDE) then
        if(self.mCurFormIndex >= 0) then
            -- 将当前窗口 Id 保存
            self.mFormIdStack:push(self.mCurFormIndex);
            -- 隐藏当前窗口
            self:hideFormNoOpenPreForm(self.mCurFormIndex);
        end
    end
end

function M:getForm(formId)
    return self.mFormArr[formId];
end

function M:hasForm(formId)
    local has = false;
    for _, value in pairs(self.mFormArr) do
        if(value ~= nil) then
            has = true;
            break;
        end
    end
    
    return has;
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
	if(self.mFormArr[formId] ~= nil) then
		local parent = self:getLayerGo(GlobalNS.UIAttrSystem[self.mFormArr[formId].mId].mCanvasId, GlobalNS.UIAttrSystem[self.mFormArr[formId].mId].mLayerId);
        self.mFormArr[formId].mGuiWin = self.mFormId2LoadItemDic:value(formId):getSelfGo();
		GlobalNS.UtilApi.SetParent(self.mFormArr[formId].mGuiWin, parent, false);
        GlobalNS.UtilApi.SetActive(self.mFormArr[formId].mGuiWin, false);     -- 加载完成后先隐藏，否则后面 showForm 判断会有问题
        self.mFormArr[formId]:onReady();
		if(self.mFormArr[formId]:isVisible()) then
			GlobalNS.UtilApi.SetActive(self.mFormArr[formId].mGuiWin, true);
		end
	else
		self.mFormId2LoadItemDic:value(formId):dispose();
		self.mFormId2LoadItemDic:Remove(formId);
	end
end

return M;