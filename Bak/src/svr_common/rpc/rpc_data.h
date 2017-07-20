#ifndef SVR_COMMON_RPC_RPC_DATA_H
#define SVR_COMMON_RPC_RPC_DATA_H

#include <cassert>
#include <functional>
#include <string>

#include "protobuf_fwd.h"  // for Message
#include "rpc_callback.h"  // for RpcCallback

// 异步Rpc数据, 基类，用于派生请求和应答。
class CRpcData
{
public:
	explicit CRpcData(const google::protobuf::Message& message);
	explicit CRpcData(const std::string& sMessage) : m_sMessage(sMessage) {};
	virtual ~CRpcData() {}

public:
	using Callback = RpcCallback;

public:
	virtual void SerializeToString(uint32_t uReqId, std::string& sOutput) const
	{
	}
	virtual Callback GetCallback() const { return Callback(); }
	const std::string& GetRpcMessage() const { return m_sMessage; }

private:
	const std::string m_sMessage;
};

#endif  // SVR_COMMON_RPC_RPC_DATA_H
