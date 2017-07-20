#include "svc_rpc_router_modifier.h"

#include "pb/svr/rpc_router_modifier.pb.h"  // for SetSvcDstSvrIdRequest
#include "log.h"
#include "rpc/rpc_call_context.h"  // for CRpcCallContext
#include "rpc/rpc_helper.h"  //for RpcHelper
#include "rpc_router.h"  // for CRpcRouter

const char LOG_NAME[] = "CSvcRpcRouterModifier";

static CRpcRouter& GetRouter()
{
	return CRpcRouter::get_mutable_instance();
}

static void ReplyEmptyMsg(const CRpcCallContext& ctx)
{
	RpcHelper::ReplyTo(ctx, svr::EmptyMsg());
}

CSvcRpcRouterModifier::CSvcRpcRouterModifier()
{
	using std::bind;
	using namespace std::placeholders;
	RegisterMethod("SetMthdDstSvrId", bind(
		&CSvcRpcRouterModifier::SetMthdDstSvrId, this, _1, _2));
	RegisterMethod("ResetMthdDstSvrId", bind(
		&CSvcRpcRouterModifier::ResetMthdDstSvrId, this, _1, _2));
	RegisterMethod("SetSvcDstSvrId", bind(
		&CSvcRpcRouterModifier::SetSvcDstSvrId, this, _1, _2));
	RegisterMethod("ResetSvcDstSvrId", bind(
		&CSvcRpcRouterModifier::ResetSvcDstSvrId, this, _1, _2));
}

void CSvcRpcRouterModifier::SetMthdDstSvrId(
	const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_DEBUG("SetMthdDstSvrId");
	svr::SetMthdDstSvrIdRequest req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;
	GetRouter().SetMthdDstSvrId((req.session_id()),
		req.service(), req.method(), req.dst_svr_id());
	ReplyEmptyMsg(ctx);
}

void CSvcRpcRouterModifier::ResetMthdDstSvrId(
	const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_DEBUG("ResetMthdDstSvrId");
	svr::ResetMthdDstSvrIdRequest req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;
	GetRouter().ResetMthdDstSvrId((req.session_id()),
		req.service(), req.method());
	ReplyEmptyMsg(ctx);
}

void CSvcRpcRouterModifier::SetSvcDstSvrId(
	const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_DEBUG("SetSvcDstSvrId");
	svr::SetSvcDstSvrIdRequest req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;
	GetRouter().SetSvcDstSvrId((req.session_id()),
		req.service(), req.dst_svr_id());
	ReplyEmptyMsg(ctx);
}

void CSvcRpcRouterModifier::ResetSvcDstSvrId(
	const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_DEBUG("ResetSvcDstSvrId");
	svr::ResetSvcDstSvrIdRequest req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;
	GetRouter().ResetSvcDstSvrId((req.session_id()), req.service());
	ReplyEmptyMsg(ctx);
}
