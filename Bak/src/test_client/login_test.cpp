#include "login_test.h"

#include "client.h"
#include "pb/rpc/login.pb.h"

#include <iostream>

using namespace std;

LoginTest::LoginTest(Client& clt)
	: m_rClient(clt)
{
}

void LoginTest::TestLogin()
{
	rpc::LoginRequest req;
	req.set_account("TestAccount");
	req.set_password("Password");
	cout << "Test login, request: " << req.ShortDebugString() << endl;
	m_rClient.PushRpcReq("rpc.Login", "Login", req, [](const std::string& s) {
		rpc::LoginResponse resp;
		if (!resp.ParseFromString(s))
		{
			cerr << "Parse LoginResponse failed!" << endl;
			return;
		}
		cout << "Login response: " << resp.ShortDebugString() << endl;
	});
}
