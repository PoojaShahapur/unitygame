#include "lua_rpc_call_context.h"

#include "rpc/rpc_call_context.h"  // for CRpcCallContext

#include <LuaIntf/LuaIntf.h>

const char LOG_NAME[] = "LuaRpcCallContext";

namespace {

// Copy CGameCltId
CGameCltId GetGameCltId(const CRpcCallContext* pCtx)
{
	assert(pCtx);
	return pCtx->GetGameCltId();
}

}  // namespace

namespace LuaRpcCallContext {

void Bind(lua_State* L)
{
	assert(L);
	// CRpcCallContext 仅在 C 中创建。
	LuaIntf::LuaBinding(L).beginModule("c_rpc_call_context")
		.beginClass<CRpcCallContext>("CRpcCallContext")
			.addFunction("get_rpc_clt_id", &CRpcCallContext::GetRpcCltId)
			.addFunction("get_game_clt_id", &GetGameCltId)
		.endClass()
	.endModule();
}

}  // namespace LuaRpcCallContext
