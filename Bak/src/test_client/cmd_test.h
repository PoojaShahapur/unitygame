#ifndef CMD_TEST_H
#define CMD_TEST_H

// 服务器端内部测试命令， 如执行Mongodb测试例程

class Client;

class CmdTest
{
public:
	explicit CmdTest(Client& clt);
	virtual ~CmdTest() {};

public:
	void Test(const char *cmd);

private:
	Client& m_rClient;
};

#endif  // CMD_TEST_H
