#ifndef SVR_COMMON_LUA_LUA_BIND_H
#define SVR_COMMON_LUA_LUA_BIND_H

#include <string>

struct lua_State;

namespace Lua
{

// Bind lua module.
// Example: Bind(L, "c_logger", LuaOpenModule_Logger);
typedef int(*lua_CFunction) (lua_State *L);
void Bind(lua_State* L, const std::string& sModuleName,
	lua_CFunction funLuaOpenModule);

}  // namespace Lua
#endif  // SVR_COMMON_LUA_LUA_BIND_H
