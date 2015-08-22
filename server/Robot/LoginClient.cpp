/**
 * \brief 定义档案服务器连接客户端
 *
 * 负责与档案服务器交互，存取档案
 * 
 */

#include "LoginClient.h"

bool LoginClient::connect()
{
  if (!zTCPClient::connect())
  {
    Zebra::logger->error("LoginClient::connect ERROR");
    return false;
  }
  pSocket->setEncMethod(CEncrypt::ENCDEC_RC5);
  unsigned char key[16] = {0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb};
  pSocket->set_key_rc5((const BYTE *)key, 16, 12);
  return true;
}

bool LoginClient::versionCmd()
{
    using namespace Cmd;
    stUserVerifyVerCmd tCmd;
    return sendCmd(&tCmd, sizeof(tCmd));
}

void LoginClient::UseIPEntry(BYTE *pszSrc, int iNum)
{
    unsigned char nKey = 16, rkey = 0;
    for(int i = 0;i<iNum; i++)
    {
	pszSrc[i] ^= lpstrIP[rkey];
	pszSrc[i]++;
	if(++rkey >= nKey)
	    rkey = 0;
    }
}

loginLoginServer_retVal LoginClient::loginLoginServer(const char *name, const char *passwd)
{
    using namespace Cmd;
    if(!connect())
    return LOGIN_FAIL;
    if(!versionCmd())
    return LOGIN_FAIL;
    unsigned char encry_pwd[] = {0x0C, 0x25, 0x86, 0x51, 0xC2, 0x01, 0xE2, 0x1A, 0x19, 0x3E, 0xBD, 0x1A, 0x19};
    stUserRequestLoginCmd tCmd;
    tCmd.game = 10;
    tCmd.zone = 30;
    bzero(tCmd.pstrPassword, sizeof(tCmd.pstrPassword));
    strncpy(tCmd.pstrName, name, sizeof(tCmd.pstrName));

    unsigned char nKey = 16, rkey = 0;
    for(int i = 0;i<12; i++)
    {
	encry_pwd[i] ^= lpstrIP[rkey];
	encry_pwd[i]++;
	if(++rkey >= nKey)
	    rkey = 0;
    }
    memcpy(tCmd.pstrPassword, encry_pwd, 13);
    if(!pSocket->sendCmd(&tCmd, sizeof(tCmd)))
	return LOGIN_FAIL;

    unsigned char pstrCmd[zSocket::MAX_DATASIZE];
    stServerReturnLoginSuccessCmd *ptCmd=(stServerReturnLoginSuccessCmd*)pstrCmd;
    stServerReturnLoginFailedCmd *fail=(stServerReturnLoginFailedCmd*)pstrCmd;
    int nCmdLen = pSocket->recvToCmd(pstrCmd, zSocket::MAX_DATASIZE, true);
    if(LOGON_USERCMD == fail->byCmd && SERVER_RETURN_LOGIN_FAILED == fail->byParam)
    {
#if 0
	WORD retVal = msgParse_login(ptCmd, (DWORD)nCmdLen);
	if(retVal == 3)
	    return LOGIN_ID_IN_USE;
	else
	    return LOGIN_FAIL;
#endif
	return LOGIN_FAIL;
    }
    else if(LOGON_USERCMD == ptCmd->byCmd && SERVER_RETURN_LOGIN_OK == ptCmd->byParam)
    {
	accid = ptCmd->dwUserID;
	loginTempID = ptCmd->loginTempID;
	memmove(pstrIP, ptCmd->pstrIP, MAX_IP_LENGTH);
	wdPort = ptCmd->wdPort;
	memmove(&key_des, &(ptCmd->key[ptCmd->key[58]]), sizeof(key_des));
	return LOGIN_SUCCESS;
    }
    return LOGIN_FAIL;
}
bool LoginClient::requestIP()
{
    if(!connect())
	return false;
    if(!versionCmd())
	return false;
    Cmd::stRequestClientIP tCmd;
    if(!sendCmd(&tCmd, sizeof(tCmd)))
	return false;
    unsigned char pstrCmd[zSocket::MAX_DATASIZE];
    Cmd::stReturnClientIP* ptCmd=(Cmd::stReturnClientIP*)pstrCmd;
    pSocket->recvToCmd(pstrCmd, zSocket::MAX_DATASIZE, true);
    if(Cmd::LOGON_USERCMD == ptCmd->byCmd
	    && Cmd::RETURN_CLIENT_IP_PARA == ptCmd->byParam)
    {
	memcpy(lpstrIP, ptCmd->pstrIP, MAX_IP_LENGTH);
	Zebra::logger->trace("����IP�ɹ�:%s",lpstrIP);
	return true;
    }

    Zebra::logger->error("����IPʧ��");
    return false;
}

bool LoginClient::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
  return true;
}

WORD LoginClient::msgParse_login(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
    using namespace Cmd;

    if (pNullCmd->cmd!=LOGON_USERCMD) return 0;

    switch (pNullCmd->para)
    {
	case SERVER_RETURN_LOGIN_FAILED:
	    {
		Zebra::logger->error("LOGIN ERROR:");
	    }
	    break;
	default:
	    break;
    }

    Zebra::logger->error("LoginClient::cmdMsgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
    return 0;
}

