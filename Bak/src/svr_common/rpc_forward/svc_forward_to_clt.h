#ifndef BASE_SERVICE_SVC_FORWARD_TO_CLT_H
#define BASE_SERVICE_SVC_FORWARD_TO_CLT_H

#include "rpc/rpc_service.h"  // for CRpcService

// forward_to_clt.proto
class CSvcForwardToClt final : public CRpcService
{
public:
	CSvcForwardToClt();

public:
	// 服务名，对应proto文件中的service, 带包名。
	std::string GetServiceName() const override { return "svr.ForwardToClt"; }

private:
	void ForwardToClt(const CRpcCallContext& ctx, const std::string& sContent);
};  // class CSvcForwardToClt

#endif  // BASE_SERVICE_SVC_FORWARD_TO_CLT_H
