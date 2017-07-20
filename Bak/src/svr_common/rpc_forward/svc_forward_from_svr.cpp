#include "svc_forward_from_svr.h"

#include "app.h"  // for CApp
#include "asio/asio_server4c.h"  // for HandleForwardedRpcRequest()
#include "game_clt_id.h"  // for CGameCltId
#include "log.h"
#include "pb/svr/forward_from_svr.pb.h"  // for ForwardRequest
#include "rpc/rpc_call_context.h"  // for CRpcCallContext
#include "rpc/rpc_helper.h"  // for ParseMsgFromStr()

const char LOG_NAME[] = "CSvcForwardFromSvr";

CSvcForwardFromSvr::CSvcForwardFromSvr()
{
	RegisterMethod("Forward",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			Forward(ctx, sContent);
		});
}

void CSvcForwardFromSvr::Forward(const CRpcCallContext& ctx,
	const std::string& sContent)
{
	svr::ForwardRequest fwdReq;
	if (!RpcHelper::ParseMsgFromStr(sContent, fwdReq))
		return;
	LOG_DEBUG(Fmt("Forward, %s.%s") % fwdReq.service() % fwdReq.method());

	// 游戏客户端的RPC请求通过Base服转发到目的Cell服，
	// 再由目的Cell服上的Svr4C服务处理(不是Svr4S)，
	// 相当于游戏客户端直连目的Cell服并请求。
	const svr::ForwardRequest::GameCltId& gameCltId = fwdReq.game_clt_id();
	CRpcCallContext ctx2(ctx, CGameCltId(gameCltId.base_svr_id(),
		gameCltId.base_rpc_clt_id()));
	CAsioServer4C& rSvr4C = CApp::get_const_instance().GetSvr4C();
	rSvr4C.HandleForwardedRpcRequest(ctx2, fwdReq.service(), fwdReq.method(),
		fwdReq.content());
}
