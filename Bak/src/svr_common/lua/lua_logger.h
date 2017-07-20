#ifndef SVR_COMMON_LUA_LUA_LOGGER_H
#define SVR_COMMON_LUA_LUA_LOGGER_H

struct lua_State;

namespace LuaLogger
{
void Bind(lua_State* L);
}

#endif  // SVR_COMMON_LUA_LUA_LOGGER_H
