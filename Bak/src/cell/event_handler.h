#ifndef CELL_EVENT_HANDLER_H
#define CELL_EVENT_HANDLER_H

#include "asio/asio_event_handler.h"  // for CAsioEventHandler

class CEventToLua;

// 为了让应用与网络库解偶，实现一个CAsioEventHandler类来处理各种事件。
class CEventHandler : public CAsioEventHandler
{
public:
	CEventHandler();
	virtual ~CEventHandler();

public:
	// 事件处理函数，缺省都为空操作。
	virtual void OnClientConnected(uint64_t uCltId,
		const std::string& sCltAddr, uint16_t uCltPort) override;
	void OnClientDisconnected(uint64_t uCltId) override;
	void OnS2sDisconnected(uint16_t uSvrId) override;
	void OnS2sConnected(uint16_t uSvrId) override;

private:
	std::unique_ptr<CEventToLua> m_pEventToLua;
};  // EventHandler

#endif  // CELL_EVENT_HANDLER_H

