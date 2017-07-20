#include "svc_cluster.h"

#include "log.h"
#include "pb/svr/cluster.pb.h"  // for ClusterInfo
#include "rpc/rpc_helper.h"  // for ParseMsgFromStr()
#include "cluster/cluster.h"  // for GetConstInfo()

const char LOG_NAME[] = "CSvcCluster";

CSvcCluster::CSvcCluster()
{
	RegisterMethod("UpdateCluster",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			UpdateCluster(ctx, sContent);
		});
	RegisterMethod("AddCell",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			AddCell(ctx, sContent);
		});
	RegisterMethod("Ping",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			Ping(ctx, sContent);
		});
}

CSvcCluster::~CSvcCluster()
{
}

void CSvcCluster::UpdateCluster(
	const CRpcCallContext& ctx, const std::string& sContent)
{
	LOG_DEBUG("UpdateCluster");
	svr::ClusterInfo req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;

	CCluster& rCluster = CCluster::get_mutable_instance();
	RpcHelper::ReplyTo(ctx, rCluster.GetConstInfo());
	rCluster.CheckClusterInfo(req);
}

void CSvcCluster::AddCell(const CRpcCallContext& ctx,
	const std::string& sContent)
{
	LOG_DEBUG("AddCell");
	svr::CellInfo req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;
	RpcHelper::ReplyTo(ctx, svr::EmptyMsg());
	CCluster::get_mutable_instance().CheckCellInfo(req);
}

void CSvcCluster::Ping(const CRpcCallContext& ctx,
	const std::string& /*sContent*/)
{
	LOG_DEBUG("Ping.");
	RpcHelper::ReplyTo(ctx, svr::EmptyMsg());
}

