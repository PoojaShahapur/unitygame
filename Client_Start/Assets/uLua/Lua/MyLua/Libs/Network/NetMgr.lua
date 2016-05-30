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
        local data = GlobalNS.ProtobufUtil.decode(command.proto, buffer);
        if(data ~= nil) then
            GCtx.mLogSys:log("NetMgr handleMsg", GlobalNS.LogTypeId.eLogCommon);
            GCtx.m_netCmdNotify:handleMsg(data);
        end
    end
end

return M;
