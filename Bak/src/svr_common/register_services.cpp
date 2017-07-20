#include "register_services.h"

#include "asio/asio_server4c.h"  // for CAsioServer4C
#include "asio/asio_server4s.h"  // for CAsioServer4S
#include "cluster/svc_cluster.h"  // for CSvcCluster
#include "rpc_forward/svc_forward_from_svr.h"  // for CSvcForwardFromSvr
#include "rpc_forward/svc_forward_to_clt.h"  // for CSvcForwardToClt
#include "rpc_forward/svc_rpc_router_modifier.h"  // for CSvcRpcRouterModifier

#include <memory>  // for make_shared<>()

void RegisterServices4Clt(CAsioServer4C& rSvr4C)
{
	// 暂时无服务
}

void RegisterServices4Svr(CAsioServer4S& rSvr4S)
{
	rSvr4S.RegisterService(std::make_shared<CSvcForwardToClt>());
	rSvr4S.RegisterService(std::make_shared<CSvcForwardFromSvr>());
	rSvr4S.RegisterService(std::make_shared<CSvcCluster>());
	rSvr4S.RegisterService(std::make_shared<CSvcRpcRouterModifier>());
}
