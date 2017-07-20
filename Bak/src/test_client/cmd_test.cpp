#include "cmd_test.h"

#include "client.h"
#include "pb/svr/test_cmd.pb.h"

#include <iostream>

using namespace std;

CmdTest::CmdTest(Client& clt)
	: m_rClient(clt)
{
}

void CmdTest::Test(const char *cmd)
{
	svr::TestCmdRequest req;
	req.set_cmd(cmd);

	cout << "CmdTest: " << cmd << ", request: " << req.ShortDebugString() << endl;
	m_rClient.PushRpcReq("svr.TestCmd", "TestCmd", req, [](const std::string& s) {
		svr::TestCmdResponse resp;
		if (!resp.ParseFromString(s))
		{
			cerr << "Parse TestCmdResponse failed!" << endl;
			return;
		}
		cout << "TestCmd response: " << resp.ShortDebugString() << endl;
	});
}
