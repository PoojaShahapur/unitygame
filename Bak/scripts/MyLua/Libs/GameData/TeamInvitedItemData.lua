--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TeamInvitedItemData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
    self.mTimer:setFuncObject(self, self.onTick);

    self.uid = 0;
    self.account = "";

    self.show = false;--显示当前邀请
    self.msgbox = nil;
end

function M:onTick()
	local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
    if lefttime <= 0 then
        if self.show then--自动结束
            GCtx.mTeamData.mDoOneInviteMsg = false;
            if self.msgbox ~= nil then
                self.msgbox:exit();
                self.msgbox = nil;
            end
        end
        GCtx.mTeamData:RemoveInvitedItemByKey(self.uid);
        self:dispose();
    else
        if self.show then
            self:showInvitedMsg(lefttime);
        end
    end
end

--组队信息
function M:showInvitedMsg(lefttime)
    if nil ~= self.msgbox and self.msgbox.mIsReady then
        self.msgbox:setDesc("<color=#00FF01FF>" .. self.account .. "</color> 邀请你一起组队，是否加入？( <color=#00FF01FF>" .. lefttime .. " </color>)");
    end	
end

function M:setdata(uid, acc, totaltime)
    self.uid = uid;
    self.account = acc;
    self.mTimer:setTotalTime(totaltime);
    self.mTimer:reset();
    self.mTimer:Start();
end

function M:onOkHandle(dispObj)
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:EnterTeam(self.uid, GCtx.mPlayerData.mHeroData.mMyselfNickName);
	GCtx.mTeamData:clearInvitedlist();--同意邀请，清除其他邀请信息
    if self.msgbox ~= nil then
        self.msgbox:exit();
        self.msgbox = nil;
    end
    self:dispose();
    GCtx.mTeamData.mDoOneInviteMsg = false;
end

function M:onCancelHandle(dispObj)
    GCtx.mTeamData:RemoveInvitedItemByKey(self.uid);
    self.show = false;
    if self.msgbox ~= nil then
        self.msgbox:exit();
        self.msgbox = nil;
    end
    GCtx.mTeamData.mDoOneInviteMsg = false;
end

function M:notifyCanShow(show)
    self.show = show;
    
    if self.show then
        self.msgbox = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIConfirmAgain);
        if nil == self.msgbox then
            self.msgbox = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConfirmAgain);
        end

        if nil ~= self.msgbox then
            local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
            self.msgbox:setDesc("<color=#00FF01FF>" .. self.account .. "</color> 邀请你一起组队，是否加入？( <color=#00FF01FF>" .. lefttime .. " </color>)");
            self.msgbox:removeOkEventHandle();--清掉上一个事件
            self.msgbox:removeCancelEventHandle();

            self.msgbox:addOkEventHandle(self, self.onOkHandle);
            self.msgbox:addCancelEventHandle(self, self.onCancelHandle);
        end
    else

    end
end

function M:dtor()
    self:dispose();
end

function M:dispose()
    if nil ~= self.mTimer then
        self.mTimer:Stop();
        GlobalNS.delete(self.mTimer);
    	self.mTimer = nil;
    end

    self.show = false;
    self.uid = 0;
    self.account = "";
    self = nil;
end

return M;
--endregion
