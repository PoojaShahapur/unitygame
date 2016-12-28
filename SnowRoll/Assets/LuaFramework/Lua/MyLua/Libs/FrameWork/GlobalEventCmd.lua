MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.Test.TestMain");

if(MacroDef.UNIT_TEST) then
	MLoader("MyLua.Test.TestMain");
end

--[[
    处理 CS 到 Lua 的全局事件
]]
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "GlobalEventCmd";
GlobalNS[M.clsName] = M;

-- 接收消息
function M.onReceiveToLua(id, buffer)
    --GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLua", GlobalNS.LogTypeId.eLogCommon);
    GCtx.mNetMgr:receiveCmd(id, buffer);
end

function M.onReceiveToLuaRpc(buffer, length)
    --GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLuaRpc", GlobalNS.LogTypeId.eLogCommon);
    GCtx.mNetMgr:receiveCmdRpc(buffer, length);
end

-- 接收消息, KBE
function M.onReceiveToLua_KBE(msgName, param)
    --GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLua_KBE", GlobalNS.LogTypeId.eLogCommon);
	GCtx.mNetCmdNotify_KBE:handleMsg(msgName, param);
end

-- 场景加载完成
function M.onSceneLoaded()
	if(MacroDef.UNIT_TEST) then
		pTestMain = GlobalNS.new(GlobalNS.TestMain);
		pTestMain:run();
	end
end

-- 主角加载完成
function M.onPlayerMainLoaded()
    --加载场景上的UI组件，主角加载完成后再加载UI，否则UI拿不到主角数据
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIPlayerDataPanel);
    --GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIForwardForce);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRockerPanel);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOptionPanel);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUITopXRankPanel);
end

-- 帧循环
function M.onAdvance(delta)
	GCtx.mProcessSys:advance(delta);
end

function M.openForm(formId)
	GCtx.mUiMgr:loadAndShow(formId);
end

function M.exitForm(formId)
	GCtx.mUiMgr:exitForm(formId);
end

function M.requireFile(filePath)
	return MLoader(filePath);
end

-- 用户昵称
function M.getUserNickName()
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIStartGame) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIStartGame);
        if nil ~= form and form:isVisible() then            
            return form.username;
        end
    end
    return "海盗一号";
end

return M;