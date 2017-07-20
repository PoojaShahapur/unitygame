#include "client.h"

#include <iostream>

#include <boost/asio/spawn.hpp>
#include <boost/system/error_code.hpp>
#include <boost/asio/steady_timer.hpp>

#include "client_rpc/rpc_req_handler.h"  // for CRpcReqHandler
#include "fmt.h"
#include "pb/rpc/rpc.pb.h"
#include "pb/rpc/first_msg.pb.h"
#include "rpc/rpc_data_sptr.h"
#include "rpc/rpc_req_data.h"
#include "rpc/rpc_resp_data.h"
#include "csv/csv_cfg.h"
#include "csv/csv_table.h"
#include "csv/csv_record.h"
#include "util.h"

using namespace std;

const uint32_t MAX_DATA_LEN = 1024 * 1024;

Client::Client(lua_State* L, boost::asio::io_service& rIoService) :
	luaState(L),
	io_service(rIoService),
	socket(io_service),
	is_running(true),
	has_cmd(false),
	m_pRpcReqHandler(new ClientRpc::CRpcReqHandler(L))
{
	CsvTable& t = CsvCfg::GetTable("test_client.csv");
	const auto& rIP = t.GetRecord("key", "server_ip");
	std::string svrIP = rIP.GetString("svalue");
	const auto& rPort = t.GetRecord("key", "server_port");
	int svrPort = rPort.GetInt<int>("ivalue");

	end_point = boost::asio::ip::tcp::endpoint(boost::asio::ip::address::from_string(svrIP), svrPort);

	assert(L);
	boost::system::error_code ec;
	socket.connect(end_point, ec);
	if (ec)
	{
		cerr << "Failed to connect " << end_point << " " << ec.message() << endl;
	}
	Spawn();
}

Client::~Client()
{
}

void Client::Spawn()
{
	try
	{
		using namespace boost::asio;
		spawn(io_service, [this](yield_context yield) {
			LoopRead(yield);
		});
		spawn(io_service, [this](yield_context yield) {
			LoopWrite(yield);
		});
	}
	catch (std::exception& e)
	{
		std::cerr << "Exception: " << e.what() << "\n";
	}
}

// Thread-safe.
void Client::PushRpcReq(
	const std::string& sServiceName,
	const std::string& sMethod,
	const google::protobuf::Message& request,
	const Callback& cb)
{
	PushRpcReq(sServiceName, sMethod, request.SerializeAsString(), cb);
}

void Client::PushRpcReq(
	const std::string& sServiceName,
	const std::string& sMethod,
	const std::string& sRequest,
	const Callback& cb)
{
	RpcDataSptr pRpc = std::make_shared<CRpcReqData>(
		sServiceName, sMethod, sRequest, cb);
	outgoing_rpc_vec.Push(pRpc);
}

void Client::PushRpcResp(uint32_t uRpcId, const std::string& sResponse)
{
	RpcDataSptr pRpc = std::make_shared<CRpcRespData>(
		uRpcId, sResponse);
	outgoing_rpc_vec.Push(pRpc);
}

void Client::LoopRead(yield_context yield)
{
	while (is_running)
	{
		bool ok = ReadOne(yield);
		if (!ok)
			Disconnect();
	}
}

void Client::LoopWrite(yield_context yield)
{
	while (is_running)
	{
		WaitMs(100, yield);
		if (outgoing_rpc_vec.IsEmpty())
			continue;

		SendLenDataBuf sendBuf;
		PrepareRpcs(sendBuf);

		boost::system::error_code ec;
		boost::asio::async_write(socket, sendBuf.vBuf, yield[ec]);
		if (!ec)
			continue;
		cerr << Fmt("Failed to send: (%d)%s") % ec.value() % ec.message() << endl;
		Disconnect();
	}
}

bool Client::ReadOne(yield_context yield)
{
	ReadBuf buf;
	uint32_t dwLenNet;  // 数据包长度, 网络字节序
	using namespace boost::asio;
	boost::system::error_code ec;
	std::size_t uReadHead = async_read(socket,
		buffer(&dwLenNet, sizeof(dwLenNet)), yield[ec]);
	if (ec)
	{
		cerr << Fmt("Failed to read len: (%d)%s") % ec.value() % ec.message() << endl;
		return false;
	}
	assert(uReadHead == sizeof(dwLenNet));
	uint32_t dwLenHost = ntohl(dwLenNet);
	if (dwLenHost > MAX_DATA_LEN)
	{
		cerr << "Illegal len: " << dwLenHost << endl;
		return false;
	}
	if (0 == dwLenHost)
		return true;

	buf.resize(dwLenHost);
	std::size_t uReadData = async_read(socket,
		buffer(&buf[0], dwLenHost), yield[ec]);
	if (ec)
	{
		cerr << Fmt("Failed to read data: (%d)%s") % ec.value() % ec.message() << endl;
		return false;
	}
	assert(uReadData == dwLenHost);

	HandleData(buf);
	return true;
}

void Client::HandleData(const ReadBuf& rBuf)
{
	assert(!rBuf.empty());

	if (transport_token.empty()) {
		rpc::FirstMsg firstMsg;
		firstMsg.ParseFromArray(&rBuf[0], rBuf.size());
		transport_token = firstMsg.token();
		return;
	}
	else {
		Util::Encrypt_XOR((char*)&rBuf[0], rBuf.size(), transport_token.c_str(), transport_token.size());
	}

	rpc::RpcPackage rpcPkg;
	bool ok = rpcPkg.ParseFromArray(&rBuf[0], rBuf.size());
	if (!ok)
	{
		cerr << "Got illegal RpcPackage, len=" << rBuf.size() << endl;
		return;
	}

	if (rpcPkg.has_request())
	{
		// cout << "Got request: " << rpcPkg.request().ShortDebugString() << endl;
		m_pRpcReqHandler->HandleRpcRequest(rpcPkg.request());
	}
	if (rpcPkg.has_response())
		HandleResponse(rpcPkg.response());
}

void Client::HandleResponse(const rpc::RpcResponse& response)
{
	auto itr = rpc_cb_map.find(response.id());
	if (itr == rpc_cb_map.end()) return;
	RpcCallback cb = (*itr).second;
	rpc_cb_map.erase(itr);
	if (cb) cb(response.content());
}

// 填空发送缓冲区列表, 记录Rpc回调
void Client::PrepareRpcs(
	SendLenDataBuf& rSendBuf)
{
	RpcDataVector vRpc;
	outgoing_rpc_vec.Swap(vRpc);  // 待发送Rpc列表
	LenVector& rLenVec = rSendBuf.vLenNet;  // 包长（网络序）列表
	DataVector& rDataVec = rSendBuf.v;  // 数据包
	BufVector& rBufVec = rSendBuf.vBuf;

	size_t uSize = vRpc.size();
	rLenVec.resize(uSize);
	rDataVec.resize(uSize);
	for (size_t i = 0; i < uSize; i++)
	{
		assert(vRpc[i]);
		const CRpcData& rpc = *vRpc[i];
		std::string& rDataStr = rDataVec[i];

		uint32_t uRpcId = rpc_id++;  // 即使是response也消耗一个
		rpc.SerializeToString(uRpcId, rDataStr);
		uint32_t uLenHost = rDataStr.size();
		if (0 == uLenHost) continue;

		// response 无回调
		const RpcCallback& cb = rpc.GetCallback();
		if (cb) rpc_cb_map[uRpcId] = cb;

		uint32_t& rLenNet = rLenVec[i];
		rLenNet = htonl(uLenHost);

		if (!transport_token.empty())
		{
			Util::Encrypt_XOR(&rDataStr[0], uLenHost, transport_token.c_str(), transport_token.size());
		}

		using boost::asio::const_buffer;
		rBufVec.emplace_back(
			const_buffer(&rLenNet, sizeof(rLenNet)));
		rBufVec.emplace_back(
			const_buffer(rDataStr.data(), uLenHost));
	}
}

void Client::WaitMs(unsigned int uMilliSeconds, yield_context yield)
{
	boost::asio::steady_timer timer(io_service);
	timer.expires_from_now(std::chrono::milliseconds(uMilliSeconds));
	boost::system::error_code ec;
	timer.async_wait(yield[ec]);
	if (!ec) return;
	cerr << Fmt("Async wait error: (%d)%s") % ec.value() % ec.message() << endl;
}

void Client::Disconnect()
{
	cout << "Disconnect.\n";
	is_running = false;
	boost::system::error_code ec;
	socket.close(ec);
	if (ec)
		cerr << "Close error: " << ec.message() << endl;
}

