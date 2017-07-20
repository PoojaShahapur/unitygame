#include "s2s_session_mgr.h"

#include "app.h"  // for CApp
#include "asio/asio_session_out.h"  // for CAsioSessionOut
#include "csv/csv.h"  // for GetTable()
#include "log.h"

const char LOG_NAME[] = "CS2sSessionMgr";

CS2sSessionMgr::CS2sSessionMgr()
{
}

CS2sSessionMgr::~CS2sSessionMgr()
{
}

void CS2sSessionMgr::InitFromConfig()
{
	// Connect to other servers.
	SpawnS2sSessions();
}

bool CS2sSessionMgr::HasSession(uint16_t uSvrId,
	const std::string& sHost, uint16_t uPort) const
{
	auto itr = m_sessionMap.find(uSvrId);
	if (itr == m_sessionMap.end())
		return false;
	const CAsioSessionOutSptr& pSession = (*itr).second;
	assert(pSession);
	assert(pSession->GetRemoteSvrId() == uSvrId);
	return pSession->GetRemoteHost() == sHost &&
		pSession->GetRemotePort() == uPort;
}

void CS2sSessionMgr::EraseSession(uint16_t uSvrId)
{
	auto itr = m_sessionMap.find(uSvrId);
	if (itr == m_sessionMap.end())
		return;
	LOG_INFO("Erase s2s session. SvrId=" << uSvrId);
	(*itr).second->End();
	m_sessionMap.erase(itr);
}

void CS2sSessionMgr::SpawnS2sSessions()
{
	uint16_t uMySvrId = CApp::get_const_instance().GetMySvrId();
	CsvTable& cfgTbl = CsvCfg::GetTable("server.csv");
	const std::string& sMyArea = cfgTbl.GetRecord(
		"id", uMySvrId).GetString("area");
	const CsvRecordSptrVec& vRec = cfgTbl.GetRecords("area", sMyArea);
	// 包括自身连接
	for (CsvRecordSptr pRec : vRec)
	{
		assert(pRec);
		SpawnS2sSession(*pRec);
	}
}

void CS2sSessionMgr::SpawnS2sSession(const CsvRecord& cfg)
{
	uint16_t uSvrId = cfg.GetInt<uint16_t>("id");
	if (0 == uSvrId)
	{
		LOG_WARN("Illegal server.csv ('id' = 0).");
		return;
	}
	const std::string& sHost = cfg.GetString("inner_host");
	uint16_t uPort = cfg.GetInt<uint16_t>("inner_port");
	RespawnS2sSession(uSvrId, sHost, uPort);
}

CAsioSessionOut* CS2sSessionMgr::GetSession(uint16_t uSvrId) const
{
	auto itr = m_sessionMap.find(uSvrId);
	if (itr == m_sessionMap.end())
		return nullptr;
	assert((*itr).second);
	return (*itr).second.get();
}

void CS2sSessionMgr::RespawnS2sSession(uint16_t uSvrId,
	const std::string& sHost, uint16_t uPort)
{
	assert(uSvrId);
	LOG_DEBUG(Fmt("Respawn s2s session to Cell_%1%(%2%:%3%)")
		% uSvrId % sHost % uPort);
	EraseSession(uSvrId);

	CAsioServer4S& rSvr4S = CApp::get_const_instance().GetSvr4S();
	auto pSession = std::make_shared<CAsioSessionOut>(
		rSvr4S, uSvrId, sHost, uPort);
	m_sessionMap[uSvrId] = pSession;
	pSession->Go();
}
