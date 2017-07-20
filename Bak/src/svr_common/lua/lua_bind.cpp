#include "lua_bind.h"

#include <LuaIntf/LuaIntf.h>
#include <cassert>

namespace Lua {

void Bind(lua_State* L, const std::string& sModuleName,
	lua_CFunction funLuaOpenModule)
{
	assert(L);
	assert(funLuaOpenModule);
	using LuaIntf::LuaRef;
	LuaRef table(L, "package.preload");
	table[sModuleName] = LuaRef::createFunctionWith(L, funLuaOpenModule);
}

}  // namespace Lua
