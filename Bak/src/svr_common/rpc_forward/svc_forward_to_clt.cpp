#include "svc_forward_to_clt.h"

#include "pb/svr/forward_to_clt.pb.h"  // for ForwardToCltRequest
#include "log.h"
#include "rpc/rpc_call_context.h"  // for CRpcCallContext
#include "rpc/rpc_helper.h"  //for RpcHelper

const char LOG_NAME[] = "CSvcForwardToClt";

CSvcForwardToClt::CSvcForwardToClt()
{
	RegisterMethod("ForwardToClt",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			ForwardToClt(ctx, sContent);
		});
}

void CSvcForwardToClt::ForwardToClt(const CRpcCallContext& ctx,
	const std::string& sContent)
{
	svr::ForwardToCltRequest req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;

	RpcHelper::Request(req.base_rpc_clt_id(), req.service(), req.method(),
		req.content(), [ctx](const std::string& sContent) {
			svr::ForwardToCltResponse resp;
			resp.set_content(sContent);
			RpcHelper::ReplyTo(ctx, resp);
		});
}
