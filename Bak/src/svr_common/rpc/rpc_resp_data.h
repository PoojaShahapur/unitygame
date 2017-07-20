#ifndef SVR_COMMON_RPC_RPC_RESP_DATA_H
#define SVR_COMMON_RPC_RPC_RESP_DATA_H

#include <string>
#include <functional>
#include "rpc_data.h"  // for CRpcData

// 异步Rpc数据
class CRpcRespData : public CRpcData
{
public:
	CRpcRespData(uint32_t uRpcId, const std::string& sMessage)
		: CRpcData(sMessage),
		  m_uRpcId(uRpcId)
	{};
	virtual ~CRpcRespData() {}

public:
	void SerializeToString(uint32_t uReqId, std::string& sOutput) const override;

private:
	uint32_t m_uRpcId;
};

#endif  // SVR_COMMON_RPC_RPC_RESP_DATA_H
