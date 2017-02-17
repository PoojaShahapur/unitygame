--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AvatarItemData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.index = nil;
    self.avatarItemBtn = nil;
end

function M:dtor()
    self.m_go:dispose();
    self.avatarItemBtn:dispose();
end

function M:init(_Prefab, _Content, _index)
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
    self.m_go.transform.parent = _Content;
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "Avatar" .. _index;

    self.index = _index;

    self.avatarItemBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.avatarItemBtn:setSelfGo(self.m_go);

    self.avatarItemBtn.mImage:setSelfGo(self.m_go);
    self.avatarItemBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..self.index..".png", GlobalNS.UtilStr.tostring(self.index));
	--self.avatarItemBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(self.index));
    self.avatarItemBtn:addEventHandle(self, self.onChoiceBtnClk);
end

function M:onChoiceBtnClk()
	if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIAccountAvatarPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountAvatarPanel);
        if nil ~= form and form.mIsReady then
            form:SetAvatarItems(self.index);
        end
    end
end

return M;

--endregion
