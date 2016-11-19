MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Network.NetCommand");
MLoader("MyLua.Libs.ProtoBuf.ProtobufUtil");

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
	--self:testLuaPBEncodeDecode();
	self:testLuaEncodeCSDecode();
end

function M:testLuaPBEncodeDecode()
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

function M:testLuaEncodeCSDecode()
	local msg = {};
	msg.requid = 1000;
	msg.reqguid = 1000;
	msg.reqaccount = "aaaaa";
	
	local command = NetCommand[1000];
	if(command ~= nil) then
		local buffer = GlobalNS.ProtobufUtil.encode(command.proto, msg);
		GlobalNS.CSSystemTest.onTestProtoBufBuffer(1000, buffer);
	end
end

return M;