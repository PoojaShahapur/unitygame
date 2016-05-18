require "MyLua.Libs.Network.CommandID"
require "MyLua.Libs.Network.NetCommand"
require "MyLua.Libs.Network.ProtobufUtil"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetMgr";
GlobalNS[M.clsName] = M;

function M:init()
    GlobalNS.ProtobufUtil.registerAll();
end

function M:sendCmd(id, data, isNetSend)
    if(isNetSend == true) then
        local command = NetCommand[id];
        if(data == nil) then
            data = {};
        end
        if(command ~= nil) then
            GCtx.mLogSys:log("Send message id: " .. id .. " Proto: " .. command.proto);
            local buffer = ProtobufUtil:encode(command.proto, data);
            GlobalNS.CSSystem.Ctx.m_luaSystem.sendFromLua(id, buffer);
        end
    end
end

function M:receiveCmd(id, buffer)
    GCtx.mLogSys:log("---------------- NetManager.receiveCmd id: ", id);
    local msg = NetMessage[id];
    if(msg ~= nil) then
        local data = ProtobufUtil:decode(msg.proto, buffer);
        if(data ~= nil) then
            GCtx.m_netDispList:handleMsg(data);
        end
    end
end

return M;
