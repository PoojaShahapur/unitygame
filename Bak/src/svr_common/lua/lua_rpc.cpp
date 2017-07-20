#include "lua_rpc.h"

#include "log.h"
#include "rpc/rpc_call_context.h"  // for CRpcCallContext
#include "rpc/rpc_helper.h"  // for RpcHelper
#include "to_function.h"  // for ToFunction<>

#include <LuaIntf/LuaIntf.h>

const char LOG_NAME[] = "LuaRpc";

namespace {

using Lua::ToFunction;

void ReplyTo(const CRpcCallContext& ctx, const std::string& sResponse)
{
	// Todo: check CRpcCallContext?
	RpcHelper::ReplyTo(ctx, sResponse);
}

void RequestClt(const LuaIntf::LuaRef& luaGameCltId,
	const std::string& sService, const std::string& sMethod,
	const std::string& sRequest, const LuaIntf::LuaRef& luaCallback)
{
	// Todo: Check parameters...
	// Todo: try/catch?
	CGameCltId gameCltId = luaGameCltId.toValue<CGameCltId>();
	RpcCallback cb = ToFunction<RpcCallback>(luaCallback);
	RpcHelper::Request(gameCltId, sService, sMethod, sRequest, cb);
}

// 跨服请求， uSvrId 为服务器ID
void RequestSvr(uint16_t uSvrId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const LuaIntf::LuaRef& luaCallback)
{
	RpcCallback cb = ToFunction<RpcCallback>(luaCallback);
	RpcHelper::RequestSvr(uSvrId, sService, sMethod, sRequest, cb);
}

}  // namespace

namespace LuaRpc {

void Bind(lua_State* L)
{
	assert(L);
	// 所有导出模块带前缀"c_"
	LuaIntf::LuaBinding(L).beginModule("c_rpc")
		.addFunction("reply_to", &ReplyTo)  // 应答，对方可能是服务器
		.addFunction("request_clt", &RequestClt)  // 请求游戏客户端
		.addFunction("request_svr", &RequestSvr)  // 请求服务器
	.endModule();
}

}  // namespace LuaRpc
