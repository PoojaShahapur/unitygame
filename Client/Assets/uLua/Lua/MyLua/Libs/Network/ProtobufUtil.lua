require "3rd/pbc/protobuf"

local M = {};
M.clsName = "ProtobufUtil";
GlobalNS[M.clsName] = M;
local this = M;

function M.registerAll()
    for i = 1, #PBFileList do
        this.registerPB(PBFileList[i]);
    end
end

function M.registerPB(file)
    local buffer = GlobalNS.CSSystem.MsgLocalStorage.readLuaBufferToFile(file);
    protobuf.register(buffer);
end

function M.encode(proto, buf)
    local buffer = protobuf.encode(proto, buf);
    return buffer;
end

function M.decode(proto, buf)
    local obj = protobuf.decode(proto, buf);
    return obj;
end

function M.PBC(enObj)
    local proto = enObj[1];
    if(proto ~= nil) then
        local deObj = protobuf.decode(proto, enObj[2]);
        local cData = {};
        if(type(deObj) == 'table') then
            for k, v in pairs(deObj) do
                cData[k] = v;
            end
            return cData;
        end
    end
    return enObj;
end

return M;