#ifndef SVR_COMMON_ASIO_ASIO_SERVER4C_HEAD__
#define SVR_COMMON_ASIO_ASIO_SERVER4C_HEAD__

#include "asio_server.h"  // for CAsioServer

class CRpcCallContext;

// Server for Clients.
class CAsioServer4C : public CAsioServer
{
public:
	explicit CAsioServer4C(io_service& rIos);
	virtual ~CAsioServer4C();

public:
	// 只有游戏客户端请求才会被转发
	void HandleForwardedRpcRequest(const CRpcCallContext& ctx,
		const std::string& sService, const std::string& sMethod,
		const std::string& sContent);
};

#endif  // SVR_COMMON_ASIO_ASIO_SERVER4C_HEAD__

