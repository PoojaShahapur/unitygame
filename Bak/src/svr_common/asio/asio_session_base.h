// Asio session base class.
// Author: Jin Qing (http://blog.csdn.net/jq0123)

#ifndef SVR_COMMON_ASIO_ASIO_SESSION_BASE_H
#define SVR_COMMON_ASIO_ASIO_SESSION_BASE_H

#include <functional>
#include <unordered_map>  // for unordered_map

#include <boost/asio/ip/tcp.hpp>  // for socket
#include <boost/asio/spawn.hpp>  // for yield_context
#include <boost/asio/strand.hpp>
#include <boost/noncopyable.hpp>

#include "protobuf_fwd.h"  // for Message
#include "rpc/rpc_callback.h"  // for RcpCallback
#include "rpc/rpc_data_vector.h"  // for RpcDataVector
#include "rpc/rpc_req_resp_fwd.h"  // for RpcResponse

class CRpcReqOutQueue;
class CAsioServer;

// Base class for CAsionSessionIn and CAsioSessionOut.
class CAsioSessionBase : boost::noncopyable
{
public:
	explicit CAsioSessionBase(CAsioServer& rServer);
	virtual ~CAsioSessionBase();

public:
	uint64_t GetSessionId() const { return m_uSessionId; }

public:
	// CAsionSessionIn and CAsioSessionOut both need to request rpc.
	// But CAsioSessionOut do not response (one-way rpc).
	void PushRpcReq(const std::string& sServiceName,
		const std::string& sMethod,
		const google::protobuf::Message& request,
		const RpcCallback& cb = RpcCallback());
	void PushRpcReq(const std::string& sServiceName,
		const std::string& sMethod,
		const std::string& sRequest,
		const RpcCallback& cb = RpcCallback());

public:
	using socket = boost::asio::ip::tcp::socket;

protected:
	using yield_context = boost::asio::yield_context;
	bool SendRpcs(socket& rSocket, yield_context yield);
	bool ReadOne(socket& rSocket, yield_context yield);

	void WaitMs(unsigned int uMilliSeconds, yield_context yield);

	RpcDataVector& GetSendingRpcVec() { return m_vSendingRpc; }
	bool HaveNothingToSend() const { return m_vSendingRpc.empty(); }

private:
	using ReadBuf = std::vector<char>;
	void HandleData(const ReadBuf& rBuf);
	void HandleResponse(const rpc::RpcResponse& response);

private:
	using LenVector = std::vector<uint32_t>;
	using DataVector = std::vector<std::string>;
	using BufVector = std::vector<boost::asio::const_buffer>;
	struct SendLenDataBuf
	{
		LenVector vLenNet;  // 包长（网络序）列表
		DataVector vData;  // 数据包
		BufVector vBuf;  // asio发送的缓冲区列表
	};

	// 填空发送缓冲区列表, 记录Rpc回调
	void PrepareRpcs(const RpcDataVector& vRpc, // 待发送Rpc列表
		SendLenDataBuf& rSendBuf);

protected:
	CAsioServer& m_rServer;
	boost::asio::io_service::strand m_strand;
	bool m_bConnected = false;
	std::string m_strSecureTransportToken;

private:
	using RpcCbMap = std::unordered_map<uint32_t, RpcCallback>;
	RpcCbMap m_mapRpcCb;
	uint32_t m_uRpcId = 0;
	const uint64_t m_uSessionId;
	RpcDataVector m_vSendingRpc;  // 待发送Rpc列表
};

#endif  // SVR_COMMON_ASIO_ASIO_SESSION_BASE_H
