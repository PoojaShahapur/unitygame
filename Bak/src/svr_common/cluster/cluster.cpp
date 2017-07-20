#include "cluster.h"

#include "csv/csv.h"
#include "detail/function_svr_selector.h"  // for CFunctionSvrSelector
#include "detail/s2s_session_mgr.h"  // for HasSession()
#include "log.h"
#include "pb/svr/cluster.pb.h"  // for ClusterInfo
#include "rpc/rpc_helper.h"  // for RequestSvr()
#include "util.h"  // for GetMySvrId()

#include <ctime>  // for time()

const char LOG_NAME[] = "CCluster";

CCluster::CCluster()
	: m_pInfo(new svr::ClusterInfo),
	m_pSessionMgr(new CS2sSessionMgr),
	m_pSvrSelector(new CFunctionSvrSelector)
{
}

CCluster::~CCluster()
{
}

void CCluster::Init()
{
	InitMyCellInfo();
	m_pSessionMgr->InitFromConfig();
}

void CCluster::InitMyCellInfo()
{
	auto& rMap = *m_pInfo->mutable_cell_info_map();
	assert(rMap.empty());
	uint16_t uSvrId = Util::GetMySvrId();  // Must after CApp::Init().
	m_pSvrSelector->AddSvr(uSvrId);
	svr::CellInfo& rCell = rMap[uSvrId];
	rCell.set_cell_id(uSvrId);

	const auto& cfg = CsvCfg::GetTable("server.csv")
		.GetRecord("id", uSvrId);
	rCell.set_inner_host(cfg.GetString("inner_host"));
	rCell.set_inner_port(cfg.GetInt<uint16_t>("inner_port"));
	rCell.set_start_ts(time(nullptr));
}

void CCluster::DeleteCell(uint16_t uSvrId)
{
	LOG_INFO("Delete Cell_" << uSvrId);
	auto& rMap = *m_pInfo->mutable_cell_info_map();
	rMap.erase(uSvrId);
	m_pSvrSelector->EraseSvr(uSvrId);
}

void CCluster::OnS2sConnected(uint16_t uSvrId)
{
	RpcHelper::RequestSvr(uSvrId, "svr.Cluster", "UpdateCluster", *m_pInfo,
		[uSvrId](const std::string& sResp) {
			CCluster::get_mutable_instance().HandleUpdateClusterResp(sResp, uSvrId);
		});
}

void CCluster::HandleUpdateClusterResp(
	const std::string& sResp, uint16_t uFromSvrId)
{
	assert(uFromSvrId);
	LOG_DEBUG("HandleUpdateClusterResp from cell" << uFromSvrId);
	svr::ClusterInfo resp;
	if (!resp.ParseFromString(sResp))
	{
		LOG_ERROR("Illegal cluster info response from cell" << uFromSvrId);
		m_pSessionMgr->EraseSession(uFromSvrId);
		return;
	}

	// 加入对方的信息
	AddPeerCellInfo(uFromSvrId, resp);
	// 有新的Cell就开始连接，待连接完成后再加入集群。
	CheckClusterInfo(resp);
}

void CCluster::AddPeerCellInfo(uint16_t uCellId, const svr::ClusterInfo& clusterInfo)
{
	const auto& newMap = clusterInfo.cell_info_map();
	auto itr = newMap.find(uCellId);
	if (itr == newMap.end())
	{
		LOG_ERROR(Fmt("Can not find cell%1% info.") % uCellId);
		return;
	}
	const svr::CellInfo& cellInfo = (*itr).second;
	if (uCellId != cellInfo.cell_id())
	{
		LOG_ERROR("Illegal info of cell" << uCellId);
		return;
	}
	if (HasCellInfo(cellInfo))
		return;

	BroadcastAddingCell(cellInfo);
	(*m_pInfo->mutable_cell_info_map())[uCellId] = cellInfo;
	m_pSvrSelector->AddSvr(uCellId);
	LOG_DEBUG("Now cells: [" << GetCellIdsStr() << "]");
}

void CCluster::CheckClusterInfo(const svr::ClusterInfo& clusterInfo)
{
	const auto& newMap = clusterInfo.cell_info_map();
	for (const auto& ele : newMap)
	{
		uint32_t uSvrId = ele.first;
		if (0 == uSvrId) continue;  // Ignore illegal svrId 0.
		const svr::CellInfo& cellInfo = ele.second;
		if (uSvrId != cellInfo.cell_id())
		{
			LOG_ERROR(Fmt("Conflicted cell ID: %1% != %2%")
				% uSvrId % cellInfo.cell_id());
			continue;
		}
		CheckCellInfo(cellInfo);
	}
}

void CCluster::CheckCellInfo(const svr::CellInfo& cellInfo)
{
	uint16_t uCellId = cellInfo.cell_id();
	if (0 == uCellId)
	{
		LOG_WARN("Got cell ID 0 in CellInfo.");
		return;
	}
	const std::string& sHost = cellInfo.inner_host();
	uint16_t uPort = cellInfo.inner_port();
	// 如果uCellId相同，但是host:port不同，则需要删除旧Session, 重新建Session.
	if (m_pSessionMgr->HasSession(uCellId, sHost, uPort))
		return;  // connecting
	if (HasCellInfo(cellInfo))
		return;
	// Connect to new cell. 会自动停止旧Session并删除。
	m_pSessionMgr->RespawnS2sSession(uCellId, sHost, uPort);
}

uint16_t CCluster::GetSvrId(const std::string& sFunction) const
{
	assert(m_pSvrSelector);
	return m_pSvrSelector->GetSvrId(sFunction);
}

uint16_t CCluster::GetRandSvrId() const
{
	return m_pSvrSelector->GetRandSvrId();
}

CAsioSessionOut* CCluster::GetS2sSession(uint16_t uRemoteSvrId) const
{
	return m_pSessionMgr->GetSession(uRemoteSvrId);
}

void CCluster::BroadcastAddingCell(const svr::CellInfo& cellInfo) const
{
	const auto& mapCells = m_pInfo->cell_info_map();
	for (const auto& ele : mapCells)
	{
		uint16_t uSvrId = ele.first;
		assert(uSvrId == ele.second.cell_id());
		assert(uSvrId != cellInfo.cell_id());  // 尚未加入集群
		if (Util::IsMySvrId(uSvrId)) continue;  // 不发自身
		RpcHelper::RequestSvr(uSvrId, "svr.Cluster", "AddCell", cellInfo);
	}
}

static void CheckCellInfoEqual(
	const svr::CellInfo& ci1, const svr::CellInfo& ci2)
{
	const char LOG_NAME[] = "CCluster.CellInfoConflict";
	if (ci1.cell_id() != ci2.cell_id()) LOG_ERROR("Cell ID");
	if (ci1.inner_host() != ci2.inner_host()) LOG_ERROR("Inner host");
	if (ci1.inner_port() != ci2.inner_port()) LOG_ERROR("Inner port");
	if (ci1.start_ts() != ci2.start_ts()) LOG_ERROR("Start timestamp");
}

bool CCluster::HasCellInfo(const svr::CellInfo& cellInfo) const
{
	uint16_t uCellId = cellInfo.cell_id();
	const auto& mapCells = m_pInfo->cell_info_map();
	auto itr = mapCells.find(uCellId);
	if (itr == mapCells.end())
		return false;

	assert(uCellId == (*itr).second.cell_id());
	CheckCellInfoEqual(cellInfo, (*itr).second);
	return true;
}

std::string CCluster::GetCellIdsStr() const
{
	const auto& mapCells = m_pInfo->cell_info_map();
	std::vector<uint16_t> vIds;
	for (const auto& ele : mapCells)
		vIds.emplace_back(ele.first);
	std::sort(vIds.begin(), vIds.end());
	std::ostringstream oss;
	for (uint16_t id : vIds)
		oss << id << ",";
	return oss.str();
}

bool CCluster::IsValidSvrId(uint16_t uSvrId) const
{
	auto& rMap = m_pInfo->cell_info_map();
	bool isValid = rMap.find(uSvrId) != rMap.end();

	return isValid;
}

