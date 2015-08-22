/*************************************************************************
 Author: wang
 Created Time: 2014年11月11日 星期二 12时04分12秒
 File Name: Robot/Client.cpp
 Description: 
 ************************************************************************/
#include "Client.h"
#include "Command.h"
#include "TimeTick.h"

Client::Client(const std::string &user, const std::string &ip, const WORD &port):zTCPClientTask(ip, port, true)/*,_one_sec(1),_one_min(60)*/
{
    bzero(pstrIP, sizeof(pstrIP));
    wdPort = 0;
    bzero(charInfo, sizeof(charInfo));
    this->user = user;
    name = user.substr(0, user.find('@'));

}

Client::~Client()
{
}

bool Client::sendCmdIM(const void *pstrCmd, const int nCmdLen)
{
    Cmd::stNullUserCmd *cmd = (Cmd::stNullUserCmd*)pstrCmd;
    cmd->dwTimestamp = (DWORD)(ClientTimeTick::currentTime.msecs()-dwGameRunTime);
    return pSocket->sendCmd(pstrCmd, nCmdLen);
}

bool Client::sendCmd(const void *pstrCmd, const int nCmdLen)
{
    return sendCmdIM(pstrCmd, nCmdLen);
}

bool Client::init(DWORD acc, DWORD tempid, DES_cblock key)
{
    using namespace Cmd;
    accid = acc;
    loginTempID = tempid;
    memmove(key_des, key, sizeof(DES_cblock));
    if(!loginGatewayServer())
	return false;
#if 0
    WORD loop = 21;
    while((!waitingCharInfo || !roleExist) && --loop>0)
	usleep(500*1000);
    if(0 == loop)
	return false;
    else
	return logonRole();
#endif
    return true;
}

bool Client::logonRole(const WORD &i)
{
    using namespace Cmd;
    if(i<Cmd::MAX_CHARINFO && charInfo[i].id!=0 && charInfo[i].id!=(DWORD)-1)
    {
	stLoginSelectUserCmd cmd;
	cmd.charNo = i;
	role_num = i;
	if(!sendCmd(&cmd, sizeof(cmd)))
	    return false;
    }
#if 0
    while(!waitingRoleLogon)
	usleep(50*1000);
#endif
    return true;
}

bool Client::connect()
{
    using namespace Cmd;
    if(!zTCPClientTask::connect())
	return false;
    pSocket->setEncMethod(CEncrypt::ENCDEC_RC5);
    unsigned char key[16] = {0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb};
    pSocket->set_key_rc5((const unsigned char*)key, 16, 12);

    if(!versionCmd())
	return false;
    return true;
}

bool Client::msgParse(const Cmd::t_NullCmd *ptNullCmd, const unsigned int nCmdLen)
{
    using namespace Cmd;
    const stNullUserCmd *ptNull = (const stNullUserCmd*)ptNullCmd;
    switch(ptNull->byCmd)
    {
	case TIME_USERCMD:
	    return msgParse_time(ptNull, nCmdLen);
	default:
	    break;
    }
    Zebra::logger->error("客户端收到消息:cmd:%u para:%u len:%u",ptNull->byCmd, ptNull->byParam, nCmdLen);
    return false;
}

bool Client::versionCmd()
{
#ifndef _MOBILE
    using namespace Cmd;
    stUserVerifyVerCmd tCmd;
    return sendCmd(&tCmd, sizeof(tCmd));
#else
    return true;
#endif
}

bool Client::loginGatewayServer()
{
    using namespace Cmd;

    waitingCharInfo = false;
    roleExist = true;

    stPasswdLogonUserCmd tCmd;
    tCmd.loginTempID = loginTempID;
    tCmd.dwUserID = accid;
    tCmd.version = 1999;
    if(!pSocket->sendCmd(&tCmd, sizeof(tCmd)))
	return false;

    pSocket->setEncMethod(CEncrypt::ENCDEC_DES);
    pSocket->set_key_des(&key_des);
    pSocket->setEncMask((DWORD)key_des[2]);
    pSocket->setDecMask((DWORD)key_des[2]);
#if 0
    while(waitingCharInfo ^ roleExist)
	usleep(50*1000);
#endif
    return true;
}

bool Client::timeAction()
{
    return true;
}

bool Client::msgParse_time(const Cmd::stNullUserCmd *ptNull, const unsigned int nCmdLen)
{
    using namespace Cmd;
    switch(ptNull->byParam)
    {
	case GAMETIME_TIMER_USERCMD_PARA:
	    {
		stGameTimeTimerUserCmd *ptCmd=(stGameTimeTimerUserCmd*)ptNull;
		qwGameTime = ptCmd->qwGameTime;
		startTime.now();
		Zebra::logger->debug("初始化游戏时间成功 %llu",qwGameTime);
		return true;
	    }
	    break;
	case REQUESTUSERGAMETIME_TIMER_USERCMD_PARA:
	    {
		if(!dwGameRunTime)
		    dwGameRunTime = ClientTimeTick::currentTime.msecs();
		stUserGameTimeTimerUserCmd cmd;
		cmd.dwUserTempID = 0;
		cmd.qwGameTime = qwGameTime + startTime.elapse(ClientTimeTick::currentTime)/1000;
		return SEND_CMD(cmd);
	    }
	    break;
    }
    return false;    
}
