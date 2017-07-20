#ifndef SVR_COMMON_RPC_FORWARD_REMOTE_RPC_ROUTER_MODIFIER_HEAD
#define SVR_COMMON_RPC_FORWARD_REMOTE_RPC_ROUTER_MODIFIER_HEAD

#include "protobuf_fwd.h"  // for Message

#include <cstdint>  // for uint16_t
#include <string>
#include <functional>

// 更改远程服务器Rpc路由表。也支持本服。
class CRemoteRpcRouterModifier
{
public:
	explicit CRemoteRpcRouterModifier(uint16_t uRemoteSvrId);
	virtual ~CRemoteRpcRouterModifier();

public:
	using string = std::string;
	using Callback = std::function<void()>;

public:
	// 设置特定方法的路由，服务的路由不变.
	void SetMthdDstSvrId(uint64_t uRpcCltId, const string& sService,
		const string& sMethod, uint16_t uSvrId,
		const Callback& cb = Callback());
	void ResetMthdDstSvrId(uint64_t uRpcCltId, const string& sService,
		const string& sMethod, const Callback& cb = Callback());

	// 设置服务的路由，特定方法的路由不变。
	void SetSvcDstSvrId(uint64_t uRpcCltId,
		const string& sService, uint16_t uSvrId,
		const Callback& cb = Callback());
	void ResetSvcDstSvrId(uint64_t uRpcCltId, const string& sService,
		const Callback& cb = Callback());

private:
	bool IsLocal() const;
	void RequestSvr(
		const std::string& sMethod,
		const google::protobuf::Message& req,
		const Callback& cb) const;

private:
	uint16_t m_uRemoteSvrId;  // 待设置服务器ID, 可以为本服ID
};  // class CRemoteRpcRouterModifier

#endif  // SVR_COMMON_RPC_FORWARD_REMOTE_RPC_ROUTER_MODIFIER_HEAD
