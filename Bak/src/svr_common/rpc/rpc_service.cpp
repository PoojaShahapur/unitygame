#include "rpc_service.h"

#include "log.h"

void CRpcService::CallMethod(const CRpcCallContext& ctx,
	const std::string& sMethod, const std::string& sContent)
{
	auto itr = m_mapHandler.find(sMethod);
	if (itr == m_mapHandler.end())
	{
		LOG_WARN_TO("CRpcService.CallMethod", Fmt("Illegal method: %s.%s()")
			% GetServiceName() % sMethod);
		return;
	}
	const MethodHandler& handler = (*itr).second;
	if (handler)  // 可以设为空来忽略请求
		handler(ctx, sContent);
}

// Todo: Change all RegisterMethod(..., handler(uint64_t uCltId, ...)), not "int nCltFd"

void CRpcService::RegisterMethod(const std::string& sMethod,
	const MethodHandler& handler)
{
	if (m_mapHandler.find(sMethod) != m_mapHandler.end())
	{
		LOG_WARN_TO("CRpcService.RegisterMethod",
			Fmt("Repetition of method: %s:%s()")
			% GetServiceName() % sMethod);
	}
	m_mapHandler[sMethod] = handler;
}

