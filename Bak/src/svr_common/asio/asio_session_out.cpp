#include "asio_session_out.h"

#include <boost/asio/ip/tcp.hpp>  // for resolver
#include <boost/asio/connect.hpp>  // for async_connect()

#include "asio_event_handler.h"  // for OnS2sConnected()
#include "asio_server4s.h"  // for GetEventHandler()
#include "log.h"

const char LOG_NAME[] = "CAsioSessionOut";

CAsioSessionOut::CAsioSessionOut(CAsioServer4S& rServer, uint16_t uRemoteSvrId,
	const std::string& sRemoteHost, uint16_t uRemotePort)
	: CAsioSessionBase(rServer),
	m_uRemoteSvrId(uRemoteSvrId),
	m_sRemoteHost(sRemoteHost),
	m_uRemotePort(uRemotePort)
{
	LOG_DEBUG("CAsioSessionOut() " << GetInfoStr());
}

CAsioSessionOut::~CAsioSessionOut()
{
	// Todo: test destructed
	LOG_DEBUG("~CAsioSessionOut() " << GetInfoStr());
}

void CAsioSessionOut::Go()
{
	using boost::asio::spawn;
	using std::bind;
	using std::placeholders::_1;
	std::shared_ptr<CAsioSessionOut> pMe = shared_from_this();
	spawn(m_strand, bind(&CAsioSessionOut::LoopRead, pMe, _1));
	spawn(m_strand, bind(&CAsioSessionOut::LoopWrite, pMe, _1));
	LOG_DEBUG_TO("CAsioSessionOut.Go", GetInfoStr());
}

void CAsioSessionOut::LoopRead(boost::asio::yield_context yield)
{
	while (m_bRunning)
	{
		// Wait for (re)connected.
		while (!m_bConnected)
			WaitMs(1, yield);

		bool ok = ReadOne(*m_pSocket, yield);
		if (!ok)
			Disconnect();  // to reconnect
	}
}

void CAsioSessionOut::LoopWrite(boost::asio::yield_context yield)
{
	while (m_bRunning)
	{
		if (!m_pSocket)
		{
			Reconnect(yield);
			continue;
		}

		if (HaveNothingToSend())
		{
			WaitMs(1, yield);  // to yield
			continue;
		}

		bool ok = SendRpcs(*m_pSocket, yield);
		if (!ok)
			Disconnect();
	}
}

void CAsioSessionOut::Reconnect(boost::asio::yield_context yield)
{
	assert(!m_bConnected);
	assert(!m_pSocket);

	boost::system::error_code ec;
	using resolver = boost::asio::ip::tcp::resolver;
	resolver rslvr(m_strand.get_io_service());
	std::ostringstream oss;
	oss << m_uRemotePort;
	resolver::query qury(m_sRemoteHost, oss.str());
	resolver::iterator iter = rslvr.async_resolve(qury, yield[ec]);
	if (ec)
	{
		LOG_WARN_TO("CAsioSessionOut.Reconnect",
			Fmt("Failed to resolve: %s:%u, (%d)%s")
			% m_sRemoteHost % m_uRemotePort % ec.value() % ec.message());
		WaitASec(yield);
		return;
	}

	m_pSocket.reset(new socket(m_strand.get_io_service()));
	boost::asio::async_connect(*m_pSocket, iter, yield[ec]);
	if (!ec)
	{
		LOG_INFO_TO("CAsioSessionOut.Reconnect", "Connected. " << GetInfoStr()); 
		m_bConnected = true;
		m_rServer.GetEventHandler().OnS2sConnected(m_uRemoteSvrId);
		return;
	}
	m_pSocket.reset();

	WaitASec(yield);
}

// Wait 1s
void CAsioSessionOut::WaitASec(boost::asio::yield_context yield)
{
	WaitMs(1000, yield);
}

void CAsioSessionOut::Disconnect()
{
	LOG_INFO_TO("CAsioSessionOut.Disconnect", GetInfoStr());
	m_bConnected = false;
	m_pSocket.reset();
	m_rServer.GetEventHandler().OnS2sDisconnected(m_uRemoteSvrId);
}

std::string CAsioSessionOut::GetInfoStr() const
{
	return (Fmt("Session_%1%(Cell_%2%-%3%:%4%)") % GetSessionId()
		% m_uRemoteSvrId % m_sRemoteHost % m_uRemotePort).str();
}

