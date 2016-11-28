MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Core.ClassLoader");
MLoader("MyLua.Libs.DataStruct.MStack");
MLoader("MyLua.Libs.DataStruct.MDictionary");
MLoader("MyLua.Libs.UI.UICore.UIFormID");
MLoader("MyLua.Libs.UI.UICore.UICanvas");
MLoader("MyLua.Libs.AuxComponent.AuxLoader.AuxUIPrefabLoader");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UIMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_formArr = {};
    self.m_curFormIndex = -1;
    self.m_formIdStack = GlobalNS.new(GlobalNS.MStack);
	self.mFormId2LoadItemDic = GlobalNS.new(GlobalNS.MDictionary);
	
	self.mUniqueNumIdGen = GlobalNS.new(GlobalNS.UniqueNumIdGen, 1); 	-- FormId 唯一 Id 生成
end

function M:dtor()

end

function M:init()
	-- UIFormID 初始化构造操作
	GlobalNS.UIFormID.init();
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
        canvasId = GlobalNS.UICanvasID.eUIFirstCanvas;
    end
    if(layerId == nil) then
        layerId = GlobalNS.UILayerID.eUISecondLayer;
    end
    GlobalNS.UtilApi.assert(canvasId < self.m_canvasList:Count());
    return self.m_canvasList:at(0):getLayerGo(layerId);
end

function M:showForm(formId)
    -- 如果当前显示的不是需要显示的
	-- 保证没有在显示之前删除
	if(self.m_formArr[formId] ~= nil) then
		if(self.m_curFormIndex ~= formId) then
			local curFormIndex_ = self.m_curFormIndex;
			self:showFormNoClosePreForm(formId);
			self.m_curFormIndex = curFormIndex_;
			
			self:pushAndHideForm(formId);
			self.m_curFormIndex = formId;
		end
	end
end

function M:showFormNoClosePreForm(formId)
    if(self.m_formArr[formId] ~= nil) then
		if(not self.m_formArr[formId]:isVisible()) then
			self.m_formArr[formId]:onShow();
			if(self.m_formArr[formId]:isReady()) then
				GlobalNS.UtilApi.SetActive(self.m_formArr[formId].m_guiWin, true);
			end
		end
        self.m_curFormIndex = formId;
    end
    
    self.m_formIdStack:removeAllEqual(formId);
end

-- 仅仅加载 lua 脚本，不加载资源
function M:loadFormScript(formId, param)
    if(self.m_formArr[formId] == nil) then
        local codePath = GlobalNS.UIAttrSystem[formId].m_luaScriptPath;
        local formCls = GlobalNS.ClassLoader.loadClass(codePath);
        self.m_formArr[formId] = GlobalNS.new(formCls, param);
        self.m_formArr[formId]:onInit();
    end
end

-- 加载脚本并且加载资源
function M:loadForm(formId, param)
    if(self.m_formArr[formId] == nil) then
        self:loadFormScript(formId, param);
    end
    
    if(not self:hasLoadItem(formId)) then
		local uiPrefabLoader = GlobalNS.new(GlobalNS.AuxUIPrefabLoader);
		self.mFormId2LoadItemDic:Add(formId, uiPrefabLoader);
		uiPrefabLoader:setFormId(formId);
		uiPrefabLoader:asyncLoad(GlobalNS.UIAttrSystem[formId].m_widgetPath, self, self.onFormPrefabLoaded);
    end
end

function M:loadAndShow(formId, param)
    if(self.m_formArr[formId] == nil or not self:hasLoadItem(formId)) then
        self:loadForm(formId, param);
    end
	if(self.m_formArr[formId] ~= nil and not self.m_formArr[formId]:isHideOnCreate()) then
		self:showForm(formId);
	end
    return self.m_formArr[formId];
end

function M:hideForm(formId)
    local bFormVisible = false;
    local form = self.m_formArr[formId];
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
    local form = self.m_formArr[formId];
    if(form.m_guiWin ~= nil and GlobalNS.UtilApi.IsActive(form.m_guiWin)) then
        form:onHide();
        GlobalNS.UtilApi.SetActive(form.m_guiWin, false);
        self.m_curFormIndex = -1;
    end
end

function M:exitForm(formId)
    local bFormVisible = false;
    local form = self.m_formArr[formId];
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
    local form = self.m_formArr[formId];
    -- 关闭当前窗口
    if(form ~= nil) then
        form:onHide();
        form:onExit();
        GlobalNS.delete(form);
        self:unloadLoadItem(formId);
        self.m_formArr[formId] = nil;
        self.m_curFormIndex = -1;
    end
    
    self.m_formIdStack:removeAllEqual(formId);
end

-- 弹出并且显示界面
function M:popAndShowForm(formId)
    -- 显示之前隐藏的窗口
    if(GlobalNS.UIAttrSystem[formId].m_preFormModeWhenClose == GlobalNS.PreFormModeWhenClose.eSHOW) then
        local curFormIndex_ = self.m_formIdStack:pop();
        if(curFormIndex_ == nil) then
            self.m_curFormIndex = -1;
        else
            self:showFormNoClosePreForm(curFormIndex_);
        end
    end
end

function M:pushAndHideForm(formId)
    if(GlobalNS.UIAttrSystem[formId].m_preFormModeWhenOpen == GlobalNS.PreFormModeWhenOpen.eCLOSE) then
        if(self.m_curFormIndex >= 0) then
            self:exitFormNoOpenPreForm(self.m_curFormIndex);
        end
    elseif(GlobalNS.UIAttrSystem[formId].m_preFormModeWhenOpen == GlobalNS.PreFormModeWhenOpen.eHIDE) then
        if(self.m_curFormIndex >= 0) then
            -- 将当前窗口 Id 保存
            self.m_formIdStack:push(self.m_curFormindex);
            -- 隐藏当前窗口
            self:hideFormNoOpenPreForm(self.m_curFormIndex);
        end
    end
end

function M:getForm(formId)
    return self.m_formArr[formId];
end

function M:hasForm(formId)
    local has = false;
    for _, value in pairs(self.m_formArr) do
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
	if(self.m_formArr[formId] ~= nil) then
		local parent = self:getLayerGo(GlobalNS.UIAttrSystem[self.m_formArr[formId].m_id].m_canvasId, GlobalNS.UIAttrSystem[self.m_formArr[formId].m_id].m_layerId);
        self.m_formArr[formId].m_guiWin = self.mFormId2LoadItemDic:value(formId):getSelfGo();
		GlobalNS.UtilApi.SetParent(self.m_formArr[formId].m_guiWin, parent, false);
        GlobalNS.UtilApi.SetActive(self.m_formArr[formId].m_guiWin, false);     -- 加载完成后先隐藏，否则后面 showForm 判断会有问题
        self.m_formArr[formId]:onReady();
		if(self.m_formArr[formId]:isVisible()) then
			GlobalNS.UtilApi.SetActive(self.m_formArr[formId].m_guiWin, true);
		end
	else
		self.mFormId2LoadItemDic:value(formId):dispose();
		self.mFormId2LoadItemDic:Remove(formId);
	end
end

return M;