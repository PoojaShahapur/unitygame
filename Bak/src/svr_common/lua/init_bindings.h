#ifndef SVR_COMMON_LUA_INIT_BINDINGS_H
#define SVR_COMMON_LUA_INIT_BINDINGS_H

struct lua_State;

namespace Lua
{
// 由 CApp::InitLua() 调用。
void InitBindings(lua_State* L);
}

#endif  // SVR_COMMON_LUA_INIT_BINDINGS_H
