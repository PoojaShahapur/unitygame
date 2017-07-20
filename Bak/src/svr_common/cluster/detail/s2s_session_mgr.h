#ifndef CELL_CLUSTER_S2S_SESSION_MGR_H
#define CELL_CLUSTER_S2S_SESSION_MGR_H

#include "asio/asio_session_out_sptr.h"  // for CAsioSessionOutSptr

#include <unordered_map>

class CsvRecord;

// 服务器互连会话管理器。
// 管理 CAsioSessionOut
class CS2sSessionMgr
{
public:
	CS2sSessionMgr();
	virtual ~CS2sSessionMgr();

public:
	// 按server.csv配置初始化
	void InitFromConfig();
	bool HasSession(uint16_t uSvrId,
		const std::string& sHost, uint16_t uPort) const;
	void EraseSession(uint16_t uSvrId);
	// 会自动停止旧Session并删除
	void RespawnS2sSession(uint16_t uSvrId,
		const std::string& sHost, uint16_t uPort);
	CAsioSessionOut* GetSession(uint16_t uSvrId) const;

private:
	void SpawnS2sSessions();
	void SpawnS2sSession(const CsvRecord& cfg);

private:
	using SessionMap = std::unordered_map<uint16_t, CAsioSessionOutSptr>;
	SessionMap m_sessionMap;
};  // CS2sSessionMgr

#endif  // CELL_CLUSTER_S2S_SESSION_MGR_H
