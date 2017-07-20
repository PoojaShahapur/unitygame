#ifndef SVR_COMMON_RPC_RPC_REQ_HANDLER_H
#define SVR_COMMON_RPC_RPC_REQ_HANDLER_H

#include "rpc/rpc_req_resp_fwd.h"  // for RpcRequest
#include "rpc/rpc_service_sptr.h"  // for RpcServiceSptr

#include <cstdint>
#include <unordered_map>

class CRpcCallContext;

class CRpcReqHandler final
{
public:
	CRpcReqHandler();
	~CRpcReqHandler();

public:
	void DisableForward() { m_bDisableForward = true; }

	// 注册服务。
	// sServiceName须包含包名，如"rpc.TestSvc".
	// pService允许为空，表示忽略该服务。
	void RegisterService(const std::string& sServiceName,
		RpcServiceSptr pService);
	void RegisterService(RpcServiceSptr pService);

public:
	void HandleRpcRequest(uint64_t uRpcCltId, const rpc::RpcRequest& req);
	void HandleRpcRequest(const CRpcCallContext& ctx,
		const std::string& sService, const std::string& sMethod,
		const std::string& sContent);
	void HandleRpcRequestInLua(const CRpcCallContext& ctx,
		const std::string& sService, const std::string& sMethod,
		const std::string& sContent);

private:
	// 转发成功则返回true, 无转发则返回false.
	bool TryToForwardRpcRequest(const CRpcCallContext& ctx,
		const std::string& sService, const std::string& sMethod,
		const std::string& sContent) const;

private:
	using ServiceMap = std::unordered_map<std::string, RpcServiceSptr>;
	ServiceMap m_mapService;
	bool m_bDisableForward = false;
};

#endif  // SVR_COMMON_RPC_RPC_REQ_HANDLER_H
