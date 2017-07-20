#ifndef CELL_CLUSTER_SVC_CLUSTER_H
#define CELL_CLUSTER_SVC_CLUSTER_H

#include "rpc/rpc_service.h"  // for CRpcService

// cluster.proto
class CSvcCluster : public CRpcService
{
public:
	CSvcCluster();
	virtual ~CSvcCluster();

public:
	// 服务名，对应proto文件中的service, 带包名。
	std::string GetServiceName() const override { return "svr.Cluster"; }

private:
	void UpdateCluster(const CRpcCallContext& ctx, const std::string& sContent);
	void AddCell(const CRpcCallContext& ctx, const std::string& sContent);
	void Ping(const CRpcCallContext& ctx, const std::string& sContent);
};  // CSvcCluster

#endif  // CELL_CLUSTER_SVC_CLUSTER_H
