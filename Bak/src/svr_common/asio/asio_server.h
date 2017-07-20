#ifndef SVR_COMMON_ASIO_ASIO_SERVER_HEAD__
#define SVR_COMMON_ASIO_ASIO_SERVER_HEAD__

#include <cstdint>  // for uint16_t

#include <boost/asio/spawn.hpp>  // for yield_context

#include "asio_fwd.h"  // for io_service
#include "asio_event_handler_sptr.h"  // for AsioEventHandlerSptr
#include "rpc/rpc_req_resp_fwd.h"  // for RpcRequest
#include "rpc/rpc_call_context.h"
#include "rpc/rpc_service_sptr.h"  // for RpcServiceSptr

class CAsioEventHandler;
class CRpcReqHandler;

class CAsioServer
{
public:
	using io_service = boost::asio::io_service;
	explicit CAsioServer(io_service& rIos);
	virtual ~CAsioServer();

public:
	void Init(uint16_t uPort);
	void Shutdown();

	io_service& GetIoService() { return m_rIoService; }

	// 注册服务。
	// sServiceName须包含包名，如"rpc.TestSvc".
	// pService允许为空，表示忽略该服务。
	void RegisterService(const std::string& sServiceName,
		RpcServiceSptr pService);
	void RegisterService(RpcServiceSptr pService);

	// 设置事件处理器
	void SetEventHandler(const AsioEventHandlerSptr& pHandler);
	CAsioEventHandler& GetEventHandler() const;

	void HandleRpcRequest(uint64_t uRpcCltId, const rpc::RpcRequest& req);
	void HandleRpcRequestInLua(const CRpcCallContext& ctx,
		const std::string& sService, const std::string& sMethod,
		const std::string& sContent);

	// 安全传输功能是否启用
	bool isSecureTransportEnabled() {
		return m_bSecureTransport;
	}
protected:
	void setSecureTransportEnabled(bool bEnabled) {
		m_bSecureTransport = bEnabled;
	}
private:
	void LoopAccept(unsigned short port, boost::asio::yield_context yield);

protected:
	std::unique_ptr<CRpcReqHandler> m_pRpcReqHandler;

private:
	io_service& m_rIoService;
	bool m_bShutdown = false;
	AsioEventHandlerSptr m_pEventHandler;
	bool m_bSecureTransport = false;
};

#endif  // SVR_COMMON_ASIO_ASIO_SERVER_HEAD__

