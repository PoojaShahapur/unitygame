#include <log4cxx/ndc.h>
#include <log4cxx/xml/domconfigurator.h>

#include "asio/asio_server4c.h"  // for CAsioServer4C
#include "asio/asio_server4s.h"  // for CAsioServer4S
#include "cluster/cluster.h"  // for CCluster
#include "log.h"
#include "util.h"  // for SetServerIdEnv()
#include "app.h"
#include "lua.h"
#include "svc_test_cpp.h"  // for CSvcTestCpp
#include "svc_plane.h"  // for CSvcTestCpp
#include "event_handler.h"  // for CEventHandler
#include "formation.h"//初始化阵型,目前默认计算1-500架共500个机群的阵型
#include "pos.h"//Vector2

#ifdef ENABLE_GVOE_GPROFTOOLS
#include <profiler.h>
#endif

const char LOG_NAME[] = "main";

int main(int argc, char* argv[])
{

#ifdef ENABLE_GVOE_GPROFTOOLS
    std::string application_name = "cell";
    Profiler::heap_profiler_start(application_name.c_str());
    std::string pprof_name = application_name + ".cpu.prof";
    Profiler::profiler_start(pprof_name.c_str());
#endif

	log4cxx::NDC ndcMain("");
	if (argc < 2)
	{
		LOG_ERROR(Fmt("Usage: %s server_id") % argv[0]);
		return -1;
	}
	uint16_t uServerId = (uint16_t)atoi(argv[1]);
	if (!Util::SetServerIdEnv(uServerId))  // for log4cxx
		return -1;

	// Must after SerServerIdEnv().
	log4cxx::xml::DOMConfigurator::configureAndWatch("log4j/cell.xml", 5000);
	LOG_INFO("-------------------------------");
	LOG_INFO(Fmt("Start cell server (ID=%1%).") % uServerId);
	LOG_INFO("-------------------------------");
	if (0 == uServerId)
	{
		LOG_FATAL("Server ID is 0!");
		return -1;
	}

	CApp& rApp = CApp::get_mutable_instance();
	if (!rApp.Init(uServerId))
		return -2;

    CellLua::Bind(rApp.GetLuaState());
    HexagonFormation::get_mutable_instance().initFormationMap(500);
    HexagonFormation::get_mutable_instance().initFormationRadiusMap(500);
    CircleFormation::get_mutable_instance().initFormationMap(500);
    CircleFormation::get_mutable_instance().initFormationRadiusMap(500);
	rApp.GetSvr4C().RegisterService(std::make_shared<CSvcTestCpp>());
	rApp.GetSvr4C().RegisterService(std::make_shared<CSvcPlane>());
	std::shared_ptr<CEventHandler> m_pEventHandler;  // Init()中初始化
	m_pEventHandler.reset(new CEventHandler);
	rApp.GetSvr4S().SetEventHandler(m_pEventHandler);
	rApp.GetSvr4C().SetEventHandler(m_pEventHandler);

	// Cluster init must after CApp::Init() and EventHandler.
	CCluster::get_mutable_instance().Init();

	rApp.BlockingRun();

#ifdef ENABLE_GVOE_GPROFTOOLS
    Profiler::profiler_stop();
    Profiler::heap_profiler_stop();
#endif

	return 0;
}
