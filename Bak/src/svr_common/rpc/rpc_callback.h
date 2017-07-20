#ifndef SVR_COMMON_RPC_RPC_CALLBACK_H
#define SVR_COMMON_RPC_RPC_CALLBACK_H

#include <functional>
#include <string>

using RpcCallback = std::function<void(const std::string& sResponseContent)>;

#endif  // SVR_COMMON_RPC_RPC_CALLBACK_H
