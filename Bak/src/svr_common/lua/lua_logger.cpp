#include "lua_logger.h"

#include <LuaIntf/LuaIntf.h>

#include "log.h"

namespace {

std::string GetLuaLogName(const std::string& sSubName)
{
	return "Lua." + sSubName;
}

void debug(const std::string& sLogName, const std::string& sMsg)
{
	LOG_DEBUG_TO(GetLuaLogName(sLogName), sMsg);
}

void info(const std::string& sLogName, const std::string& sMsg)
{
	LOG_INFO_TO(GetLuaLogName(sLogName), sMsg);
}

void warn(const std::string& sLogName, const std::string& sMsg)
{
	LOG_WARN_TO(GetLuaLogName(sLogName), sMsg);
}

void error(const std::string& sLogName, const std::string& sMsg)
{
	LOG_ERROR_TO(GetLuaLogName(sLogName), sMsg);
}

void fatal(const std::string& sLogName, const std::string& sMsg)
{
	LOG_FATAL_TO(GetLuaLogName(sLogName), sMsg);
}
}  // namespace

namespace LuaLogger {

void Bind(lua_State* L)
{
	assert(L);
	LuaIntf::LuaBinding(L).beginModule("c_logger")
		.addFunction("debug", &debug)
		.addFunction("info", &info)
		.addFunction("warn", &warn)
		.addFunction("error", &error)
		.addFunction("fatal", &fatal)
	.endModule();
}

}  // namespace LuaLogger

