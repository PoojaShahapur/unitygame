/*----------------------------------------------------------------
// Copyright (C) 2016 苏州，顾宏
//
// 模块名：CApp
// 创建者：Macro Gu
// 创建日期：2016.1.11
// 修改者列表：
//   Jin Qing (http://blog.csdn.net/jq0123)
//----------------------------------------------------------------*/

#include "app.h"

#include "asio/asio_server4c.h"  // for dtr()
#include "asio/asio_server4s.h"  // for dtr()
#include "csv/csv.h"  // for CsvCfg::Init()
#include "log.h"
#include "lua/init_bindings.h"  // for InitBindings()
#include "multi_task/multi_task_mgr.h"  // for MultiTask
#include "redis/redis.h"  // for CRedis
#include "timer_queue/timer_queue_root.h"  // for TimerQueueRoot
#include "register_services.h"  // for RegisterServices4Clt()

#include <LuaIntf/LuaIntf.h>  // for LuaContext

#include <boost/asio/io_service.hpp>  // for io_service
#include <boost/asio/spawn.hpp>  // for yield_context
#include <boost/asio/steady_timer.hpp>  // for steady_timer

const char LOG_NAME[] = "CApp";
const char SCRIPTS_DIR[] = "../scripts";

CApp::CApp()
{
}

CApp::~CApp()
{
}

void CApp::BlockingRun()
{
	assert(m_bInited);
	try
	{
		m_pIoService->run();
	}
	catch (std::exception& e)
	{
		LOG_ERROR("Exception: " << e.what());
	}
}

bool CApp::Init(int16_t uMySvrId)
{
	assert(uMySvrId);
	m_uMySvrId = uMySvrId;
	m_pIoService.reset(new io_service);
	m_pLuaCtx.reset(new LuaIntf::LuaContext);
	m_pSvr4C.reset(new CAsioServer4C(*m_pIoService));
	m_pSvr4S.reset(new CAsioServer4S(*m_pIoService));
	m_pRedis.reset(new CRedis);

	if (!CsvCfg::Init())  // 必须最先初始化
		return false;

	InitServers();
	if (!InitRedis())
		return false;
	InitTimeLoop();

	// 启动多任务管理器
	unsigned threadnum = 8;  // GetOptValue("multi_task", "threadnum", "8")
	MultiTask::Mgr::get_mutable_instance().Run(m_pIoService.get(), threadnum);

	m_bInited = true;

	// InitLua()要在最后，要求C++已初始化
	return InitLua();
}

CRedis& CApp::GetRedis() const
{
	assert(m_pRedis);
	return *m_pRedis;
}

lua_State* CApp::GetLuaState() const
{
	assert(m_bInited);
	return m_pLuaCtx->state();
}

bool CApp::InitLua()
{
	assert(m_pLuaCtx);
	try
	{
		Lua::InitBindings(m_pLuaCtx->state());
		// lua脚本的顶层目录名
		m_pLuaCtx->setGlobal("G_LUA_ROOTPATH", SCRIPTS_DIR);
	}
	catch (const std::string& e)
	{
		LOG_FATAL("CApp InitLua() exception: " << e);
		return false;
	}
	if (!DoLuaInitFile("common/init_common.lua"))
		return false;
	if (!DoLuaInitFile("cell/init_cell.lua"))
		return false;
	return true;
}

// sLuaInitFileName 必须是相对于 LuaPath 目录的文件名，如 "base/init_base.lua"
bool CApp::DoLuaInitFile(const std::string& sLuaInitFileName)
{
	assert(m_pLuaCtx);
	// lua脚本的顶层路径名
	std::string sLuaInitFilePath =
		(Fmt("%1%/%2%") % SCRIPTS_DIR % sLuaInitFileName).str();
	try {
		// 读取初始化文件, load所有的lua文件, 由init文件负责完成
		m_pLuaCtx->doFile(sLuaInitFilePath.c_str());
	}
	catch (LuaIntf::LuaException& e) {
		LOG_ERROR(Fmt("Failed to run '%1%', %2%") % sLuaInitFilePath % e.what());
		return false;
	}
	return true;
}

void CApp::InitServers()
{
	assert(m_uMySvrId);
	assert(m_pSvr4C);
	assert(m_pSvr4S);

	const auto& cfg = CsvCfg::GetTable("server.csv")
		.GetRecord("id", m_uMySvrId);
	uint16_t uInnerPort = cfg.GetInt<uint16_t>("inner_port");
	uint16_t uOuterPort = cfg.GetInt<uint16_t>("outer_port");
	m_pSvr4C->Init(uOuterPort);
	m_pSvr4S->Init(uInnerPort);
	RegisterServices4Clt(*m_pSvr4C);
	RegisterServices4Svr(*m_pSvr4S);
}

bool CApp::InitRedis()
{
	assert(m_uMySvrId);
	assert(m_pIoService);
	assert(m_pRedis);

	const std::string& sMyArea = CsvCfg::GetTable("server.csv")
		.GetRecord("id", m_uMySvrId).GetString("area");
	const auto& cfg = CsvCfg::GetTable("redis.csv").GetRecord("area", sMyArea);
	bool bDisabled = cfg.GetBool("disabled");
	if (bDisabled)
	{
		LOG_INFO("Redis is disabled.");
		return true;  // 禁用Redis
	}

	const std::string& sHost = cfg.GetString("host");
	uint16_t uPort = cfg.GetInt<uint16_t>("port");
	return m_pRedis->Init(*m_pIoService, sHost, uPort);
}

// Timer.
static void LoopTimer(boost::asio::io_service& rIos,
	boost::asio::yield_context yield)
{
	boost::asio::steady_timer timer(rIos);
	TimerQueueRoot& rTimerQueueRoot = TimerQueueRoot::Get();
	for (;;)
	{
		rTimerQueueRoot.Tick();
		timer.expires_from_now(std::chrono::microseconds(10));
		boost::system::error_code ec;
		timer.async_wait(yield[ec]);
		if (ec)
			LOG_ERROR(Fmt("Async wait error: (%d)%s") % ec.value() % ec.message());
	}
}

void CApp::InitTimeLoop()
{
	assert(m_pIoService);
	io_service& rIos = *m_pIoService;
	using namespace boost::asio;
	spawn(rIos, [&rIos](yield_context yield){
		LoopTimer(rIos, yield);
	});
}

