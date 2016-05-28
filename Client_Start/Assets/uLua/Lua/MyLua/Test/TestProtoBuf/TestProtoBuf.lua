require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Libs.Network.CommandID"
require "MyLua.Libs.Network.NetCommand"
require "MyLua.Libs.ProtoBuf.ProtobufUtil"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestProtoBuf";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:dispose()
	
end

function M:run()
	self:testPB();
end

function M:testPB()
	local msg = {};
	msg.requid = 1000;
	msg.reqguid = 1000;
	msg.reqaccount = "aaaaa";
	
	local command = NetCommand[1000];
	if(command ~= nil) then
		local buffer = GlobalNS.ProtobufUtil.encode(command.proto, msg);
		local data = GlobalNS.ProtobufUtil.decode(command.proto, buffer);
	end
end

return M;