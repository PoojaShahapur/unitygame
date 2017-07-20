#include "rpc_helper.h"

#include "app.h"  // for CApp
#include "asio/asio_server4c.h"  // for GetSessionIn()
#include "asio/asio_server4s.h"  // for GetSessionIn()
#include "asio/asio_session_in.h"  // for CAsioSessionIn
#include "asio/asio_session_in_mgr.h"  // for CAsioSessionInMgr
#include "asio/asio_session_out.h"  // for CAsioSessionOut
#include "cluster/cluster.h"  // for CCluster
#include "log.h"
#include "pb/svr/forward_from_svr.pb.h"  // for ForwardResponse
#include "pb/svr/forward_to_clt.pb.h"  // for ForwardToCltRequest
#include "rpc_call_context.h"  // for GetRpcCltId()

#include <google/protobuf/message.h>  // for Message

const char LOG_NAME[] = "RcpHelper";

namespace {

template <class T>
inline CAsioSessionOut* GetSessionOut(T)
{
	assert("Illegal server ID (must be uint16_t).");
	return nullptr;
}

template <>
inline CAsioSessionOut* GetSessionOut(uint16_t uRemoteSvrId)
{
	return CCluster::get_const_instance().GetS2sSession(uRemoteSvrId);
}

template <class T>
inline CAsioSessionIn* GetSessionIn(T)
{
	assert("Illegal client ID (must be uint64_t).");
	return nullptr;
}

template <>
inline CAsioSessionIn* GetSessionIn(uint64_t uCltRpcId)
{
	return CAsioSessionInMgr::get_const_instance().GetSessionIn(uCltRpcId);
}

uint16_t GetFunctionSvrId(const std::string& sFunction)
{
	return CCluster::get_const_instance().GetSvrId(sFunction);
}

}  // namespace

namespace RpcHelper
{

bool ParseMsgFromStr(const std::string& sData,
	google::protobuf::Message& rMsg)
{
	if (rMsg.ParseFromString(sData))
		return true;
	LOG_WARN(Fmt("Failed to parse message. len=%u") % sData.size());
	return false;
}

void ReplyTo(const CRpcCallContext& ctx,
	const google::protobuf::Message& resp)
{
	ReplyTo(ctx, resp.SerializeAsString());
}

void ReplyTo(const CRpcCallContext& ctx, const std::string& sResp)
{
	uint64_t uRpcCltId = ctx.GetRpcCltId();
	CAsioSessionIn* pSession = GetSessionIn(uRpcCltId);
	if (!pSession)
	{
		LOG_WARN(Fmt("Can not find session_%1% to reply.") % uRpcCltId);
		return;
	}

	if (!ctx.IsForwarded())
	{
        //std::string temp = sResp.substr(0, 50);
	    //LOG_DEBUG(Fmt("non-forward, push rcp response to client, rpcid=%lu,resp=%s") % uRpcCltId % temp.c_str());
		pSession->PushRpcResp(ctx.GetRpcId(), sResp);
		return;
	}

	svr::ForwardResponse fwdResp;
	fwdResp.set_content(sResp);
    //std::string temp = sResp.substr(0, 50);
	//LOG_WARN(Fmt("push rcp response to client, rpcid=%lu,resp=%s") % uRpcCltId % temp.c_str());
	pSession->PushRpcResp(ctx.GetRpcId(), fwdResp);
}

void Request(uint64_t uRpcCltId, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb)
{
	CAsioSessionIn* pSession = GetSessionIn(uRpcCltId);
	if (pSession)
		pSession->PushRpcReq(sService, sMethod, req, cb);
	else
		LOG_WARN(Fmt("Can not find session_%1% to request.") % uRpcCltId);
}

void Request(uint64_t uRpcCltId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb)
{
	CAsioSessionIn* pSession = GetSessionIn(uRpcCltId);
	//LOG_WARN(Fmt("req client method, rpcid=%lu,service=%s,method=%s") % uRpcCltId % sService.c_str() % sMethod.c_str());
	if (pSession)
		pSession->PushRpcReq(sService, sMethod, sRequest, cb);
	else
		LOG_WARN(Fmt("Can not find session_%1% to request.") % uRpcCltId);
}

void Request(const CGameCltId& gameCltId, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb/* = RpcCallback() */)
{
	Request(gameCltId, sService, sMethod, req.SerializeAsString(), cb);
}

void Request(const CGameCltId& gameCltId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb/* = RpcCallback() */)
{
	if (gameCltId.IsLocal())
	{
		Request(gameCltId.GetBaseRpcCltId(), sService, sMethod, sRequest, cb);
		return;
	}

	svr::ForwardToCltRequest req;
	req.set_base_rpc_clt_id(gameCltId.GetBaseRpcCltId());
	req.set_service(sService);
	req.set_method(sMethod);
	req.set_content(sRequest);
	RequestSvr(gameCltId.GetBaseSvrId(), "svr.ForwardToClt", "ForwardToClt", req,
		[cb](const std::string sContent) {
			svr::ForwardToCltResponse resp;
			if (!ParseMsgFromStr(sContent, resp)) return;
			if (cb) cb(resp.content());
		});
}

// Request between servers.
void RequestSvr(uint16_t uServerId, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb)
{
	CAsioSessionOut* pS2s = GetSessionOut(uServerId);
	if (pS2s)
		pS2s->PushRpcReq(sService, sMethod, req, cb);
	else
		LOG_WARN("Can not find s2s session to request. RemoteSvrId=" << uServerId);
}

void RequestSvr(uint16_t uServerId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb)
{
	CAsioSessionOut* pS2s = GetSessionOut(uServerId);
	if (pS2s)
		pS2s->PushRpcReq(sService, sMethod, sRequest, cb);
	else
		LOG_WARN("Can not find s2s session to request. RemoteSvrId=" << uServerId);
}

void RequestSvr(const std::string& sFunction, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb)
{
	uint16_t uSvrId = GetFunctionSvrId(sFunction);
	RequestSvr(uSvrId, sService, sMethod, req, cb);
}

void RequestSvr(const std::string& sFunction, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb)
{
	uint16_t uSvrId = GetFunctionSvrId(sFunction);
	RequestSvr(uSvrId, sService, sMethod, sRequest, cb);
}

}  // namespace RpcHelper
