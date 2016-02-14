require "3rd/pbc/protobuf"

ProtobufUtil = 
{

};

function ProtobufUtil:registerAll()
    for i = 1, #PBFileList do
        self:registerPB(PBFileList[i]);
    end
end

function ProtobufUtil:registerPB(file)
    local buffer = MsgLocalStorage.readLuaBufferFile(file);
    protobuf.register(buffer);
end

function ProtobufUtil:encode(proto, buf)
    local obj = protobuf.encode(proto, buf);
    return obj;
end

function ProtobufUtil:decode(proto, buf)
    local buffer = protobuf.decode(proto, buf);
    return obj;
end

function PBC(enObj)
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