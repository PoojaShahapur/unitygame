/// Rpc路由表.
//
// * 可以设置整个服务的路由，也可以设置某个方法的路由。
// * 可根据服务名和方法名查找目的服ID.
// * 不同玩家有不同的路由表.
// * 全局功能服路由会动态选举目的服。
// * 玩家路由优先于全局功能服路由。
// * 方法名路由优先于服务名路由。
// @module c_rpc_router

#include "lua_rpc_router.h"

#include "lua_bind.h"  // for Lua::Bind()
#include "rpc_forward/rpc_router.h"  // for CRpcRouter

#include <LuaIntf/LuaIntf.h>

namespace {

using std::string;

CRpcRouter& GetRouter()
{
	return CRpcRouter::get_mutable_instance();
}

// return 0 if no route.
uint16_t GetDstSvrId(uint32_t uSessnId,
	const string& sService, const string& sMethod)
{
	return CRpcRouter::get_const_instance().GetDstSvrId(
		(uSessnId), sService, sMethod);
}

// 设置服务的功能服。功能服将动态选举。
void SetSvcFunction(const string& sService, const string& sFunction)
{
	GetRouter().SetSvcFunction(sService, sFunction);
}

// 重置服务的功能服。功能服不再动态选举。
void ResetSvcFunction(const string& sService)
{
	GetRouter().ResetSvcFunction(sService);
}

// 设置方法的功能服。功能服将动态选举。
void SetMthdFunction(const string& sService,
	const string& sMethod, const string& sFunction)
{
	GetRouter().SetMthdFunction(sService, sMethod, sFunction);
}

// 重置方法的功能服。功能服不再动态选举。
void ResetMthdFunction(const string& sService, const string& sMethod)
{
	GetRouter().ResetMthdFunction(sService, sMethod);
}

// 设置玩家的路由。不同玩家转发到不同Cell服。
void SetSvcDstSvrId(uint32_t uSessnId,
	const string& sService, uint16_t uSvrId)
{
	GetRouter().SetSvcDstSvrId((uSessnId), sService, uSvrId);
}

// 重置玩家的服务路由。
void ResetSvcDstSvrId(uint32_t uSessnId, const string& sService)
{
	GetRouter().ResetSvcDstSvrId((uSessnId), sService);
}

// 设置玩家服务方法路由
void SetMthdDstSvrId(uint32_t uSessnId, const string& sService,
	const string& sMethod, uint16_t uSvrId)
{
	GetRouter().SetMthdDstSvrId((uSessnId), sService, sMethod, uSvrId);
}

// 重置玩家服务方法路由
void ResetMthdDstSvrId(uint32_t uSessnId,
	const string& sService, const string& sMethod)
{
	GetRouter().ResetMthdDstSvrId((uSessnId), sService, sMethod);
}

}  // namespace

extern "C" int LuaOpenModule_RpcRouter(lua_State* L)
{
	assert(L);
	using namespace LuaIntf;
	LuaRef mod = LuaRef::createTable(L);
	LuaBinding(mod)
		.addFunction("get_dst_svr_id", &GetDstSvrId)
		.addFunction("set_svc_function", &SetSvcFunction)
		.addFunction("reset_svc_function", &ResetSvcFunction)
		.addFunction("set_mthd_function", &SetMthdFunction)
		.addFunction("reset_mthd_function", &ResetMthdFunction)
		.addFunction("set_svc_dst_svr_id", &SetSvcDstSvrId)
		.addFunction("reset_svc_dst_svr_id", &ResetSvcDstSvrId)
		.addFunction("set_mthd_dst_svr_id", &SetMthdDstSvrId)
		.addFunction("reset_mthd_dst_svr_id", &ResetMthdDstSvrId)
		;
	mod.pushToStack();
	return 1;
}

namespace LuaRpcRouter {

void Bind(lua_State* L)
{
	assert(L);
	::Lua::Bind(L, "c_rpc_router", LuaOpenModule_RpcRouter);
}

}  // namespace LuaRpcRouter

;
/// 获取目的服务器ID号.
// 无路由则返回0.
// @function get_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名，如"rpc.Login"
// @string method 方法名
// @treturn int 目的服务器ID号
;
/// 设置服务功能名.
// @function set_svc_function
// @string service 服务名
// @string function_string 功能名
;
/// 重置服务功能名.
// @function reset_svc_function
// @string service 服务名
;
/// 设置方法功能名.
// @function set_mthd_function
// @string service 服务名
// @string method 方法名
// @string funciton_string 功能名
;
/// 重置方法功能名.
// @function reset_mthd_function
// @string service 服务名
// @string method 方法名
;
/// 设置玩家服务目的服ID号.
// @function set_svc_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @int svr_id 目的服ID号
;
/// 重置玩家服务目的服ID号.
// @function reset_svc_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
;
/// 设置玩家服务方法目的服ID号.
// @function set_mthd_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @string method 方法名
// @int svr_id 目的服ID号
;
/// 重置玩家服务方法目的服ID号.
// @function reset_mthd_dst_svr_id
// @int session_id 玩家会话ID号
// @string service 服务名
// @string method 方法名
;
