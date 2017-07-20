#ifndef SVR_COMMON_LUA_LUA_UTIL_H
#define SVR_COMMON_LUA_LUA_UTIL_H

struct lua_State;

namespace LuaUtil
{
void Bind(lua_State* L);
}

#endif  // SVR_COMMON_LUA_LUA_UTIL_H
