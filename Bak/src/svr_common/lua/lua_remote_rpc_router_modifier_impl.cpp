#include "lua_remote_rpc_router_modifier_impl.h"

#include "rpc_forward/remote_rpc_router_modifier.h"  // for CRemoteRpcRouterModifier
#include "to_function.h"  // for ToFunction<>()

#include <LuaIntf/LuaIntf.h>

namespace LuaRemoteRpcRouterModifierImpl {

Modifier::Modifier(uint16_t uRemoteSvrId)
	: m_pRrrm(new CRemoteRpcRouterModifier(uRemoteSvrId))  // unique_ptr
{
}

void Modifier::SetMthdDstSvrId(uint32_t uSessnId, const string& sService,
	const string& sMethod, uint16_t uSvrId, const LuaRef& cb) const
{
	assert(m_pRrrm);
	m_pRrrm->SetMthdDstSvrId((uSessnId),
		sService, sMethod, uSvrId, ToCallback(cb));
}

void Modifier::ResetMthdDstSvrId(uint32_t uSessnId, const string& sService,
	const string& sMethod, const LuaRef& cb) const
{
	assert(m_pRrrm);
	m_pRrrm->ResetMthdDstSvrId((uSessnId),
		sService, sMethod, ToCallback(cb));
}

void Modifier::SetSvcDstSvrId(uint32_t uSessnId, const string& sService,
	uint16_t uSvrId, const LuaRef& cb) const
{
	assert(m_pRrrm);
	m_pRrrm->SetSvcDstSvrId((uSessnId),
		sService, uSvrId, ToCallback(cb));
}

void Modifier::ResetSvcDstSvrId(uint32_t uSessnId,
	const string& sService, const LuaRef& cb) const
{
	assert(m_pRrrm);
	m_pRrrm->ResetSvcDstSvrId((uSessnId),
		sService, ToCallback(cb));
}

Modifier::Callback Modifier::ToCallback(const LuaRef& cb) const
{
	return Lua::ToFunction<Callback>(cb);
}

}  // namespace LuaRemoteRpcRouterModifierImpl

