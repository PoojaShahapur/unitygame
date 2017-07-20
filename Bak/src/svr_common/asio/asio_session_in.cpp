// Asio server session.
// Author: Jin Qing (http://blog.csdn.net/jq0123)

#include "asio_session_in.h"

#include "asio_event_handler.h"  // for OnClientDisconnected()
#include "asio_server.h"  // for InsertSession()
#include "asio_server4c.h"  // for CAsioServer4C
#include "asio_session_in_mgr.h"  // for CAsioSessionInMgr
#include "log.h"
#include "pb/rpc/first_msg.pb.h"
#include "rpc/rpc_resp_data.h"  // for CRpcRespData
#include "util.h"  // for Util

#include <google/protobuf/message.h>  // for Message
#include <chrono>
#include <boost/asio/write.hpp>  // for async_write()

const uint32_t SERVER_VERSION = 20113;

const char LOG_NAME[] = "CAsioSessionIn";

inline CAsioSessionInMgr& GetMgr()
{
	return CAsioSessionInMgr::get_mutable_instance();
}

CAsioSessionIn::CAsioSessionIn(CAsioServer& rServer)
	: CAsioSessionBase(rServer),
	m_socket(rServer.GetIoService())
{
	GetMgr().InsertSession(*this);
}

CAsioSessionIn::~CAsioSessionIn()
{
	GetMgr().EraseSession(*this);
	LOG_DEBUG(Fmt("~CAsioSessionIn() Id=%u(%s:%u)")
		% GetSessionId() % m_sCltAddr % m_uCltPort);
}

void CAsioSessionIn::Disconnect()
{
	if (!m_bConnected)
		return;

	// Todo（Jinq): 须延时到所有发送完成，如重复登录时须发完后再断开。
	boost::system::error_code ec;
	m_socket.close(ec);
	m_bConnected = false;
	GetSendingRpcVec().clear();
	LOG_INFO(Fmt("Disconnected. Id=%u(%s:%u)")
		% GetSessionId() % m_sCltAddr % m_uCltPort);
	m_rServer.GetEventHandler().OnClientDisconnected(GetSessionId());
}

void CAsioSessionIn::Go()
{
	if (!InitOnConnect())
		return;

	if (m_rServer.isSecureTransportEnabled())
	{
		using namespace std::chrono;
		auto tp = system_clock::now(); 
		uint32_t cur_time = duration_cast<seconds>(tp.time_since_epoch()).count();
		
		rpc::FirstMsg msg;
		m_strSecureTransportToken = Util::GenerateRandToken();
		msg.set_token(m_strSecureTransportToken.c_str(), m_strSecureTransportToken.size());
		msg.set_time(cur_time);
		msg.set_version(SERVER_VERSION);

		const std::string& msgString = msg.SerializeAsString();
		uint32_t msgSize = msgString.size();
		uint32_t netSize = htonl(msgSize);

		std::vector<boost::asio::const_buffer> sendBuf;
		sendBuf.emplace_back(&netSize, sizeof(netSize));
		sendBuf.emplace_back(msgString.c_str(), msgSize);

		boost::asio::async_write(m_socket, sendBuf, [](boost::system::error_code ec, size_t sentSize) {
			if (ec)
			{
				LOG_WARN_TO("CAsioSessionIn.Go",
					Fmt("Failed to send the FirstMsg: (%d)%s") % ec.value() % ec.message());
			}
			//m_rServer.setSecureTransportEnabled(false);
		});
	}

	using boost::asio::spawn;
	using std::bind;
	using std::placeholders::_1;
	std::shared_ptr<CAsioSessionIn> pMe = shared_from_this();
	spawn(m_strand, bind(&CAsioSessionIn::LoopRead, pMe, _1));
	spawn(m_strand, bind(&CAsioSessionIn::LoopWrite, pMe, _1));
}

void CAsioSessionIn::PushRpcResp(uint32_t uRpcId,
	const google::protobuf::Message& response)
{
	RpcDataSptr pRpc = std::make_shared<CRpcRespData>(
		uRpcId, response.SerializeAsString());
	PushRpcData(pRpc);
}

void CAsioSessionIn::PushRpcResp(uint32_t uRpcId,
	const std::string& sResponse)
{
	RpcDataSptr pRpc = std::make_shared<CRpcRespData>(
		uRpcId, sResponse);
	PushRpcData(pRpc);
}

bool CAsioSessionIn::IsGameClient() const
{
	// 如果m_rServer是CAsioServer4C说明是游戏客户端。
	return nullptr != dynamic_cast<const CAsioServer4C*>(&m_rServer);
}

bool CAsioSessionIn::InitOnConnect()
{
	boost::system::error_code ec;
	boost::asio::ip::tcp::endpoint endpoint = m_socket.remote_endpoint(ec);
	if (ec)
	{
		LOG_DEBUG(Fmt("Failed to get remote endpoint: (%d)%s")
			% ec.value() % ec.message());
		return false;
	}

	m_sCltAddr = endpoint.address().to_string();
	m_uCltPort = endpoint.port();
	LOG_INFO_TO("CAsioSessionIn.Go",
		Fmt("Accepted connection. Remote=%s:%u. CltId=%u.")
		% m_sCltAddr % m_uCltPort % GetSessionId());
	m_rServer.GetEventHandler().OnClientConnected(GetSessionId(),
		m_sCltAddr, m_uCltPort);
	m_bConnected = true;
	return true;
}

void CAsioSessionIn::LoopRead(boost::asio::yield_context yield)
{
	assert(m_bConnected);
	while (ReadOne(m_socket, yield))
		;

	Disconnect();
}

void CAsioSessionIn::LoopWrite(boost::asio::yield_context yield)
{
	assert(m_bConnected);
	while (m_bConnected)
	{
		if (HaveNothingToSend())
		{
			WaitMs(1, yield);
			continue;
		}

		bool ok = SendRpcs(m_socket, yield);
		if (!ok)
		{
			Disconnect();
			break;
		}
	}
	LOG_DEBUG_TO("CAsioSessionIn.LoopWrite", "End.");
}
