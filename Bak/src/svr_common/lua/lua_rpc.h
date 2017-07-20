#ifndef SVR_COMMON_LUA_LUA_RPC_H
#define SVR_COMMON_LUA_LUA_RPC_H

struct lua_State;

namespace LuaRpc
{
void Bind(lua_State* L);
}

#endif  // SVR_COMMON_LUA_LUA_RPC_H
