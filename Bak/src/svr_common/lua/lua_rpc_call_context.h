#ifndef SVR_COMMON_LUA_LUA_RPC_CALL_CONTEXT_H
#define SVR_COMMON_LUA_LUA_RPC_CALL_CONTEXT_H

struct lua_State;

namespace LuaRpcCallContext
{
void Bind(lua_State* L);
}

#endif  // SVR_COMMON_LUA_LUA_RPC_CALL_CONTEXT_H
