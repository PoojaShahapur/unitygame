#ifndef SVR_COMMON_RPC_RPC_HELPER_H
#define SVR_COMMON_RPC_RPC_HELPER_H

#include "rpc_callback.h"  // for RpcCallback

#include "protobuf_fwd.h"  // for Message

#include <cstdint>
#include <string>

class CGameCltId;
class CRpcCallContext;

namespace RpcHelper
{

bool ParseMsgFromStr(const std::string& sData,
	google::protobuf::Message& rMsg);

void ReplyTo(const CRpcCallContext& ctx, const google::protobuf::Message& resp);
void ReplyTo(const CRpcCallContext& ctx, const std::string& sResp);

// 请求Rpc到游戏客户端，
// 对方不应该是个服务器，服务器内部应该用RequestSvr()
void Request(uint64_t uRpcCltId, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb = RpcCallback());
void Request(uint64_t uRpcCltId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb = RpcCallback());

// 通过Base转发请求到游戏客户端
void Request(const CGameCltId& gameCltId, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb = RpcCallback());
void Request(const CGameCltId& gameCltId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb = RpcCallback());

// Request between servers.
void RequestSvr(uint16_t uServerId, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb = RpcCallback());
void RequestSvr(uint16_t uServerId, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb = RpcCallback());
void RequestSvr(const std::string& sFunction, const std::string& sService,
	const std::string& sMethod, const google::protobuf::Message& req,
	const RpcCallback& cb = RpcCallback());
void RequestSvr(const std::string& sFunction, const std::string& sService,
	const std::string& sMethod, const std::string& sRequest,
	const RpcCallback& cb = RpcCallback());

}  // namespace RpcHelper

#endif  // SVR_COMMON_RPC_RPC_HELPER_H
