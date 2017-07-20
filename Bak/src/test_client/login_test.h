#ifndef LOGIN_TEST_H
#define LOGIN_TEST_H

class Client;

class LoginTest
{
public:
	explicit LoginTest(Client& clt);
	virtual ~LoginTest() {};

public:
	void TestLogin();

private:
	Client& m_rClient;
};

#endif  // CLIENT_H
