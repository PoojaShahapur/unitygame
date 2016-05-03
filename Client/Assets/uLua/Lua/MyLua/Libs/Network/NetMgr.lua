require "MyLua.Libs.Network.CommandID"
require "MyLua.Libs.Network.NetCommand"
require "MyLua.Libs.Network.ProtobufUtil"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetMgr";
GlobalNS[M.clsName] = M;

function M:init()
    GlobalNS.ProtobufUtil.registerAll();
end

function M:postCommand(id, data)
    if(not SINGLE_MODE) then
        local command = NetCommand[id];
        if(data == nil) then
            data = {};
        end
        if(command ~= nil) then
            print("Send message id: " .. id .. " Proto: " .. command.proto);
            local buffer = ProtobufUtil:encode(command.proto, data);
            GlobalNS.CSSystem.Ctx.m_luaSystem.SendFromLua(id, buffer);
        end
    end
end

function M:receiveMsg(id, buffer)
    print("---------------- NetManager.receiveMsg id: ", id);
    if(not SINGLE_MODE) then
        local msg = NetMessage[id];
        if(msg ~= nil) then
            local data = ProtobufUtil:decode(msg.proto, buffer);
        end
    end
end

return M;
