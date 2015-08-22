/**
* \brief 登陆连接管理容器
*
* 
*/

#include "FLServer.h"
#include "LoginManager.h"
#include "LoginTask.h"
#include "ServerManager.h"
#include "FLCommand.h"
#include "GYListManager.h"

LoginManager *LoginManager::instance = NULL;
DWORD LoginManager::maxGatewayUser=MAX_GATEWAYUSER;

/**
* \brief 向容器中添加一个连接
*
* \param task 一个连接任务
* \return 添加是否成功
*/
bool LoginManager::add(LoginTask *task)
{
	if( (DWORD)task == 0x00000001 )
	{
		//int iii = 0;
	}
	Zebra::logger->debug("LoginManager::add");
	if (task)
	{
		task->genTempID();
		mlock.lock();
		LoginTaskHashmap_const_iterator it = loginTaskSet.find(task->getTempID());
		if (it != loginTaskSet.end())
		{
			mlock.unlock();
			return false;
		}
		loginTaskSet.insert(LoginTaskHashmap_pair(task->getTempID(),task));
		mlock.unlock();
		return true;
	}
	else
		return false;
}

/**
* \brief 从一个容器中移除一个连接
*
* \param task 一个连接任务
*/
void LoginManager::remove(LoginTask *task)
{
	Zebra::logger->debug("LoginManager::remove");
	if (task)
	{
		mlock.lock();
		loginTaskSet.erase(task->getTempID());
		mlock.unlock();
	}
}

/**
* \brief 广播指令到指定的登陆连接
*
* \param loginTempID 登陆连接的唯一编号
* \param pstrCmd 待转发的指令
* \param nCmdLen 待转发的指令长度
* \return 转发是否成功
*/
bool LoginManager::broadcast(const DWORD loginTempID,const void *pstrCmd,int nCmdLen)
{
    Zebra::logger->debug("LoginManager::broadcast");
    zMutex_scope_lock scope_lock(mlock);
    LoginTaskHashmap_iterator it = loginTaskSet.find(loginTempID);
    if (it != loginTaskSet.end())
    {
	return it->second->sendCmd(pstrCmd,nCmdLen);
    }
    else
	return false;
}

/**
* \brief 登陆验证返回
* \param loginTempID 指定的客户端连接临时编号
* \param retcode 待返回的代码
* \param session 登陆验证返回的会话信息
*/
void LoginManager::verifyReturn(const DWORD loginTempID,const BYTE retcode,const t_NewLoginSession &session)
{
    Zebra::logger->debug("LoginManager::verifyReturn");
    using namespace Cmd::DBAccess;
    zMutex_scope_lock scope_lock(mlock);

    LoginTaskHashmap_iterator it = loginTaskSet.find(loginTempID);
    if (it != loginTaskSet.end())
    {
	LoginTask *task = it->second;

	GYList *gy = NULL;
	switch(retcode)
	{
	    case SESSIONCHECK_SUCCESS:
		switch(session.state)
		{
		    case 0:
			//Zebra::logger->error("登陆成功直接分配网关");
			//登陆成功直接分配网关
			Zebra::logger->error("gameZone = %u",session.gameZone.zone);
			gy = GYListManager::getInstance().getAvl(session.gameZone);
			if (NULL == gy)
			{
			    task->LoginReturn(Cmd::LOGIN_RETURN_GATEWAYNOTAVAILABLE);
			    Zebra::logger->error("LoginManager::verifyReturn ����û�п�");
			}
			else if (gy->wdNumOnline >= (maxGatewayUser - 10))
			{
			    task->LoginReturn(Cmd::LOGIN_RETURN_USERMAX);
			    Zebra::logger->error("LoginManager::verifyReturn ���������û�������:%d,�޷����������½����",gy->wdNumOnline);
			}
			else
			{
			    CEncrypt                      e;
			    Cmd::FL::t_NewSession_Session tCmd;

			    // [ranqd delete] 未成功登陆的用户不记数，等待网关返回
			    //	gy->wdNumOnline++;

			    tCmd.session             = session;
			    tCmd.session.wdGatewayID = gy->wdServerID;

			    //生成des加密密钥                
			    e.random_key_des(&tCmd.session.des_key);
			    Zebra::logger->info("[LOGIN]����DES��Կ:%u %u %u %u %u %u %u %u",tCmd.session.des_key[0],tCmd.session.des_key[1],tCmd.session.des_key[2],tCmd.session.des_key[3],tCmd.session.des_key[4],tCmd.session.des_key[5],tCmd.session.des_key[6],tCmd.session.des_key[7]);
			    /*
			       for (int i=0; i<sizeof(tCmd.session.des_key); i++)
			       tCmd.session.des_key[i] = (DWORD)randBetween(0,255);
			       */
			    bcopy(gy->pstrIP,tCmd.session.pstrIP,sizeof(tCmd.session.pstrIP));
			    tCmd.session.wdPort = gy->wdPort;
			    ServerManager::getInstance().broadcast(session.gameZone,&tCmd,sizeof(tCmd));
			}
			break;
		    case 1:
			//帐号处于锁定状态
			task->LoginReturn(Cmd::LOGIN_RETURN_LOCK);
			Zebra::logger->error("帐号已锁定");
			break;
		    case 4:
			//帐号处于待激活状态
			task->LoginReturn(Cmd::LOGIN_RETURN_WAITACTIVE);
			Zebra::logger->error("帐号待激活");
			break;
		}
		break;
	    case SESSIONCHECK_DB_FAILURE:
		Zebra::logger->info("数据库出错,登陆失败");
		task->LoginReturn(Cmd::LOGIN_RETURN_DB);
		break;
	    case SESSIONCHECK_PWD_FAILURE:
		Zebra::logger->info("账号密码错误,登陆失败");
		task->LoginReturn(Cmd::LOGIN_RETURN_PASSWORDERROR);
		break;
	}
    }
}

/**
* \brief 返回错误代码到指定的客户端
* \param loginTempID 指定的客户端连接临时编号
* \param retcode 待返回的代码
* \param tm 返回信息以后是否断开连接,缺省是断开连接
*/
void LoginManager::loginReturn(const DWORD loginTempID,const BYTE retcode,const bool tm)
{
	Zebra::logger->debug("LoginManager::loginReturn");
	mlock.lock();
	LoginTaskHashmap_iterator it = loginTaskSet.find(loginTempID);
	if (it != loginTaskSet.end())
		it->second->LoginReturn(retcode,tm);
	mlock.unlock();
}

/**
* \brief 对容器中的所有元素调用回调函数
* \param cb 回调函数实例
*/
void LoginManager::execAll(LoginTaskCallback &cb)
{
	Zebra::logger->debug("LoginManager::execAll");
	zMutex_scope_lock scope_lock(mlock);
	for(LoginTaskHashmap_iterator it = loginTaskSet.begin(); it != loginTaskSet.end(); ++it)
	{
		cb.exec(it->second);
	}
}

