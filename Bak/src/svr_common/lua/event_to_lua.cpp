#include "event_to_lua.h"

#include "log.h"
#include "util.h"  // for GetLuaState()

const char LOG_NAME[] = "CEventToLua";

CEventToLua::CEventToLua()
{
	lua_State* L = Util::GetLuaState();
	assert(L);

	using LuaIntf::LuaRef;
	LuaRef require(L, "require");
	try
	{
		m_handler = require.call<LuaRef>("event_handler.event_handler");
	}
	catch (const LuaIntf::LuaException& e)
	{
		LOG_WARN("Failed to require event_handler. " << e.what());
	}
	if (m_handler.isTable())
		return;

	LOG_WARN("Lua require should return a table.");
	m_handler = LuaRef();  // reset
}

CEventToLua::~CEventToLua()
{
}

