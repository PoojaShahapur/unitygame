#ifndef SVR_COMMON_ASIO_ASIO_EVENT_HANDLER_H
#define SVR_COMMON_ASIO_ASIO_EVENT_HANDLER_H

#include "asio_event_handler_sptr.h"

#include <cstdint>  // for uint64_t
#include <string>

// 为了让应用与网络库解偶，实现一个CAsioEventHandler类并设置，用来处理各种事件。
class CAsioEventHandler
{
public:
	CAsioEventHandler() {};
	virtual ~CAsioEventHandler() {};

public:
	// 事件处理函数，缺省都为空操作。
	virtual void OnClientConnected(uint64_t uCltId,
		const std::string& sCltAddr, uint16_t uCltPort) {};
	virtual void OnClientDisconnected(uint64_t uCltId) {};
	virtual void OnS2sDisconnected(uint16_t uSvrId) {};
	virtual void OnS2sConnected(uint16_t uSvrId) {};
};  // CAsioEventHandler

#endif  // SVR_COMMON_ASIO_ASIO_EVENT_HANDLER_H

