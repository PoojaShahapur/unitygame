#include "rpc_data.h"

#include <google/protobuf/message.h>

const char LOG_NAME[] = "CRpcData";

CRpcData::CRpcData(const google::protobuf::Message& message)
	: m_sMessage(message.SerializeAsString())
{
}
