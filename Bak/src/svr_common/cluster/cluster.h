#ifndef CELL_CLUSTER_CLUSTER_H
#define CELL_CLUSTER_CLUSTER_H

#include "singleton.h"  // for Singleton

#include <memory>  // for unique_ptr<>
#include <string>

class CFunctionSvrSelector;
class CS2sSessionMgr;
class CAsioSessionOut;

namespace svr {
class ClusterInfo;
class CellInfo;
}  // namespace svr

// 每个Cell服各自维护集群信息。包括自身。
class CCluster : public Singleton<CCluster>
{
public:
	CCluster();
	virtual ~CCluster();

public:
	svr::ClusterInfo& GetMutableInfo() const { return *m_pInfo; }
	const svr::ClusterInfo& GetConstInfo() const { return *m_pInfo; }

public:
	void Init();

	void DeleteCell(uint16_t uSvrId);
	void OnS2sConnected(uint16_t uSvrId);

	// 查看收到的集群信息，有新的Cell就开始连接，待连接完成后再加入集群。
	// 可能是被动接收连接后收到信息，也可能是主动连接后应答信息。
	void CheckClusterInfo(const svr::ClusterInfo& clusterInfo);
	// 查看接收到的Cell信息，如果是新Cell就开始连接。
	void CheckCellInfo(const svr::CellInfo& cellInfo);

	// 功能服选择。
	uint16_t GetSvrId(const std::string& sFunction) const;
	uint16_t GetRandSvrId() const;
	CAsioSessionOut* GetS2sSession(uint16_t uRemoteSvrId) const;
	bool IsValidSvrId( uint16_t uSvrId) const;

private:
	void HandleUpdateClusterResp(const std::string& sResp,
		uint16_t uFromSvrId);
	void AddPeerCellInfo(uint16_t uCellId, const svr::ClusterInfo& clusterInfo);
	void BroadcastAddingCell(const svr::CellInfo& cellInfo) const;
	bool HasCellInfo(const svr::CellInfo& cellInfo) const;
	void InitMyCellInfo();
	std::string GetCellIdsStr() const;

private:
	using ClusterInfoSptr = std::unique_ptr<svr::ClusterInfo>;
	ClusterInfoSptr m_pInfo;
	std::unique_ptr<CS2sSessionMgr> m_pSessionMgr;
	std::unique_ptr<CFunctionSvrSelector> m_pSvrSelector;
};  // CCluster

#endif  // CELL_CLUSTER_CLUSTER_H
