NetManager = {};
local this = NetManager; 

function NetManager.postCommand(id, data)
    if(not SINGLE_MODE) then
        local command = NetCommand[id];
        if(data == nil) then
            data = {};
        end
        if(command ~= nil) then
            print("Send message id: " .. id .. " Proto: " .. command.proto);
            local buffer = ProtobufUtil:encode(command.proto, data);
            Ctx.m_luaSystem.SendFromLua(id, buffer);
        end
    end
end

function NetManager.receiveMsg(id, buffer)
    print("---------------- NetManager.receiveMsg id: ", id);
    if(not SINGLE_MODE) then
        local msg = NetMessage[id];
        if(msg ~= nil) then
            local data = ProtobufUtil:decode(msg.proto, buffer);
        end
    end
end