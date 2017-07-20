#include "rpc_resp_data.h"

#include "log.h"
#include "pb/rpc/rpc.pb.h"

const char LOG_NAME[] = "CRpcRespData";

void CRpcRespData::SerializeToString(uint32_t uReqId,
	std::string& sOutput) const
{
	(void)uReqId;

	rpc::RpcPackage pkg;
	rpc::RpcResponse* pResp = pkg.mutable_response();
	pResp->set_id(m_uRpcId);
	pResp->set_content(GetRpcMessage());
	if (!pkg.SerializeToString(&sOutput))
	{	
		LOG_WARN("Failed to serialize RpcPackage message.");
	}
}

