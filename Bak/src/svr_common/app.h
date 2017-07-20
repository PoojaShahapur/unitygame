#ifndef _SVR_COMMON_APP_HEAD__
#define _SVR_COMMON_APP_HEAD__

#include "singleton.h"
#include "asio/asio_fwd.h"  // for io_service

#include <memory>  // for unique_ptr<>
#include <string>

class CAsioServer4C;
class CAsioServer4S;
class CEventHandler;
class CRedis;

namespace LuaIntf {
class LuaContext;
}  // namespace LuaIntf

struct lua_State;

// CApp单件，应用对象(Application)，用来保存和获取全局对象。
class CApp : public Singleton<CApp>
{
public:
	CApp();
	virtual ~CApp();

public:
	uint16_t GetMySvrId() const
	{
		assert(m_bInited);
		return m_uMySvrId;
	}
	CAsioServer4C& GetSvr4C() const
	{
		assert(m_bInited);
		return *m_pSvr4C;
	}
	CAsioServer4S& GetSvr4S() const
	{
		assert(m_bInited);
		return *m_pSvr4S;
	}
	CRedis& GetRedis() const;
	lua_State* GetLuaState() const;

public:
	bool Init(int16_t uMySvrId);
	void BlockingRun();

private:
	bool DoLuaInitFile(const std::string& sLuaInitFileName);
	bool InitLua();
	void InitServers();
	bool InitRedis();
	void InitTimeLoop();

private:
	bool m_bInited = false;
	uint16_t m_uMySvrId = 0;
	using io_service = boost::asio::io_service;
	std::unique_ptr<io_service> m_pIoService;
	std::unique_ptr<LuaIntf::LuaContext> m_pLuaCtx;
	std::unique_ptr<CAsioServer4C> m_pSvr4C;
	std::unique_ptr<CAsioServer4S> m_pSvr4S;
	std::unique_ptr<CRedis> m_pRedis;
};

#endif  // _SVR_COMMON_APP_HEAD__
