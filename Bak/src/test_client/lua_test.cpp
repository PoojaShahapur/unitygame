#include "lua_test.h"

#include "client.h"
#include "cmd_test.h"
#include "fmt.h"
#include "log.h"
#include "login_test.h"
#include "lua/lua_logger.h"  // for LuaLogger
#include "lua/lua_timer_queue.h"  // for LuaTimerQueue
#include "lua/to_function.h"  // for ToFunction<>
#include "lua/lua_csv.h"  // for LuaCsv

#include <LuaIntf/LuaIntf.h>
#include <iostream>

const char LOG_NAME[] = "LuaTest";

LuaTest::LuaTest(lua_State* L, boost::asio::io_service& rIoService) :
	m_pLuaState(L),
	m_pClient(new Client(L, rIoService)),  // unique_ptr
	m_pLoginTest(new LoginTest(*m_pClient)),  // unique_ptr
	m_pTestForInner(new CmdTest(*m_pClient))  // unique_ptr
{
	assert(L);

	InitLua();
}

LuaTest::~LuaTest()
{
}

void LuaTest::Run()
{
	DoLuaInitFile("test_client/bot_auto_run.lua");
}

void LuaTest::InitLua()
{
	Bind();

	// lua脚本的顶层路径名
	LuaIntf::LuaContext ctx(m_pLuaState);
	ctx.setGlobal("G_LUA_ROOTPATH", GetLuaRootPath());
	DoLuaInitFile("common/init_common.lua");
	DoLuaInitFile("test_client/init_test_client.lua");
}

void LuaTest::Bind()
{
	assert(m_pLuaState);
	assert(m_pClient);

	LuaIntf::LuaBinding(m_pLuaState).beginModule("c_cmd")
		.addFunction("mongodb_test", [this]() { m_pTestForInner->Test("mongodb_test"); })
	.endModule();

	LuaLogger::Bind(m_pLuaState);

	LuaIntf::LuaBinding(m_pLuaState).beginModule("c_rpc")
		.addFunction("request", [this](
			const std::string& sService,
			const std::string& sMethod,
			const std::string& sRequest,
			const LuaIntf::LuaRef& luaCallback) {
			RequestRpc(sService, sMethod, sRequest, luaCallback);
		})
		.addFunction("reply", [this](
			uint32_t uRpcId, const std::string& sResponse) {
			ReplyRpc(uRpcId, sResponse);
		})
	.endModule();

	LuaIntf::LuaBinding(m_pLuaState).beginModule("c_test")
		.addFunction("login", [this]() { m_pLoginTest->TestLogin(); })
		.addFunction("cmd", [this](const char *cmd) { m_pTestForInner->Test(cmd); })
	.endModule();

	LuaTimerQueue::Bind(m_pLuaState);
	LuaCsv::Bind(m_pLuaState);
}

// sLuaInitFileName 必须是相对于 LuaPath 目录的文件名，如 "cell/init_cell.lua"
void LuaTest::DoLuaInitFile(const std::string& sLuaInitFileName)
{
	// lua脚本的顶层路径名
	std::string sLuaInitFilePath =
		(Fmt("%1%/%2%") % GetLuaRootPath() % sLuaInitFileName).str();
	try {
		// 读取初始化文件, load所有的lua文件, 由init文件负责完成
		LuaIntf::LuaContext ctx(m_pLuaState);
		ctx.doFile(sLuaInitFilePath.c_str());
	} catch (LuaIntf::LuaException& e) {
		std::cerr << Fmt("Failed to run '%1%', %2%\n")
			% sLuaInitFilePath % e.what();
	}
}

std::string LuaTest::GetLuaRootPath() const
{
	return "../scripts";
}

void LuaTest::RequestRpc(
	const std::string& sService,
	const std::string& sMethod,
	const std::string& sRequest,
	const LuaIntf::LuaRef& luaCallback) const
{
	// default empty callback
	RpcCallback cb = Lua::ToFunction<RpcCallback>(luaCallback);
	m_pClient->PushRpcReq(sService, sMethod, sRequest, cb);
}

void LuaTest::ReplyRpc(uint32_t uRpcId, const std::string& sResponse) const
{
	m_pClient->PushRpcResp(uRpcId, sResponse);
}

