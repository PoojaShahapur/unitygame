#ifndef CELL_SVC_TEST_CPP_H
#define CELL_SVC_TEST_CPP_H

#include "rpc/rpc_service.h"  // for CRpcService

// Cpp服务示例。
// test_cpp.proto
class CSvcTestCpp final : public CRpcService
{
public:
	CSvcTestCpp();

public:
	// 服务名，对应proto文件中的service, 带包名。
	std::string GetServiceName() const override { return "svr.test.TestCpp"; }

private:
	void Test(const CRpcCallContext& ctx, const std::string& sContent);
};  // class CSvcTestCpp

#endif  // CELL_SVC_TEST_CPP_H
