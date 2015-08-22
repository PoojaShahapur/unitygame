/*************************************************************************
 Author: wang
 Created Time: 2014年10月22日 星期三 14时35分46秒
 File Name: ScenesServer/CardTujianManager.cpp
 Description: 
 ************************************************************************/

#include "CardTujianManager.h"
#include "SceneUser.h"
#include "HeroCardCmd.h"
#include "zDatabaseManager.h"
#include "zXML.h"
using namespace xml;

CardTujianManager::CardTujianManager()
{

}
CardTujianManager::~CardTujianManager()
{

}

void CardTujianManager::initTujian(SceneUser& user)
{
    CardTujianConfig::Init::ItemMapIter it = cardtujian.init.item.begin();
    for(; it!=cardtujian.init.item.end(); it++)
    {
	user.tujianData.privateCardMap.insert(std::make_pair(it->first, it->second.num()));
    }
}

bool CardTujianManager::addTuJian(SceneUser& user, const DWORD id)
{
    zCardB *cb = cardbm.get(id);
    if(!cb)
    {
	Zebra::logger->error("玩家:%s(%u) 添加非法的图鉴id:%u", user.name, user.id, id);
	return false;
    }
    //Effective STL No.24
    std::map<DWORD, BYTE>::iterator it = user.tujianData.privateCardMap.find(id);
    if(it != user.tujianData.privateCardMap.end())
    {
	user.tujianData.privateCardMap[id]++;
    }
    else
    {
	user.tujianData.privateCardMap.insert(std::make_pair(id, 1));
    }

    Cmd::stNotifyOneCardTujianInfoCmd send;
    send.id = id;
    send.num = user.tujianData.privateCardMap[id];
    user.sendCmdToMe(&send, sizeof(send));
    return true;
}

void CardTujianManager::notifyAllTujianDataToMe(SceneUser& user)
{
    BUFFER_CMD(Cmd::stNotifyAllCardTujianInfoCmd, send, zSocket::MAX_USERDATASIZE);
    std::map<DWORD, BYTE>::iterator it = user.tujianData.privateCardMap.begin();
    for(; it != user.tujianData.privateCardMap.end(); it++)
    {
	Zebra::logger->debug("[图鉴]准备发送图鉴数据 %u:%u", it->first, it->second);
	send->info[send->count].id = it->first;
	send->info[send->count].num = it->second;
	send->count++;
    }
    user.sendCmdToMe(send, sizeof(Cmd::stNotifyAllCardTujianInfoCmd)+send->count*sizeof(send->info[0]));
}

WORD CardTujianManager::getOneTujianNum(SceneUser& user, const DWORD id)
{
    WORD num = 0; 
    std::map<DWORD, BYTE>::iterator it = user.tujianData.privateCardMap.find(id);
    if(it != user.tujianData.privateCardMap.end())
    {
	num = it->second;
    }
    else
    {
	num = 0;
    }

    return num;
}
//////////////////////////////////////分割线/////////////////////////////////////////////////////////
unsigned int CardTujianData::saveCardTujianData(unsigned char* dest)
{
    if(!dest)
	return 0;
    int len = 0;
    int size = this->privateCardMap.size();
    memcpy(dest, &size, sizeof(size));
    len += sizeof(int);
    std::map<DWORD, BYTE>::iterator iter = this->privateCardMap.begin();
    while(iter != this->privateCardMap.end())
    {
	memcpy(dest+len, &(iter->first), sizeof(DWORD));
	len += sizeof(DWORD);
	memcpy(dest+len, &(iter->second), sizeof(BYTE));
	len += sizeof(BYTE);
	++iter;
    }
    Zebra::logger->debug("[卡牌图鉴]二进制保存字节数:%u", len);
    return len;
}

unsigned int CardTujianData::loadCardTujianData(unsigned char* src)
{
    if(!src)
	return 0;
    int len = 0;
    int size = *((int*)(src));
    len += sizeof(int);
    DWORD id = 0;
    BYTE num = 0;
    while(size-- > 0)
    {
	id = *((DWORD*)(src+len));
	len += sizeof(DWORD);
	num = *((BYTE*)(src+len));
	len += sizeof(BYTE);

	//Effective STL No.24
	this->privateCardMap.insert(std::make_pair(id, num));
    }
    Zebra::logger->debug("[卡牌图鉴]二进制加载字节数:%u", len);
    return len;
}
