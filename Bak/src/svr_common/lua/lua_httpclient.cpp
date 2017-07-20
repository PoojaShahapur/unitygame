#include "lua_httpclient.h"

#include "lua/event_to_lua.h"
#include "fmt.h"

#include <LuaIntf/LuaIntf.h>

#include <functional>
#include "httpclient/httpclient_task.h"

namespace LuaIntf
{
	LUA_USING_LIST_TYPE(std::vector)
}


namespace {

	void __httpclient_get_callback(uint32_t id, const std::string &retdata)
	{
		//LOG_INFO_TO("__httpclient_get_callback", Fmt("result size:%d") % retdata.size());
		std::string lua_event_id = (Fmt("__httpclient_get_%u") % id).str();
		CEventToLua().Dispatch(lua_event_id, retdata);
	}

	void __httpclient_post_callback(uint32_t id, const std::string &retdata)
	{
		std::string lua_event_id = (Fmt("__httpclient_post_%u") % id).str();
		CEventToLua().Dispatch(lua_event_id, retdata);
	}

	uint32_t httpclient_get(const std::string& url)
	{
		static uint32_t g_query_id = 0;
		HttpClient_Get(url)->SetQueryCB(std::bind(__httpclient_get_callback, ++g_query_id, std::placeholders::_1))->Go();
		return g_query_id;
	}

	uint32_t httpclient_post(const std::string& url, const std::string& data)
	{
		static uint32_t g_query_id = 0;
		HttpClient_Post(url, data)->SetQueryCB(std::bind(__httpclient_post_callback, ++g_query_id, std::placeholders::_1))->Go();
		return g_query_id;
	}
}  // namespace

namespace LuaHttpClient {

	void Bind(lua_State* L)
	{
		assert(L);
		LuaIntf::LuaBinding(L).beginModule("c_httpclient")
			.addFunction("get", &httpclient_get)
			.addFunction("post", &httpclient_post)
			.endModule();
	}

}  // namespace LuaHttpClient

