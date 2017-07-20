#include "remote_rpc_router_modifier.h"

#include "log.h"
#include "rpc/rpc_helper.h"  // for RequestSvr()
#include "rpc_router.h"  // for CRpcRouter
#include "pb/svr/rpc_router_modifier.pb.h"  // for SetMthdDstSvrIdRequest
#include "util.h"  // for IsMySvrId()

const char LOG_NAME[] = "CRemoteRpcRouterModifier";

static CRpcRouter& GetRouter()
{
	return CRpcRouter::get_mutable_instance();
}

CRemoteRpcRouterModifier::CRemoteRpcRouterModifier(uint16_t uRemoteSvrId)
	: m_uRemoteSvrId(uRemoteSvrId)
{
}

CRemoteRpcRouterModifier::~CRemoteRpcRouterModifier()
{
}

void CRemoteRpcRouterModifier::SetMthdDstSvrId(uint64_t uRpcCltId,
	const string& sService, const string& sMethod, uint16_t uSvrId,
	const Callback& cb)
{
	LOG_DEBUG("SetMthdDstSvrId");
	if (IsLocal())
	{
		GetRouter().SetMthdDstSvrId(uRpcCltId, sService, sMethod, uSvrId);
		if (cb) cb();
		return;
	}

	svr::SetMthdDstSvrIdRequest req;
	req.set_session_id(uRpcCltId);
	req.set_service(sService);
	req.set_method(sMethod);
	req.set_dst_svr_id(uSvrId);
	RequestSvr("SetMthdDstSvrId", req, cb);
}

void CRemoteRpcRouterModifier::ResetMthdDstSvrId(uint64_t uRpcCltId,
	const string& sService, const string& sMethod, const Callback& cb)
{
	LOG_DEBUG("ResetMthdDstSvrId");
	if (IsLocal())
	{
		GetRouter().ResetMthdDstSvrId(uRpcCltId, sService, sMethod);
		if (cb) cb();
		return;
	}

	svr::ResetMthdDstSvrIdRequest req;
	req.set_session_id(uRpcCltId);
	req.set_service(sService);
	req.set_method(sMethod);
	RequestSvr("ResetMthdDstSvrId", req, cb);
}

void CRemoteRpcRouterModifier::SetSvcDstSvrId(uint64_t uRpcCltId,
	const string& sService, uint16_t uSvrId, const Callback& cb)
{
	LOG_DEBUG("SetSvcDstSvrId");
	if (IsLocal())
	{
		GetRouter().SetSvcDstSvrId(uRpcCltId, sService, uSvrId);
		if (cb) cb();
		return;
	}

	svr::SetSvcDstSvrIdRequest req;
	req.set_session_id(uRpcCltId);
	req.set_service(sService);
	req.set_dst_svr_id(uSvrId);
	RequestSvr("SetSvcDstSvrId", req, cb);
}

void CRemoteRpcRouterModifier::ResetSvcDstSvrId(uint64_t uRpcCltId,
	const string& sService, const Callback& cb)
{
	LOG_DEBUG("ResetSvcDstSvrId");
	if (IsLocal())
	{
		GetRouter().ResetSvcDstSvrId(uRpcCltId, sService);
		if (cb) cb();
		return;
	}

	svr::ResetSvcDstSvrIdRequest req;
	req.set_session_id(uRpcCltId);
	req.set_service(sService);
	RequestSvr("ResetSvcDstSvrId", req, cb);
}

bool CRemoteRpcRouterModifier::IsLocal() const
{
	return 0 == m_uRemoteSvrId || Util::IsMySvrId(m_uRemoteSvrId);
}

void CRemoteRpcRouterModifier::RequestSvr(
	const std::string& sMethod,
	const google::protobuf::Message& req,
	const Callback& cb) const
{
	RpcHelper::RequestSvr(m_uRemoteSvrId,
		"svr.RpcRouterModifier", sMethod, req,
		[cb](const string&) { if (cb) cb(); });
}
