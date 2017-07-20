#include "log.h"

#include "lua_test.h"  // for LuaTest
#include "read_lines.h"  // for ReadLines
#include "plane/plane_test.h"  // for PlaneTest
#include "timer_queue/timer_queue_root.h"  // for TimerQueueRoot
#include "util.h"  // for GetMs()

#include <behaviac/behaviac.h>
#include <boost/asio/io_service.hpp>
#include <boost/asio/spawn.hpp>
#include <boost/asio/steady_timer.hpp>
#include <log4cxx/ndc.h>
#include <log4cxx/xml/domconfigurator.h>
#include <lua.h>

#include <future>

#include <memory>
#include <vector>

#include "csv/csv_cfg.h"
#include "csv/csv_table.h"
#include "csv/csv_record.h"

struct lua_State;

const char LOG_NAME[] = "main";

static void InitLog4cxx()
{
	log4cxx::xml::DOMConfigurator::configureAndWatch("log4j/test_client.xml", 5000);
	LOG_INFO("-------------------------------");
	LOG_INFO("Start test client");
	LOG_INFO("-------------------------------");
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
		timer.expires_from_now(std::chrono::milliseconds(100));
		boost::system::error_code ec;
		timer.async_wait(yield[ec]);
		if (ec)
		{
			LOG_ERROR(Fmt("Async wait error: (%d)%s")
				% ec.value() % ec.message());
		}
	}
}

static void InitBehavic()
{
	behaviac::Workspace::GetInstance()->SetFilePath("../behaviac_plane/exported");
	behaviac::Workspace::GetInstance()->SetFileFormat(behaviac::Workspace::EFF_xml);
	behaviac::Config::SetLogging(true);
}

static void CleanupBehaviac()
{
	behaviac::Workspace::GetInstance()->Cleanup();
}

static auto* s_pWs = behaviac::Workspace::GetInstance();

static void BehaviacUpdate()
{
	s_pWs->SetDoubleValueSinceStartup(Util::GetMs());
	s_pWs->Update();
}

using io_service = boost::asio::io_service;
int g_argc;
char** g_argv;
extern lua_State* lua_main(int argc, char **argv);

class Bot {
private:
	std::shared_ptr<LuaTest> m_luaTestSptr;
	std::shared_ptr<PlaneTest> m_planeTestSptr;
	lua_State* L;
public:
	Bot(io_service& ioService)
		:L(NULL)
	{
		L = lua_main(g_argc, g_argv);
		if (!L) {

			lua_close(L);
			return;
		}

		m_luaTestSptr = std::make_shared<LuaTest>(L, ioService);
		m_planeTestSptr = std::make_shared<PlaneTest>(L);
	}
	void Active() {
		m_luaTestSptr->Run();
	}

	~Bot() {
		if (L)
			lua_close(L);
	}
};

using BotSptr = std::shared_ptr<Bot>;
using yield_context = boost::asio::yield_context;
class TestBots {

private:
	std::vector<BotSptr> m_botList;
	int m_botNum;
	io_service& m_ioService;
public:
	TestBots(io_service& ios, int num)
		:m_botNum(num),m_ioService(ios){
	}
	void SpawnBot(int interval, yield_context yield) {

		boost::asio::steady_timer timer(m_ioService);
		int idx = 0;
		for (;;)
		{
			timer.expires_from_now(std::chrono::milliseconds(interval));
			boost::system::error_code ec;
			timer.async_wait(yield[ec]);
			if (ec)
			{
				LOG_ERROR(Fmt("Async wait error: (%d)%s")
					% ec.value() % ec.message());
				break;
			}

			ActiveBot(idx++);
			if (idx >= m_botNum)
			{
				break;
			}
		}	
	}
	void MakeBots() {
		for (int i = 0; i < m_botNum; i++)
		{
			m_botList.push_back(std::make_shared<Bot>(m_ioService));
		}
	}

	void ActiveBot(int idx) {
		if (idx >= m_botNum)
			return;
		m_botList[idx]->Active();
	}
};
using boost::asio::spawn;

int main(int argc, char* argv[])
{

	g_argc = argc;
	g_argv = argv;

	int botsNum = 1;
	int loginInterval = 1;
	bool auto_test = false;

	{ // load config from csv
		bool ok = CsvCfg::Init();
		assert(ok);
		CsvTable& t = CsvCfg::GetTable("test_client.csv");
		const auto& rAuto = t.GetRecord("key", "auto_test");
		auto_test = rAuto.GetBool("ivalue");

		const auto& rCount = t.GetRecord("key", "client_count");
		botsNum = rCount.GetInt<uint32_t>("ivalue");

		const auto& rInterval = t.GetRecord("key", "login_interval");
		loginInterval = rInterval.GetInt<uint32_t>("ivalue");
	}

	InitLog4cxx();
	log4cxx::NDC ndcMain("");
	srand(time(NULL));

	InitBehavic();

	TimerQueueRoot::Get().InsertRepeatNow(1000, BehaviacUpdate);

	io_service ioService;
	try
	{
		using yield_context = boost::asio::yield_context;
		spawn(ioService, [&ioService](yield_context yield) {
			LoopTimer(ioService, yield);
		});

		if (auto_test)
		{
			static TestBots bots(ioService, botsNum);
			using std::placeholders::_1;
			bots.MakeBots();

			spawn(ioService, std::bind(&TestBots::SpawnBot, &bots, loginInterval, _1));
		}
		else
		{
			lua_State* L = lua_main(g_argc, g_argv);
			static LuaTest luaTest(L, ioService);
			static PlaneTest planeTest(L);

			extern void doREPL(lua_State *L, io_service&, yield_context);
			spawn(ioService, [L, &ioService](yield_context yield) {
				doREPL(L, ioService, yield);
			});
		}

		ioService.run();
	}
	catch (std::exception& e)
	{
		LOG_ERROR("Exception: " << e.what());
	}

	CleanupBehaviac();
}
