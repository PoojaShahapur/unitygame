--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "RankListItem";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.index = 0;
    self.account = "";
    self.m_FocusBtn = nil;
    self.m_AvatarBtn = nil;
end

function M:dtor()
    
end

function M:dispose()
    if GlobalNS.UtilApi.IsUObjNil(self.m_go) then
        GlobalNS.UtilApi.Destroy(self.m_go);
    end
    if self.m_FocusBtn ~= nil then
        self.m_FocusBtn:dispose();
    end
    if self.m_AvatarBtn ~= nil then
        self.m_AvatarBtn:dispose();
    end
end

function M:init(_Prefab,  _Content, _index)
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
    --self.m_go.transform.parent = _Content;
	GlobalNS.CSSystem.SetParent(self.m_go.transform, _Content);
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "RankItem" .. _index;
    self.index = _index;

    if self.index > 3 then
        local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Honer");
        GlobalNS.UtilApi.DestroyImmediate(Honer);
    else
        local Rank = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Rank");
        Rank:SetActive(false);
    end

    self.m_FocusBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.m_FocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Focus_Btn"));
    self.m_FocusBtn:addEventHandle(self, self.onFocusBtnClk);

    self.m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.m_AvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Avatar"));
    self.m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
end

--关注
function M:onFocusBtnClk()
	GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:Follow(self.account);
end

--查看
function M:onAvatarBtnClk()
    GCtx.mPlayerData.mHeroData.mViewAccount = self.account;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
end

return M;

--endregion
