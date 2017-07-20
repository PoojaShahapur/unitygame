#include "rpc_call_context.h"

#include "util.h"  // for GetMySvrId()

CRpcCallContext::CRpcCallContext(uint64_t uRpcCltId, uint32_t uRpcId)
	: CRpcCallContext(uRpcCltId, uRpcId, CGameCltId(
		Util::GetMySvrId(), uRpcCltId))
{
}

CRpcCallContext::CRpcCallContext(const CRpcCallContext& ctx,
	const CGameCltId& gameCltId)
	: CRpcCallContext(ctx.GetRpcCltId(), ctx.GetRpcId(), gameCltId)
{
}

CRpcCallContext::CRpcCallContext(uint64_t uRpcCltId, uint32_t uRpcId,
	const CGameCltId& gameCltId)
	: m_uRpcCltId(uRpcCltId), m_uRpcId(uRpcId),
	m_gameCltId(gameCltId)
{
}
