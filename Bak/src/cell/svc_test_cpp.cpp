#include "svc_test_cpp.h"

#include "log.h"
#include "pb/svr/test_cpp.pb.h"
#include "rpc/rpc_call_context.h"  // for GetRpcId()
#include "rpc/rpc_helper.h"  // for PushRpcResp()

const char LOG_NAME[] = "CSvcTestCpp";

// Cpp服务示例。

CSvcTestCpp::CSvcTestCpp()
{
	RegisterMethod("Test",
		[this](const CRpcCallContext& ctx, const std::string& sContent) {
			Test(ctx, sContent);
		});
}

void CSvcTestCpp::Test(const CRpcCallContext& ctx,
	const std::string& sContent)
{
	svr::test::TestRequest req;
	if (!RpcHelper::ParseMsgFromStr(sContent, req))
		return;

	LOG_INFO(Fmt("Test. str = %1%") % req.str());
	svr::test::TestResponse resp;
	resp.set_str(req.str());
	RpcHelper::ReplyTo(ctx, resp);
}
