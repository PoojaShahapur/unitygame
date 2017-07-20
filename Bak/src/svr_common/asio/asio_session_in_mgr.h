#ifndef SVR_COMMON_ASIO_ASIO_SESSION_N_MGR_HEAD__
#define SVR_COMMON_ASIO_ASIO_SESSION_N_MGR_HEAD__

#include "singleton.h"  // for Singleton<>

#include <cstdint>  // for uint16_t
#include <unordered_map>

class CAsioSessionIn;

// 管理连入的会话。（连出会话CAsioSessionOut由应用管理。）
class CAsioSessionInMgr : public Singleton<CAsioSessionInMgr>
{
public:
	CAsioSessionInMgr();
	virtual ~CAsioSessionInMgr();

	void InsertSession(CAsioSessionIn& rSession);
	void EraseSession(CAsioSessionIn& rSession);
	CAsioSessionIn* GetSessionIn(uint64_t uSessionId) const;

private:
	// SessionId -> SessionIn
	std::unordered_map<uint64_t, CAsioSessionIn*> m_mapSessions;
};

#endif  // SVR_COMMON_ASIO_ASIO_SESSION_N_MGR_HEAD__

