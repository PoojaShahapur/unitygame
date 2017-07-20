#include "rpc_forwarder.h"

#include "log.h"
#include "pb/svr/forward_from_svr.pb.h"  // for ForwardResponse
#include "rpc/rpc_call_context.h"  // for CRpcCallContext
#include "rpc/rpc_helper.h"  // for RpcHelper

#include <cassert>

const char LOG_NAME[] = "RpcForwarder";

static void HandleForwardResponse(const CRpcCallContext& ctx,
	const std::string& sContent)
{
	svr::ForwardResponse fwdResp;
	if (RpcHelper::ParseMsgFromStr(sContent, fwdResp))
		RpcHelper::ReplyTo(ctx, fwdResp.content());
}

namespace RpcForwarder {

void ForwardTo(uint16_t uCellSvrId, const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent)
{
	assert(!ctx.IsForwarded());  // 不允许多次转发防止循环
	LOG_DEBUG(Fmt("ForwardTo cell_%1%: %2%.%3%")
		% uCellSvrId % sService % sMethod);

	svr::ForwardRequest fwdReq;
	fwdReq.set_service(sService);
	fwdReq.set_method(sMethod);
	fwdReq.set_content(sContent);
	svr::ForwardRequest::GameCltId* pGameCltId = fwdReq.mutable_game_clt_id();
	pGameCltId->set_base_svr_id(ctx.GetGameCltId().GetBaseSvrId());
	pGameCltId->set_base_rpc_clt_id(ctx.GetRpcCltId());

	RpcHelper::RequestSvr(uCellSvrId, "svr.ForwardFromBase", "Forward",
		fwdReq, [ctx](const std::string& sContent) {
			HandleForwardResponse(ctx, sContent);
		});
}

}  // namespace RpcForwarder
