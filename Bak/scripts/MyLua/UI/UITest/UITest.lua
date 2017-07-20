MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UITest.TestNS");
MLoader("MyLua.UI.UITest.TestData");
MLoader("MyLua.UI.UITest.TestCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UITest";
GlobalNS.TestNS[M.clsName] = M;

function M:ctor()
    --print("M:ctor()");
    --print(tostring(self));
	self.mId = GlobalNS.UIFormId.eUITest;
	self.mData = GlobalNS.new(GlobalNS.TestNS.TestData);

    self.allcount = 50;
    self.datas={};
    for i=1, 20 do
	    local it ={};
	    it.name="name "..i;
	    it.title ="title"..i;
        it.index = i;
        it.m_AvatarBtn = nil;
        it.m_AvatarImage = nil;
	    table.insert(self.datas,it);
    end

    self.mSpriteId = 1;
    self.mSpriteList = {};
end

function M:dtor()
	local count = #self.mSpriteList;
    for i=1, count do
        local item = self.mSpriteList[i];
        if nil ~= item then
            item:dispose();
            item = nil;
        end
    end
    self.mSpriteList = nil;
end

function M:onInit()
    M.super.onInit(self);
    --print("M:onInit()");
	
	self.mTestBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTestBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    -- TODO: 之前使用的是 self.super ，结果如果子类中没有实现这个函数，然后这个函数又被调用了，结果直接调用父类的，而父类中有会再次调用 self 的 super 的函数，结果就死循环了，不断的调用自己
    -- self.super.onReady(self)
    M.super.onReady(self);
    --print("M:onReady()");
    --GlobalNS.CSImportToLua.UtilApi.addEventHandle(self.gameObject, self.onBtnClk);
	--SDK.Lib.UtilApi.addEventHandle(self.gameObject, "Button", self.onBtnClk);
	--GlobalNS.UtilApi.addEventHandleByPath(self.mGuiWin, GlobalNS.TestNS.TestPath.BtnTest, self, self.onBtnClk);
	self.mTestBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.TestNS.TestPath.BtnTest));
    self:on_rootscroll_loaded();
        
    local FormationPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "FormationPanel");
    for i=1, 5 do
        local itembtn = GlobalNS.new(GlobalNS.AuxButton);
        itembtn:addEventHandle(self, self.onBtnFClk);
        itembtn.param1 = i;
        itembtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(FormationPanel, "BtnF" .. i));
        itembtn.mImage:setSelfGo(itembtn:getSelfGo());
        local skininfo = {GlobalNS.UtilLogic.getAtlasAndImageBySkinBaseId(self.mSpriteId)};
        itembtn.mImage:setSpritePath(skininfo[1], skininfo[2]);
        table.insert(self.mSpriteList, itembtn);
    end
end

function M:onShow()
    M.super.onShow(self);
    --print("M:onShow()");
    
    --local aaa = 0;
    --aaa.print();
    
    --error("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
end

function M:onHide()
    M.super.onHide(self);
    --print("M:onHide()");
end

function M:onExit()
    M.super.onExit(self);
    --print("M:onExit()");
end

function M:onBtnClk()
	--self:testSendMsg();
	--self:testLoginMsg();
	--self:testEmptyLoginMsg();
	--self:testEnumLoginMsg();
    self.scroll_rect_table:ScrollTo(0);
end

function M:testSendMsg()
	local msg = {};
	msg.requid = 1000;
	msg.reqguid = 1000;
	msg.reqaccount = "aaaaa";
	GlobalNS.UtilMsg.sendMsg(1000, msg);
end

function M:testLoginMsg()
	local rpc = {};
	rpc.request = {};
	rpc.request.id = 1002;
	rpc.request.service = "rpc.Login";
	rpc.request.method = "Login";
	
    local msg = {};
    msg.account = "account";
    msg.password = "password";
    GlobalNS.UtilMsg.sendMsgRpc(1002, rpc, msg);
end

function M:testEmptyLoginMsg()
	local rpc = {};
	rpc.request = {};
	rpc.request.id = 1002;
	rpc.request.service = "rpc.Login";
	rpc.request.method = "Login";
	
    local msg = {};
    GlobalNS.UtilMsg.sendMsgRpc(1002, rpc, msg);
end

function M:testEnumLoginMsg()
	local rpc = {};
	rpc.request = {};
	rpc.request.id = 1002;
	rpc.request.service = "rpc.Login";
	rpc.request.method = "Login";
	
    local msg = {};
    msg.result = "ERR_SERVER_FULL";
	--msg.result = 2;
    msg.error_str = "password";
    GlobalNS.UtilMsg.sendMsgRpc(1003, rpc, msg);
end

function M:on_rootscroll_loaded()

    --获取ScrollRect的GameObject对象
    self.mScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Scroll View");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mScrollRect, "Viewport");
    --获取ScrollRect下Content中的RectTransform组件
    self.scroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");

    self.scroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            scroll_rect_item.gameObject:SetActive(true);
            local name = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "name", "Text");
            name.text = self.datas[index].name;
            local title = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "title", "Text");
            title.text = self.datas[index].title;

            self.datas[index].m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
            self.datas[index].m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
            self.datas[index].m_AvatarBtn.param1 = index;
            self.datas[index].m_AvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Avatar"));
            self.datas[index].m_AvatarBtn:setIsDestroySelf(false);

            self.datas[index].m_AvatarImage = GlobalNS.new(GlobalNS.AuxImage);
            self.datas[index].m_AvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Avatar"));
            self.datas[index].m_AvatarImage:setIsDestroySelf(false);
            if 0 == index % 2 then
                self.datas[index].m_AvatarImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
            else
                self.datas[index].m_AvatarImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
            end
            scroll_rect_item.name = self.datas[index].name;
        end
     
     self.scroll_rect_table.onItemDispear = 
         function(index)
		  	--[[
             if nil ~= self.datas[index].m_AvatarBtn then
                 self.datas[index].m_AvatarBtn:removeEventHandle(self, self.onAvatarBtnClk);
             end
		  	]]
         
             if nil ~= self.datas[index].m_AvatarBtn then
                 self.datas[index].m_AvatarBtn:clearEventHandle();
             end
         end

     self.scroll_rect_table.onShowNewPage = 
         function(index)
             Debugger.Log("page " .. index);
         end

     self.scroll_rect_table.onAllItemRenderEnd = 
         function(beginindex, index)
             Debugger.Log("end " .. beginindex .. " " .. index);
             if self.allcount <= #self.datas then
                return;
             end

             for i=21, 50 do
	             local it ={};
	             it.name="name "..i;
	             it.title ="title"..i;
                 it.index = i;
                 it.m_AvatarBtn = nil;
                 it.m_AvatarImage = nil;
	             table.insert(self.datas,it);
             end

             self.scroll_rect_table.recordCount= 50;
             self.scroll_rect_table:update();
             self.scroll_rect_table:ScrollTo(beginindex);
             self.scroll_rect_table:Refresh(beginindex, -1);
         end
     --self.scroll_rect_table.data= self.datas;
     --self.scroll_rect_table:ScrollTo(0);
     self.scroll_rect_table.recordCount= 20;
     self.scroll_rect_table:init();
     self.scroll_rect_table:Refresh(-1,-1);
end

--查看
function M:onAvatarBtnClk(dispObj)
    local index = dispObj.param1;
    self.mSpriteId = index;
end

function M:onBtnFClk(dispObj)
    local index = dispObj.param1;
    local skininfo = {GlobalNS.UtilLogic.getAtlasAndImageBySkinBaseId(self.mSpriteId)};
    self.mSpriteList[index].mImage:setSpritePath(skininfo[1], skininfo[2]);
end

return M;

