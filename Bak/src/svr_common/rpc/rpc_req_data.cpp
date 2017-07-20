#include "rpc_req_data.h"

#include "log.h"
#include "pb/rpc/rpc.pb.h"

const char LOG_NAME[] = "CRpcReqData";

void CRpcReqData::SerializeToString(uint32_t uReqId,
	std::string& sOutput) const
{
	rpc::RpcPackage pkg;
	rpc::RpcRequest* pReq = pkg.mutable_request();
	pReq->set_id(uReqId);
	pReq->set_service(m_sService);
	pReq->set_method(m_sMethod);
	pReq->set_content(GetRpcMessage());
	if (!pkg.SerializeToString(&sOutput))
		LOG_WARN("Failed to serialize RpcPackage message.");
}
