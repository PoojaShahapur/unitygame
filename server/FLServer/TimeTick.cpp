/**
 * \brief 时间回调函数
 *
 * 
 */

#include "FLServer.h"
#include "TimeTick.h"
#include "LoginManager.h"

FLTimeTick *FLTimeTick::instance = NULL;

struct LoginTimeout : public LoginManager::LoginTaskCallback
{  
    const zTime &ct;
    LoginTimeout(const zTime &ct) : ct(ct) {}
    void exec(LoginTask *lt)
    {
	if (lt->timeout(ct))	//结束掉已经不该存在的连接
	    lt->Terminate();
    }
};

void FLTimeTick::run()
{
    Zebra::logger->debug("FLTimeTick::run()");
    while(!isFinal())
    {
	zThread::sleep(1);
	zTime ct;
	if (ct.sec() % 10 == 0)
	{
	    LoginTimeout cb(ct);
	    LoginManager::getInstance().execAll(cb);
	}
    }
}

