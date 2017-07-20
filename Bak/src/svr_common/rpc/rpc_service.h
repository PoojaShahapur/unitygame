#ifndef SVR_COMMON_RPC_RPC_SERVICE_H
#define SVR_COMMON_RPC_RPC_SERVICE_H

#include <cstdint>  // for uint32_t
#include <functional>
#include <string>
#include <unordered_map>

class CRpcCallContext;

// The rpc service base class.
class CRpcService
{
public:
	CRpcService() {};
	virtual ~CRpcService() {};

public:
	// sContent is serialized request message.
	void CallMethod(const CRpcCallContext& ctx,
		const std::string& sMethod, const std::string& sContent);

	using MethodHandler = std::function<void (const CRpcCallContext&, const std::string&)>;
	void RegisterMethod(const std::string& sMethod, const MethodHandler& handler);

public:
	// 服务名，对应proto文件中的service, 带包名, 如: "rpc.Login"。
	virtual std::string GetServiceName() const = 0;

private:
	using HandlerMap = std::unordered_map<std::string, MethodHandler>;
	HandlerMap m_mapHandler;
};  // class CRpcService

#endif  // SVR_COMMON_RPC_RPC_SERVICE_H
