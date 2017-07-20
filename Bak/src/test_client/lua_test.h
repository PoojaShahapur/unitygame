#ifndef LUA_TEST_H
#define LUA_TEST_H

#include "asio/asio_fwd.h"  // for io_service

#include <memory>  // for unique_ptr
#include <string>

class Client;
class LoginTest;
class CmdTest;

namespace LuaIntf {
class LuaRef;
}

struct lua_State;

class LuaTest
{
public:
	explicit LuaTest(lua_State* L, boost::asio::io_service& rIoService);
	virtual ~LuaTest();

public:
	void PrintHelp() const;
	void Run();

private:
	void InitLua();
	void Bind();
	void DoLuaInitFile(const std::string& sLuaInitFileName);
	std::string GetLuaRootPath() const;

private:
	void OnResponse(uint32_t uLuaRpcId, const std::string& sRespContent) const;
	void RequestRpc(
		const std::string& sService,
		const std::string& sMethod,
		const std::string& sRequest,
		const LuaIntf::LuaRef& luaCallback) const;
	void ReplyRpc(uint32_t uRpcId, const std::string& sResponse) const;

private:
	lua_State* m_pLuaState;

	std::unique_ptr<Client> m_pClient;
	std::unique_ptr<LoginTest> m_pLoginTest;
	std::unique_ptr<CmdTest> m_pTestForInner;
};

#endif  // LUA_TEST_H
