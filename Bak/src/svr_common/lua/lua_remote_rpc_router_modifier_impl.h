#ifndef SVR_COMMON_LUA_REMOTE_RPC_ROUTER_MODIFIER_IMPL_HEAD
#define SVR_COMMON_LUA_REMOTE_RPC_ROUTER_MODIFIER_IMPL_HEAD

#include <cstdint>  // for uint16_t
#include <functional>  // for function<>
#include <memory>  // for unique_ptr<>
#include <string>

class CRemoteRpcRouterModifier;

namespace LuaIntf {
class LuaRef;
}

namespace LuaRemoteRpcRouterModifierImpl {

// 用于导出 CRemoteRpcRouterModifier 到 lua.
// 需要处理int转SessionId, 及回调函数转换，所以添加一层封装。
class Modifier final
{
public:
	explicit Modifier(uint16_t uRemoteSvrId);

public:
	using string = std::string;
	using LuaRef = LuaIntf::LuaRef;

public:
	// 设置特定方法的路由，服务的路由不变.
	void SetMthdDstSvrId(uint32_t uSessnId, const string& sService,
		const string& sMethod, uint16_t uSvrId, const LuaRef& cb) const;
	void ResetMthdDstSvrId(uint32_t uSessnId, const string& sService,
		const string& sMethod, const LuaRef& cb) const;

	// 设置服务的路由，特定方法的路由不变。
	void SetSvcDstSvrId(uint32_t uSessnId, const string& sService,
		uint16_t uSvrId, const LuaRef& cb) const;
	void ResetSvcDstSvrId(uint32_t uSessnId, const string& sService,
		const LuaRef& cb) const;

private:
	using Callback = std::function<void()>;
	Callback ToCallback(const LuaRef& cb) const;

private:
	std::unique_ptr<CRemoteRpcRouterModifier> m_pRrrm;
};

}  // namespace LuaRemoteRpcRouterModifierImpl
#endif  // SVR_COMMON_LUA_REMOTE_RPC_ROUTER_MODIFIER_IMPL_HEAD
