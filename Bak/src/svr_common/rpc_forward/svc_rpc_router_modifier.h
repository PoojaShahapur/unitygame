#ifndef SVR_COMMON_RPC_FORWARD_SVC_RPC_ROUTER_MODIFIER_H
#define SVR_COMMON_RPC_FORWARD_SVC_RPC_ROUTER_MODIFIER_H

#include "rpc/rpc_service.h"  // for CRpcService

// rpc_router_modifier.proto
class CSvcRpcRouterModifier final : public CRpcService
{
public:
	CSvcRpcRouterModifier();

public:
	// 服务名，对应proto文件中的service, 带包名。
	std::string GetServiceName() const override { return "svr.RpcRouterModifier"; }

private:
	// 设置特定方法的路由
	void SetMthdDstSvrId(const CRpcCallContext& ctx, const std::string& sContent);
	void ResetMthdDstSvrId(const CRpcCallContext& ctx, const std::string& sContent);
	// 设置服务的路由
	void SetSvcDstSvrId(const CRpcCallContext& ctx, const std::string& sContent);
	void ResetSvcDstSvrId(const CRpcCallContext& ctx, const std::string& sContent);
};  // class CSvcRpcRouterModifier

#endif  // SVR_COMMON_RPC_FORWARD_SVC_RPC_ROUTER_MODIFIER_H
