#ifndef SVR_COMMON_RPC_FORWARD_RPC_FORWARDER_HEAD
#define SVR_COMMON_RPC_FORWARD_RPC_FORWARDER_HEAD

#include <cstdint>  // for uint16_t
#include <string>

class CRpcCallContext;

namespace RpcForwarder {
void ForwardTo(uint16_t uCellId, const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent);
}  // namespace RpcForwarder

#endif  // SVR_COMMON_RPC_FORWARD_RPC_FORWARDER_HEAD
