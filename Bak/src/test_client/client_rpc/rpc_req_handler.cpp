#include "rpc_req_handler.h"

#include "log.h"
#include "pb/rpc/rpc.pb.h"  // for RpcRequest

#include <LuaIntf/LuaIntf.h>  // for LuaRef

const char LOG_NAME[] = "CRpcReqHandler";

namespace ClientRpc {

CRpcReqHandler::CRpcReqHandler(lua_State* L)
	: m_luaState(L)
{
}

CRpcReqHandler::~CRpcReqHandler()
{
}

void CRpcReqHandler::HandleRpcRequest(
	const rpc::RpcRequest& req)
{
	HandleRpcRequestInLua(req.id(),
		req.service(), req.method(), req.content());
}

void CRpcReqHandler::HandleRpcRequestInLua(uint32_t uRpcId,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent)
{
	using LuaIntf::LuaRef;
	LuaRef require(m_luaState, "require");
	try
	{
		LuaRef handler = require.call<LuaRef>("rpc_request_handler");
		handler.dispatchStatic("handle", uRpcId, sService, sMethod, sContent);
		// Todo: Register Lua service directly. No rpc_request_handler.
	}
	catch (const LuaIntf::LuaException& e)
	{
		LOG_ERROR("Failed to call lua rpc_request_handler.handle(), "
			<< e.what());
	}
}

}  // namespace ClientRpc
