// Asio server session.
// Author: Jin Qing (http://blog.csdn.net/jq0123)

#ifndef SVR_COMMON_ASIO_ASIO_SESSION_IN_H
#define SVR_COMMON_ASIO_ASIO_SESSION_IN_H

#include <memory>  // for enable_shared_from_this<>

#include "asio_session_base.h"  // for CAsioSessionBase
#include "rpc/rpc_req_resp_fwd.h"  // for RpcResponse

// Incoming connection session.
// Server accepted session.
// The remote peer is client or server.
class CAsioSessionIn :
	public std::enable_shared_from_this<CAsioSessionIn>,
	public CAsioSessionBase
{
public:
	explicit CAsioSessionIn(CAsioServer& rServer);
	virtual ~CAsioSessionIn();

public:
	socket& GetSocket()
	{
		return m_socket;
	}
	void Disconnect();
	void Go();

	// CAsioSessionOut should not response, only request.
	// Only CAsioSessionIn can response Rpc.
	void PushRpcResp(uint32_t uRpcId, const google::protobuf::Message& response);
	void PushRpcResp(uint32_t uRpcId, const std::string& sResponse);

	// 对方是否游戏客户端。有可能是服务器连接。
	bool IsGameClient() const;

private:
	bool InitOnConnect();
	void LoopRead(boost::asio::yield_context yield);
	void LoopWrite(boost::asio::yield_context yield);

private:
	void HandleData(const char* pData, uint32_t uLen);
	void HandleResponse(const rpc::RpcResponse& response);

private:
	// 添加欲发送的消息到队列中
	void PushRpcData(const RpcDataSptr& pRpc)
	{
		assert(pRpc);
		GetSendingRpcVec().emplace_back(pRpc);
	}

private:
	socket m_socket;

	std::string m_sCltAddr;
	uint16_t m_uCltPort = 0;
};

#endif  // SVR_COMMON_ASIO_ASIO_SESSION_IN_H
