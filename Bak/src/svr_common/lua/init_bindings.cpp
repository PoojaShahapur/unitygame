#include "init_bindings.h"

#include "lua/lua_rpc_router.h"  // for LuaRpcRouter
#include "lua_csv.h"  // for LuaCsv
#include "lua_game_clt_id.h"  // for LuaGameCltId
#include "lua_logger.h"  // for LuaLogger
#include "lua_mongodb.h"
#include "lua_redis.h"  // for LuaRedis
#include "lua_remote_rpc_router_modifier.h"  // for LuaRemoteRpcRouterModifier
#include "lua_rpc.h"  // for LuaRpc
#include "lua_rpc_call_context.h"  // for LuaRpcCallContext
#include "lua_timer_queue.h"  // for LuaTimerQueue
#include "lua_util.h"  // for LuaUtil
#include "lua_httpclient.h" // 

#include <cassert>

namespace Lua {

void InitBindings(lua_State* L)
{
	assert(L);
	LuaLogger::Bind(L);
	LuaRpc::Bind(L);
	LuaRpcCallContext::Bind(L);
	LuaMongoDb::Bind(L);
	LuaTimerQueue::Bind(L);
	LuaUtil::Bind(L);
	LuaGameCltId::Bind(L);
	LuaRedis::Bind(L);
	LuaRpcRouter::Bind(L);
	LuaCsv::Bind(L);
	LuaHttpClient::Bind(L);
	LuaRemoteRpcRouterModifier::Bind(L);

	// 新增Lua导出须在此添加，并且更新
	// doc/lua/cpp_to_lua_api.md
}

}  // namespace Lua
