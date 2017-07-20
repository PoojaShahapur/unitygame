#ifndef CELL_RPC_FORWARD_SVC_FORWARD_FROM_SVR_HEAD
#define CELL_RPC_FORWARD_SVC_FORWARD_FROM_SVR_HEAD

#include "rpc/rpc_service.h"  // for CRpcService

// forward_from_svr.proto
// Cell服务,处理转发的RPC.
// 注意只有游戏客户端的请求会被转发，服务器内部请求无转发。
// 该服务是服务器内部服务，但是最终获得的游戏客户端请求将由Svr4C来处理。
class CSvcForwardFromSvr final : public CRpcService
{
public:
	CSvcForwardFromSvr();

public:
	std::string GetServiceName() const override
	{
		return "svr.ForwardFromBase";
	}

private:
	void Forward(const CRpcCallContext& ctx, const std::string& sContent);
};  // class CSvcForwardFromSvr

#endif  // CELL_RPC_FORWARD_SVC_FORWARD_FROM_SVR_HEAD
