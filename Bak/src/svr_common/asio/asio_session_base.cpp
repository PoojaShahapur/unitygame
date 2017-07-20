// Asio session base class.
// Author: Jin Qing (http://blog.csdn.net/jq0123)

#include "asio_session_base.h"

#ifdef _WIN32
#include <winsock2.h>  // for ntohl()
#else
#include <arpa/inet.h>  // for ntohl()
#endif  // _WIN32

#include <boost/asio/buffer.hpp>
#include <boost/asio/read.hpp>
#include <boost/asio/steady_timer.hpp>  // for steady_timer
#include <boost/asio/write.hpp>  // for async_write()

#include "asio_server.h"  // for CAsioServer
#include "log.h"
#include "pb/rpc/rpc.pb.h"  // for RpcPackage
#include "rpc/rpc_data.h"  // for CRpcData
#include "rpc/rpc_req_data.h"  // for CRpcReqData
#include "util.h"

const uint32_t MAX_DATA_LEN = 1024 * 1024;
const char LOG_NAME[] = "CAsioSessionBase";

// Sequence id generator.
static uint64_t s_uIdGenerator = 1;  // 0 is illegal.

CAsioSessionBase::CAsioSessionBase(CAsioServer& rServer) :
	m_rServer(rServer),
	m_strand(rServer.GetIoService()),
	m_uSessionId(s_uIdGenerator++),
	m_strSecureTransportToken("default_token")
{
}

CAsioSessionBase::~CAsioSessionBase()
{
}

void CAsioSessionBase::PushRpcReq(
	const std::string& sServiceName,
	const std::string& sMethod,
	const google::protobuf::Message& request,
	const RpcCallback& cb)
{
	RpcDataSptr pRpc = std::make_shared<CRpcReqData>(
		sServiceName, sMethod, request.SerializeAsString(), cb);
	m_vSendingRpc.emplace_back(pRpc);
}

void CAsioSessionBase::PushRpcReq(
	const std::string& sServiceName,
	const std::string& sMethod,
	const std::string& sRequest,
	const RpcCallback& cb)
{
	RpcDataSptr pRpc = std::make_shared<CRpcReqData>(
		sServiceName, sMethod, sRequest, cb);
	m_vSendingRpc.emplace_back(pRpc);
}

bool CAsioSessionBase::ReadOne(socket& rSocket, yield_context yield)
{
	if (!m_bConnected)  // 客户端已断开
	{
		LOG_DEBUG_TO("CAsioSessionBase.ReadOne", "Client is disconnected.");
		return false;
	}
	ReadBuf buf;
	uint32_t dwLenNet;  // 数据包长度, 网络字节序
	using namespace boost::asio;
	boost::system::error_code ec;
	std::size_t uReadHead = async_read(rSocket,
		buffer(&dwLenNet, sizeof(dwLenNet)), yield[ec]);
	if (ec)
	{
		LOG_INFO_TO("CAsioSessionBase.Read", Fmt("Failed to read len: (%d)%s")
			% ec.value() % ec.message());
		return false;
	}
	assert(uReadHead == sizeof(dwLenNet));
	uint32_t dwLenHost = ntohl(dwLenNet);
	if (dwLenHost > MAX_DATA_LEN)
	{
		LOG_WARN_TO("CAsioSessionBase.Read", "Illegal len: " << dwLenHost);
		return false;
	}
	// LOG_DEBUG_TO("CAsioSessionBase.Read", "Got head len: " << dwLenHost);
	if (0 == dwLenHost)
		return true;

	buf.resize(dwLenHost);
	std::size_t uReadData = async_read(rSocket,
		buffer(&buf[0], dwLenHost), yield[ec]);
	if (ec)
	{
		LOG_INFO_TO("CAsioSessionBase.Read", Fmt("Failed to read data: (%d)%s")
			% ec.value() % ec.message());
		return false;
	}
	assert(uReadData == dwLenHost);

	if (m_rServer.isSecureTransportEnabled())
	{
		Util::Encrypt_XOR(&buf[0], dwLenHost, m_strSecureTransportToken.c_str(), m_strSecureTransportToken.size());
	}

	HandleData(buf);
	return true;
}

void CAsioSessionBase::WaitMs(unsigned int uMilliSeconds,
	boost::asio::yield_context yield)
{
	boost::asio::steady_timer timer(m_strand.get_io_service());
	timer.expires_from_now(std::chrono::milliseconds(uMilliSeconds));
	boost::system::error_code ec;
	timer.async_wait(yield[ec]);
	if (!ec) return;
	LOG_WARN_TO("CAsioSessionBase.WaitMs", Fmt("Async wait error: (%d)%s")
		% ec.value() % ec.message());
}

bool CAsioSessionBase::SendRpcs(socket& rSocket,
	boost::asio::yield_context yield)
{
	assert(m_bConnected);
	RpcDataVector vRpc;  // 须保留到async_write()结束
	m_vSendingRpc.swap(vRpc);  // m_vSendingRpc 会在其他协程中更改
	// Todo(jinq): 数据太多时警告

	SendLenDataBuf sendBuf;
	PrepareRpcs(vRpc, sendBuf);

	boost::system::error_code ec;
	boost::asio::async_write(rSocket, sendBuf.vBuf, yield[ec]);
	if (!ec)
		return true;
	LOG_WARN_TO("CAsioSessionBase.SendRpcs",
		Fmt("Failed to send: (%d)%s") % ec.value() % ec.message());
	return false;
}

void CAsioSessionBase::HandleData(const ReadBuf& rBuf)
{
	assert(!rBuf.empty());
	rpc::RpcPackage rpcPkg;
	bool ok = rpcPkg.ParseFromArray(&rBuf[0], rBuf.size());
	if (!ok)
	{
		LOG_WARN_TO("CAsioSessionBase.HandleData",
			"Got illegal RpcPackage, len=" << rBuf.size());
		return;
	}

	if (rpcPkg.has_request())
		m_rServer.HandleRpcRequest(GetSessionId(), rpcPkg.request());
	if (rpcPkg.has_response())
		HandleResponse(rpcPkg.response());
}

void CAsioSessionBase::HandleResponse(const rpc::RpcResponse& response)
{
	auto itr = m_mapRpcCb.find(response.id());
	if (itr == m_mapRpcCb.end())
		return;
	RpcCallback cb = (*itr).second;
	m_mapRpcCb.erase(itr);
	assert(cb);
	cb(response.content());
}

// 填空发送缓冲区列表, 记录Rpc回调
void CAsioSessionBase::PrepareRpcs(
	const RpcDataVector& vRpc, // 待发送Rpc列表
	SendLenDataBuf& rSendBuf)
{
	LenVector& rLenVec = rSendBuf.vLenNet;  // 包长（网络序）列表
	DataVector& rDataVec = rSendBuf.vData;  // 数据包
	BufVector& rBufVec = rSendBuf.vBuf;

	size_t uSize = vRpc.size();
	rLenVec.resize(uSize);
	rDataVec.resize(uSize);
	for (size_t i = 0; i < uSize; i++)
	{
		assert(vRpc[i]);
		const CRpcData& rpc = *vRpc[i];
		std::string& rDataStr = rDataVec[i];

		uint32_t uRpcId = m_uRpcId++;  // 即使是response也消耗一个
		rpc.SerializeToString(uRpcId, rDataStr);
		uint32_t uLenHost = rDataStr.size();
		if (0 == uLenHost)
			continue;

		// response 无回调
		const RpcCallback& cb = rpc.GetCallback();
		if (cb)
			m_mapRpcCb[uRpcId] = cb;

		uint32_t& rLenNet = rLenVec[i];
		rLenNet = htonl(uLenHost);

		if (m_rServer.isSecureTransportEnabled())
		{
			Util::Encrypt_XOR(&rDataStr[0], uLenHost, m_strSecureTransportToken.c_str(), m_strSecureTransportToken.size());
		}

		using boost::asio::const_buffer;
		rBufVec.emplace_back(
			const_buffer(&rLenNet, sizeof(rLenNet)));
		rBufVec.emplace_back(
			const_buffer(rDataStr.data(), uLenHost));
	}
}

