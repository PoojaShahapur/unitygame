#ifndef SVR_COMMON_ASIO_ASIO_SERVER4S_HEAD__
#define SVR_COMMON_ASIO_ASIO_SERVER4S_HEAD__

#include "asio_server.h"  // for CAsioServer

// Server for Servers.
class CAsioServer4S : public CAsioServer
{
public:
	explicit CAsioServer4S(io_service& rIos);
	virtual ~CAsioServer4S();
};

#endif  // SVR_COMMON_ASIO_ASIO_SERVER4S_HEAD__

