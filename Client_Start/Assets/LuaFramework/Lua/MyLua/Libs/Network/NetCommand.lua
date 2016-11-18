RpcRequest = 1;
RpcResponse = 2;
RpcPackage = 3

MSG_ReqTest = 1000;
MSG_RetTest = 1001;
LoginRequest = 1002;
LoginResponse = 1003;

NetCommand = 
{
	[RpcRequest] = { proto = "rpc.RpcRequest", },
	[RpcResponse] = { proto = "rpc.RpcResponse", },
	[RpcPackage] = { proto = "rpc.RpcPackage", },
	
    [MSG_ReqTest] = { proto = "msg.MSG_ReqTest", },
    [MSG_RetTest] = { proto = "msg.MSG_RetTest", },
    [LoginRequest] = { proto = "rpc.LoginRequest", },
    [LoginResponse] = { proto = "rpc.LoginResponse", },
};