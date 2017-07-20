/// 更改远程Rpc路由表.
// 也支持本服Rpc路由表更改(但本服使用 `c_rpc_router` 更简单).
// @module c_remote_rpc_router_modifier

#include "lua_remote_rpc_router_modifier.h"

#include "lua_bind.h"  // for Lua::Bind()
#include "lua_remote_rpc_router_modifier_impl.h"  // for Modifier
#include "rpc_forward/remote_rpc_router_modifier.h"  // for CRemoteRpcRouterModifier

#include <LuaIntf/LuaIntf.h>

extern "C" int LuaOpenModule_RemoteRpcRouterModifier(lua_State* L)
{
	assert(L);
	using namespace LuaIntf;
	using Modifier = LuaRemoteRpcRouterModifierImpl::Modifier;
	LuaRef mod = LuaRef::createTable(L);
	LuaBinding(mod).beginClass<Modifier>("CRemoteRpcRouterModifier")
		.addConstructor(LUA_ARGS(uint16_t))
		.addFunction("set_svc_dst_svr_id", &Modifier::SetSvcDstSvrId)
		.addFunction("reset_svc_dst_svr_id", &Modifier::ResetSvcDstSvrId)
		.addFunction("set_mthd_dst_svr_id", &Modifier::SetMthdDstSvrId)
		.addFunction("reset_mthd_dst_svr_id", &Modifier::ResetMthdDstSvrId)
		;
	mod.pushToStack();
	return 1;
}

namespace LuaRemoteRpcRouterModifier {

void Bind(lua_State* L)
{
	assert(L);
	::Lua::Bind(L, "c_remote_rpc_router_modifier",
		LuaOpenModule_RemoteRpcRouterModifier);
}

}  // namespace LuaRemoteRpcRouterModifier

/// @type CRemoteRpcRouterModifier
;
/// 构造函数
// @function CRemoteRpcRouterModifier
// @int remote_svr_id 远程服务器ID, 也支持本服
;
/// 设置玩家服务目的服ID号.
// @function set_svc_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @int svr_id 目的服ID号
// @func[opt] cb 回调, function()
;
/// 重置玩家服务目的服ID号.
// @function reset_svc_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @func[opt] cb 回调, function()
;
/// 设置玩家服务方法目的服ID号.
// @function set_mthd_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @string method 方法名
// @int svr_id 目的服ID号
// @func[opt] cb 回调, function()
;
/// 重置玩家服务方法目的服ID号.
// @function reset_mthd_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @string method 方法名
// @func[opt] cb 回调, function()
;
