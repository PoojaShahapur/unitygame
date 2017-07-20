#ifndef SVR_COMMON_ASIO_ASIO_SESSION_OUT_HEAD__
#define SVR_COMMON_ASIO_ASIO_SESSION_OUT_HEAD__

#include <memory>  // for enable_shared_from_this<>

#include <boost/asio/spawn.hpp>  // for yield_context
#include <boost/asio/ip/tcp.hpp>  // for socket

#include "asio_session_base.h"  // for CAsioSessionBase

class CRpcReqOutQueue;
class CAsioServer4S;

// Outgoing connection session.
// Server to server session.
// Server connect to other server.
// The remote server will accept this connect and create a CAsioSessionIn.
// The remote server will also create a CAsioSessionOut and connect this server.
// This server will also accept connection from remote server and create a CAsioSessionIn.
// There are 2 connections and 4 sessions between 2 servers.
class CAsioSessionOut :
    public std::enable_shared_from_this<CAsioSessionOut>,
    public CAsioSessionBase
{
public:
    CAsioSessionOut(CAsioServer4S& rServer, uint16_t uRemoteSvrId,
        const std::string& sRemoteHost, uint16_t uRemotePort);
    virtual ~CAsioSessionOut();

public:
    void Go();
    void End() { m_bRunning = false; }

public:
    uint16_t GetRemoteSvrId() const { return m_uRemoteSvrId; }
    const std::string& GetRemoteHost() const { return m_sRemoteHost; }
    uint16_t GetRemotePort() const { return m_uRemotePort; }

private:
    void LoopRead(boost::asio::yield_context yield);
    void LoopWrite(boost::asio::yield_context yield);
private:
    void Reconnect(boost::asio::yield_context yield);
    void WaitASec(boost::asio::yield_context yield);
    void Disconnect();
    std::string GetInfoStr() const;

private:
    std::unique_ptr<socket> m_pSocket;
    bool m_bRunning = true;

    // Remote server info:
    const uint16_t m_uRemoteSvrId;
    const std::string m_sRemoteHost;
    const uint16_t m_uRemotePort;
};  // class CAsioSessionOut

#endif  // SVR_COMMON_ASIO_ASIO_SESSION_OUT_HEAD__
