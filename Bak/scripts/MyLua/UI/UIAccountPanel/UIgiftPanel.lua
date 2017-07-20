MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelData");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIgiftPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIgiftPanel;
	self.mData = GlobalNS.new(GlobalNS.AccountPanelNS.AccountPanelData);

    self.showlevellist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.showlevellist:setIsSpeedUpFind(true);
	self.showlevellist:setIsOpKeepSort(true);
end

function M:dtor()
	self.showlevellist:clear();
end

function M:onInit()
    M.super.onInit(self);
    
	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

    local count = #LuaExcelManager.level_level;

    for i = 1, count do
        local item = LuaExcelManager.level_level[i];
        if (not self.showlevellist:ContainsKey(item.type)) then
	    	self.showlevellist:add(item.type, item);
	    end
    end
end

function M:onReady()
    M.super.onReady(self);
   
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Bg");
    self.Gift_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Gift_Image");
    self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Gift_Image, "Esc_Btn"));

    self:on_scrollview_loaded();
    self:updateUIData();--默认今日榜
end

function M:onShow()
    M.super.onShow(self);
end

function M:on_scrollview_loaded()
    --获取ScrollRect的GameObject对象
    self.mScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Gift_Image, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mScrollRect, "Viewport");
    --获取ScrollRect下Content中的ScrollRectTable组件
    self.scroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");
    self.pagesize = self.scroll_rect_table.pageSize;

    self.scroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            if GlobalNS.UtilApi.IsUObjNil(scroll_rect_item) then
                return;
            end

            local i = index - 1;

            scroll_rect_item.gameObject:SetActive(true);
            scroll_rect_item.name = "RankItem" .. index;

            local level = self.showlevellist:value(GCtx.mGameData.levellist:get(i).m_leveltype).id;
            
            --段位
            local Dan = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Image");
            GCtx.mGameData.levellist:get(i).m_LevelImage = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levellist:get(i).m_LevelImage:setSelfGo(Dan);
	    	GCtx.mGameData.levellist:get(i).m_LevelImage:setSpritePath("Atlas/DefaultSkin/Level.asset", LuaExcelManager.level_level[level].image);

            local danName = GlobalNS.UtilApi.getComByPath(Dan, "Dan", "Text");
            danName.text = LuaExcelManager.level_level[level].name;
            --自己的段位
            local NowDan = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan, "NowDan");
            if LuaExcelManager.level_level[level].type == LuaExcelManager.level_level[GCtx.mPlayerData.mHeroData.mMyselfLevel].type then
                NowDan:SetActive(true);
                GlobalNS.UtilApi.setImageColor(scroll_rect_item.gameObject, 1, 1, 1, 0.7);
            else
                NowDan:SetActive(false);
                GlobalNS.UtilApi.setImageColor(scroll_rect_item.gameObject, 1, 1, 1, 1);
            end

            local Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan, "Image");
            if GCtx.mGameData.levellist:get(i).m_leveltype >= 16 then
                Image:SetActive(true);
                local startext = GlobalNS.UtilApi.getComByPath(Image, "Text", "Text");
                startext.text = LuaExcelManager.level_level[level].star;
            else
                Image:SetActive(false);
            end

            --奖励
            local Gift1_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Gift1_Image");
            GCtx.mGameData.levellist:get(i).m_Gift1Image = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levellist:get(i).m_Gift1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Gift1_Image, "gift_Image"));
            local Gift2_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Gift2_Image");
            GCtx.mGameData.levellist:get(i).m_Gift2Image = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levellist:get(i).m_Gift2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Gift2_Image, "gift_Image"));
            local Gift3_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Gift3_Image");
            GCtx.mGameData.levellist:get(i).m_Gift3Image = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levellist:get(i).m_Gift3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Gift3_Image, "gift_Image"));
            local Gift4_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Gift4_Image");
            GCtx.mGameData.levellist:get(i).m_Gift4Image = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levellist:get(i).m_Gift4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Gift4_Image, "gift_Image"));

            Gift1_Image:SetActive(true);
            Gift2_Image:SetActive(true);
            Gift3_Image:SetActive(true);
            Gift4_Image:SetActive(true);

            local giftnum = GCtx.mGameData.levellist:get(i).m_gift.Count; --奖励个数
            if 1 == giftnum then
                local giftid = GCtx.mGameData.levellist:get(i).m_gift[0].baseid;
                local objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift1Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_1_Num = GlobalNS.UtilApi.getComByPath(Gift1_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[0].num;
                Award_1_Num.text = text;

                Gift2_Image:SetActive(false);
                Gift3_Image:SetActive(false);
                Gift4_Image:SetActive(false);
            elseif 2 == giftnum then
                local giftid = GCtx.mGameData.levellist:get(i).m_gift[0].baseid;
                local objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift1Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_1_Num = GlobalNS.UtilApi.getComByPath(Gift1_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[0].num;
                Award_1_Num.text = text;

                giftid = GCtx.mGameData.levellist:get(i).m_gift[1].baseid;
                objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift2Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_2_Num = GlobalNS.UtilApi.getComByPath(Gift2_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[1].num;
                Award_2_Num.text = text;

                Gift3_Image:SetActive(false);
                Gift4_Image:SetActive(false);
            elseif 3 == giftnum then
                local giftid = GCtx.mGameData.levellist:get(i).m_gift[0].baseid;
                local objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift1Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_1_Num = GlobalNS.UtilApi.getComByPath(Gift1_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[0].num;
                Award_1_Num.text = text;

                giftid = GCtx.mGameData.levellist:get(i).m_gift[1].baseid;
                objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift2Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_2_Num = GlobalNS.UtilApi.getComByPath(Gift2_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[1].num;
                Award_2_Num.text = text;

                giftid = GCtx.mGameData.levellist:get(i).m_gift[2].baseid;
                objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift3Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_3_Num = GlobalNS.UtilApi.getComByPath(Gift3_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[2].num;
                Award_3_Num.text = text;

                Gift4_Image:SetActive(false);
            elseif 4 == giftnum then
                local giftid = GCtx.mGameData.levellist:get(i).m_gift[0].baseid;
                local objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift1Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_1_Num = GlobalNS.UtilApi.getComByPath(Gift1_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[0].num;
                Award_1_Num.text = text;

                giftid = GCtx.mGameData.levellist:get(i).m_gift[1].baseid;
                objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift2Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_2_Num = GlobalNS.UtilApi.getComByPath(Gift2_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[1].num;
                Award_2_Num.text = text;

                giftid = GCtx.mGameData.levellist:get(i).m_gift[2].baseid;
                objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift3Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_3_Num = GlobalNS.UtilApi.getComByPath(Gift3_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[2].num;
                Award_3_Num.text = text;

                giftid = GCtx.mGameData.levellist:get(i).m_gift[3].baseid;
                objinfo = {GlobalNS.UtilLogic.getAtlasAndImageByObjid(giftid)};
                GCtx.mGameData.levellist:get(i).m_Gift4Image:setSpritePath(objinfo[1], objinfo[2]);
                local Award_4_Num = GlobalNS.UtilApi.getComByPath(Gift4_Image, "gift_num", "Text");
                local text = "" .. GCtx.mGameData.levellist:get(i).m_gift[3].num;
                Award_4_Num.text = text;
            else

            end            
        end
end

function M:updateUIData()
    if GCtx.mGameData.levellistCount > 0 then
        self.scroll_rect_table.recordCount= GCtx.mGameData.levellistCount;
        self.scroll_rect_table:init();
        self.scroll_rect_table:Refresh(-1, -1);
    end
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    self.mCloseBtn:dispose();
    M.super.onExit(self);
end

function M:onCloseBtnClk()
    self:exit();
end

return M;