#ifndef SVR_COMMON_RPC_RPC_REQ_HANDLER_H
#define SVR_COMMON_RPC_RPC_REQ_HANDLER_H

#include "rpc/rpc_req_resp_fwd.h"  // for RpcRequest
#include "rpc/rpc_service_sptr.h"  // for RpcServiceSptr

#include <cstdint>
#include <unordered_map>

struct lua_State;

namespace ClientRpc {

class CRpcReqHandler final
{
public:
	explicit CRpcReqHandler(lua_State* L);
	~CRpcReqHandler();

public:
	void HandleRpcRequest(const rpc::RpcRequest& req);

private:
	void HandleRpcRequestInLua(uint32_t uRpcId,
		const std::string& sService, const std::string& sMethod,
		const std::string& sContent);

private:
	lua_State* m_luaState;
};

}  // namespace ClientRpc
#endif  // SVR_COMMON_RPC_RPC_REQ_HANDLER_H
