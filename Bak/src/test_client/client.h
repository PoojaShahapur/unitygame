#ifndef CLIENT_H
#define CLIENT_H

#include "protobuf_fwd.h"  // for Message
#include "rpc/rpc_callback.h"  // for RpcCallback
#include "rpc/rpc_data_vector.h"  // for RpcDataVector
#include "rpc/rpc_req_resp_fwd.h"  // for RpcResponse
#include "rpc/rpc_service_sptr.h"  // for RpcServiceSptr

#include <atomic>
#include <mutex>
#include <unordered_map>  // for unordered_map

#include <boost/asio.hpp>
#include <boost/asio/spawn.hpp>  // for yield_context

namespace ClientRpc {
class CRpcReqHandler;
}

struct lua_State;

class Client
{
public:
	explicit Client(lua_State* L, boost::asio::io_service& rIoService);
	~Client();

	// Thread-safe
	bool IsRunning() const { return is_running; }
public:
	// Thread-safe.
	using Callback = RpcCallback;
	void PushRpcReq(
		const std::string& sServiceName,
		const std::string& sMethod,
		const google::protobuf::Message& request,
		const Callback& cb);
	void PushRpcReq(
		const std::string& sServiceName,
		const std::string& sMethod,
		const std::string& sRequest,
		const Callback& cb);
	void PushRpcResp(uint32_t uRpcId, const std::string& sResponse);

private:
	void Spawn();

	using yield_context = boost::asio::yield_context;
	void LoopRead(yield_context yield);
	void LoopWrite(yield_context yield);

	void WaitMs(unsigned int uMilliSeconds, yield_context yield);
	void Disconnect();

	bool ReadOne(yield_context yield);
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
		DataVector v;  // 数据包
		BufVector vBuf;  // asio发送的缓冲区列表
	};

	// 填空发送缓冲区列表, 记录Rpc回调
	void PrepareRpcs(SendLenDataBuf& rSendBuf);

private:
	lua_State* luaState;
	boost::asio::io_service& io_service;
	boost::asio::ip::tcp::socket socket;
	boost::asio::ip::tcp::endpoint end_point;

	std::atomic_bool is_running;
	std::string cmd;
	std::atomic_bool has_cmd;

	using RpcCbMap = std::unordered_map<uint32_t, RpcCallback>;
	RpcCbMap rpc_cb_map;
	uint32_t rpc_id = 0;
	std::string transport_token;

	class SafeRpcDataVector
	{
	public:
		bool IsEmpty() { LockGuard ld(m_mtx); return m_v.empty(); }
		void Swap(RpcDataVector& rV) { LockGuard ld(m_mtx); rV.swap(m_v); }
		void Push(const RpcDataSptr& p) { LockGuard ld(m_mtx); m_v.push_back(p); }

	private:
		RpcDataVector m_v;  // 待发送Rpc列表
		std::mutex m_mtx;
		using LockGuard = std::lock_guard<std::mutex>;
	};
	SafeRpcDataVector outgoing_rpc_vec;
	
	std::unique_ptr<ClientRpc::CRpcReqHandler> m_pRpcReqHandler;
};

#endif  // CLIENT_H
