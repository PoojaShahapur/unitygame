#ifndef SVR_COMMON_LUA_LUA_MONGODB_H
#define SVR_COMMON_LUA_LUA_MONGODB_H

struct lua_State;

namespace LuaMongoDb
{
void Bind(lua_State* L);
}

#endif  // SVR_COMMON_LUA_LUA_MONGODB_H
