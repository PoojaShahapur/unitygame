#ifndef SVR_COMMON_RPC_RPC_DATA_SPTR_H
#define SVR_COMMON_RPC_RPC_DATA_SPTR_H

#include <memory>

// 异步Rpc数据
class CRpcData;

using RpcDataSptr = std::shared_ptr<CRpcData>;

#endif  // SVR_COMMON_RPC_RPC_DATA_SPTR_H
