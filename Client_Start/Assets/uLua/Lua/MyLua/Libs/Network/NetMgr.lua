require "MyLua.Libs.Network.CommandID"
require "MyLua.Libs.Network.NetCommand"
require "MyLua.Libs.ProtoBuf.ProtobufUtil"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetMgr";
GlobalNS[M.clsName] = M;

function M:init()
    GlobalNS.ProtobufUtil.registerAll();
end

function M:sendCmd(id, data, isNetSend)
    if(isNetSend == nil or isNetSend == true) then
        local command = NetCommand[id];
        if(data == nil) then
            data = {};
        end
        if(command ~= nil) then
            GCtx.mLogSys:log("NetMgr::sendCmd id = " .. id .. " Proto: " .. command.proto, GlobalNS.LogTypeId.eLogCommon);
            local buffer = GlobalNS.ProtobufUtil.encode(command.proto, data);
            GlobalNS.CSSystem.sendFromLua(id, buffer);
        end
    end
end

-- RPC 包
function M:sendCmdRpc(id, rpc, data, isNetSend)
    if(isNetSend == nil or isNetSend == true) then
        local command = NetCommand[id];
        if(data == nil) then
            data = {};
        end
        if(command ~= nil) then
            GCtx.mLogSys:log("NetMgr::sendCmd id = " .. id .. " Proto: " .. command.proto, GlobalNS.LogTypeId.eLogCommon);
            local buffer = GlobalNS.ProtobufUtil.encode(command.proto, data);
			rpc.request.content = buffer;
			
			command = NetCommand[3];
			if(command ~= nil) then
				local rpcBuffer = GlobalNS.ProtobufUtil.encode(command.proto, rpc);
				GlobalNS.CSSystem.sendFromLuaRpc(rpcBuffer);
			end
        end
    end
end

function M:receiveCmd(id, buffer)
    GCtx.mLogSys:log("NetMgr::receiveCmd id = " .. id, GlobalNS.LogTypeId.eLogCommon);
    local command = NetCommand[id];
    if(command ~= nil) then
        local data = GlobalNS.ProtobufUtil.decode(command.proto, buffer, buffer.mLength);
        if(data ~= nil) then
            GCtx.mLogSys:log("NetMgr handleMsg", GlobalNS.LogTypeId.eLogCommon);
            GCtx.m_netCmdNotify:handleMsg(data);
        end
    end
end

-- RPC 包
function M:receiveCmdRpc(buffer, length)
    GCtx.mLogSys:log("NetMgr::receiveCmdRpc", GlobalNS.LogTypeId.eLogCommon);
    local command = NetCommand[3];
    if(command ~= nil) then
        local rpcData = GlobalNS.ProtobufUtil.decode(command.proto, buffer, length);
		--local rpcData = GlobalNS.ProtobufUtil.decode(command.proto, buffer);
        if(rpcData ~= nil and rpcData.response ~= nil) then
			GCtx.mLogSys:log("NetMgr::receiveCmdRpc id = " .. rpcData.response.id, GlobalNS.LogTypeId.eLogCommon);
			command = NetCommand[rpcData.response.id + 1];
			if(command ~= nil) then
				local msgBody = GlobalNS.ProtobufUtil.decode(command.proto, rpcData.response.content);
				rpcData.response.msgBody = msgBody;
				GCtx.mLogSys:log("NetMgr handleMsg", GlobalNS.LogTypeId.eLogCommon);
				GCtx.m_netCmdNotify:handleMsg(rpcData);
			end
        end
    end
end

return M;