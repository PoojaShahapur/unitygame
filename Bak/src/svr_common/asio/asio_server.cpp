/*----------------------------------------------------------------
// Copyright (C) 2016 巨人网络
//
// 模块名：asio_server
// 创建者：Jin Qing (http://blog.csdn.net/jq0123)
// 修改者列表：
// 创建日期：2016.5.14
// 模块描述：Asio服务器
//----------------------------------------------------------------*/

#include "asio_server.h"

#include "asio_event_handler.h"  // for CAsioEventHandler
#include "asio_session_in.h"  // for CAsioSessionIn
#include "log.h"
#include "rpc/rpc_req_handler.h"  // for CRpcReqHandler
#include "rpc/rpc_call_context.h"//for CRpcCallContext

const char LOG_NAME[] = "CAsioServer";

CAsioServer::CAsioServer(io_service& rIos)
	: m_rIoService(rIos),
	m_pRpcReqHandler(new CRpcReqHandler)
{
}

CAsioServer::~CAsioServer()
{
}

// Accept coroutine.
void CAsioServer::LoopAccept(unsigned short port,
	boost::asio::yield_context yield)
{
	using boost::asio::ip::tcp;
	tcp::acceptor acceptor(m_rIoService,
		tcp::endpoint(tcp::v4(), port));

	for (;;)
	{
		boost::system::error_code ec;
		std::shared_ptr<CAsioSessionIn> pSession(new CAsioSessionIn(*this));
		acceptor.async_accept(pSession->GetSocket(), yield[ec]);
		if (ec)
			continue;
		pSession->Go();
	}
}

// 注册服务。
// sServiceName须包含包名，如"rpc.TestSvc".
// pService允许为空，表示忽略该服务。
void CAsioServer::RegisterService(
	const std::string& sServiceName,
	RpcServiceSptr pService)
{
	m_pRpcReqHandler->RegisterService(sServiceName, pService);
}

void CAsioServer::RegisterService(RpcServiceSptr pService)
{
	m_pRpcReqHandler->RegisterService(pService);
}

void CAsioServer::SetEventHandler(const AsioEventHandlerSptr& pHandler)
{
	m_pEventHandler = pHandler;
}

CAsioEventHandler& CAsioServer::GetEventHandler() const
{
	if (m_pEventHandler)
		return *m_pEventHandler;

	// 缺省为空处理器。
	static CAsioEventHandler s_handler;
	return s_handler;
}

void CAsioServer::HandleRpcRequest(uint64_t uRpcCltId,
	const rpc::RpcRequest& req)
{
	m_pRpcReqHandler->HandleRpcRequest(uRpcCltId, req);
}

void CAsioServer::HandleRpcRequestInLua(const CRpcCallContext& ctx,
	const std::string& sService, const std::string& sMethod,
	const std::string& sContent)
{
    m_pRpcReqHandler->HandleRpcRequestInLua(ctx, sService, sMethod, sContent);
}

void CAsioServer::Init(uint16_t uPort)
{
	// Todo: 绑定监听IP
	LOG_INFO("Listen on port " << uPort);
	// See: doc/html/boost_asio/example/cpp11/spawn/echo_server.cpp
	using namespace boost::asio;
	spawn(m_rIoService, [this, uPort](yield_context yield){
		LoopAccept(uPort, yield);
	});
}

void CAsioServer::Shutdown()
{
	m_bShutdown = true;
	LOG_INFO("Shutdown()");
}
