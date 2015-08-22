#include "GroupCardManager.h"
#include "Zebra.h"
#include "SceneUser.h"
#include "zDatabaseManager.h"
#include "zXML.h"

///////////////////////////////////////////////
//
//code[GroupCardManager.cpp] defination by codemokey
//
//
///////////////////////////////////////////////

using namespace xml;


GroupCardManager::GroupCardManager()
{}

GroupCardManager::~GroupCardManager()
{}

WORD GroupCardManager::getOneIdTimes(const DWORD cardID, DWORD id[], const WORD count)
{
    WORD times = 0;
    for(WORD i=0; i<count; i++)
    {
	if(id[i] == cardID)
	{
	    times++;
	}
    }
    return times;
}

WORD GroupCardManager::getOneNameTimes(const DWORD cardID, DWORD id[], const WORD count)
{
    WORD times = 0;
    zCardB *cb = cardbm.get(cardID);
    if(!cb)
	return (WORD)-1;
    zCardB *cb_tmp = NULL;
    for(WORD i=0; i<count; i++)
    {
	cb_tmp = cardbm.get(id[i]);
	if(!cb_tmp)
	{
	    return (WORD)-1;
	}
	if(0 == strcmp(cb->name, cb_tmp->name))
	{
	    times++;
	}
    }
    return times;
}

DWORD GroupCardManager::getOccupationByIndex(SceneUser& user, const DWORD index)
{
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it != user.groupcardData.groupCardMap.end())
    {
	return (it->second.occupation);
    }
    return 0;
}

/**
 * \brief   套牌是否可用
 * \param   index 
 * \return  真:true ,假:false
*/
bool GroupCardManager::canUseOneGroup(SceneUser& user, const DWORD index)
{
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it == user.groupcardData.groupCardMap.end())
    {	
	return false;
    }
    for(WORD i=0; i<30; i++)
    {
	if(it->second.cards[i] == 0)
	{
	    return false;
	}
    }

    return true;
}

bool GroupCardManager::handleCreateOneGroup(SceneUser& user, const DWORD occupation, bool gm)
{
    if(occupation <= HERO_OCCUPATION_NONE || occupation >= HERO_OCCUPATION_MAX)
    {
	Zebra::logger->error("[套牌]玩家:%s(%u) 新建了一个空套牌 职业非法 occupation,%u",user.name, user.id, occupation);
	return false;
    }
    DWORD count = 0;
    for(std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.begin(); it!= user.groupcardData.groupCardMap.end(); it++)
    {
	if(it->first >= 1000)
	{
	    count++;
	}
    }
    if(count >= cardtujian.limit.totalGroup)
    {
	Zebra::logger->error("[套牌]玩家:%s(%u) 新建了一个空套牌 建立失败,已经有:%u个套牌了",user.name, user.id, count);
	return false;
    }

    DWORD index = 0;
    index = zMisc::randBetween(1000, 2000);
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    while(it != user.groupcardData.groupCardMap.end())
    {
	index = zMisc::randBetween(1000, 2000);
	it = user.groupcardData.groupCardMap.find(index);
    }

    if(it == user.groupcardData.groupCardMap.end())
    {
	t_group tmp;
	if(gm)
	{
	    for(WORD i=0; i<30; i++)
	    {
		tmp.cards[i] = 10000;
	    }
	}
	tmp.occupation = occupation;
	char name[MAX_NAMESIZE+1] = "自定义套牌";
	WORD count = 1;
	while(checkGroupNameExist(name, user))
	{
	    count++;
	    sprintf(name, "自定义套牌%d", count);
	}
	strncpy(tmp.name, name, MAX_NAMESIZE);
	user.groupcardData.groupCardMap[index] = tmp;


	Cmd::stRetCreateOneCardGroupUserCmd send;
	send.occupation = occupation;
	send.index = index;
	strncpy(send.name, tmp.name, MAX_NAMESIZE);
	zXMLParser xml;
	BYTE *transName = xml.charConv((BYTE*)(send.name), "GB2312", "UTF-8");
	if(transName)
	{
	    strncpy(send.name, (char *)transName, MAX_NAMESIZE);
	    SAFE_DELETE_VEC(transName);
	}
	user.sendCmdToMe(&send, sizeof(send));
	Zebra::logger->debug("[套牌]玩家:%s(%u) 新建了一个空套牌 index,%u occupation,%u",user.name, user.id, index, occupation);
    }
    return true;
}

bool GroupCardManager::handleDeleteOneGroup(SceneUser& user, const DWORD index)
{
    Cmd::stRetDeleteOneCardGroupUserCmd send;
    send.index = index;
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it != user.groupcardData.groupCardMap.end())
    {
	user.groupcardData.groupCardMap.erase(it);
	send.success = 1;
	user.sendCmdToMe(&send, sizeof(send));
	Zebra::logger->debug("[套牌]玩家:%s(%u) 删除了一个套牌 index,%u ",user.name, user.id, index);
    }
    else
    {
	send.success = 0;
	user.sendCmdToMe(&send, sizeof(send));
	Zebra::logger->error("[套牌]玩家:%s(%u) 请求删除非法的套牌 index,%u ",user.name, user.id, index);
	return false;
    }
    return true;
}

bool GroupCardManager::handleSaveOneGroup(SceneUser& user, DWORD id[], const WORD count, const DWORD index)
{
    Cmd::stRetSaveOneCardGroupUserCmd send;
    send.index = index;
    if(index < 1000)
    {
	user.sendCmdToMe(&send, sizeof(send));
	return false;
    }
#ifdef _WC_DEBUG
    for(WORD i=0; i<count; i++)
    {
	Zebra::logger->debug("打印接收到的存储套牌消息 角色:%s(%u) 第 %u 个,%u",user.name, user.id, i, id[i]);
    }
#endif
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it == user.groupcardData.groupCardMap.end())
    {
	user.sendCmdToMe(&send, sizeof(send));
	return false;
    }
    if(count > 30 || (count == 0))
    {
	user.sendCmdToMe(&send, sizeof(send));
	Zebra::logger->error("存储套牌数量超过了30张 存储失败");
	return false;
    }

    DWORD cardID = 0;
    WORD occupation = it->second.occupation;
    BYTE flag = 0;
    for(WORD i=0; i<count; i++)
    {
	cardID = id[i];
	zCardB *cb = cardbm.get(cardID);
	if(!cb)
	{
	    flag = 1;
	    Zebra::logger->error("卡牌表中 没有这个ID:%u 的卡牌 存储失败",cardID);
	    break;
	}
	if((cb->occupation > 0) && (occupation != cb->occupation))		//职业判断
	{
	    flag = 1;
	    Zebra::logger->error("当前要存储的职业 和 %u卡牌的职业不相符",cardID);
	    break;
	}
	if(getOneIdTimes(cardID, id, count) > CardTujianManager::getMe().getOneTujianNum(user, cardID))	    //自身拥有判断
	{
	    flag = 1;
	    Zebra::logger->error("卡牌图鉴中 没有这个ID:%u 的卡牌 存储失败",cardID);
	    break;
	}
	if(getOneNameTimes(cardID, id, count) > cardtujian.limit.sameNameNum)//同名个数限制
	{
	    flag = 1;
	    Zebra::logger->error("相同名字的卡牌数量超过:%u 存储失败", cardtujian.limit.sameNameNum());
	    break;
	}
    }

    if(flag)
    {
	user.sendCmdToMe(&send, sizeof(send));
	return false;
    }
    bzero(it->second.cards, sizeof(it->second.cards));
    for(WORD i=0; i<count; i++)
    {
	it->second.cards[i] = id[i];
    }
    send.success = 1;
    user.sendCmdToMe(&send, sizeof(send));
    Zebra::logger->debug("[套牌]玩家:%s(%u) 保存了一个套牌 index,%u ",user.name, user.id, index);
    user.save(Cmd::Record::OPERATION_WRITEBACK);
    return true;
}

bool GroupCardManager::handleRenameOneGroup(SceneUser& user, const DWORD index, char *name)
{
    if (strlen(name) > 12)
    {
	Zebra::logger->warn("套牌名字长度太长(%d)",strlen(name));
	return true;
    }
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it != user.groupcardData.groupCardMap.end())
    {
	Zebra::logger->debug("[套牌]玩家:%s(%u) 给套牌改名 index,%u %s-->%s",user.name, user.id, index, it->second.name, name);
	strncpy(it->second.name, name, MAX_NAMESIZE);
    }
    else
    {
	Zebra::logger->error("[套牌]玩家:%s(%u) 请求改名非法的套牌 index,%u ",user.name, user.id, index);
	return false;
    }
    return true;
}

void GroupCardManager::notifyAllGroupListToMe(SceneUser& user)
{
    BUFFER_CMD(Cmd::stRetCardGroupListInfoUserCmd, send, zSocket::MAX_USERDATASIZE);
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.begin();
    for(; it != user.groupcardData.groupCardMap.end(); it++)
    {
	send->info[send->count].index = it->first;
	send->info[send->count].occupation = it->second.occupation;
	send->info[send->count].cardNum = countOneGroupCard(user, it->first);
	strncpy(send->info[send->count].name, it->second.name, MAX_NAMESIZE);
	zXMLParser xml;
	BYTE *transName = xml.charConv((BYTE*)(send->info[send->count].name), "GB2312", "UTF-8");
	if(transName)
	{
	    strncpy(send->info[send->count].name, (char *)transName, MAX_NAMESIZE);
	    SAFE_DELETE_VEC(transName);
	}
	Zebra::logger->debug("[套牌]准备发送套牌信息 %u:%u:%u:%s",send->info[send->count].index, send->info[send->count].occupation, send->info[send->count].cardNum, send->info[send->count].name);
	send->count++; 
    }
    user.sendCmdToMe(send, sizeof(Cmd::stRetCardGroupListInfoUserCmd)+send->count*sizeof(send->info[0]));
}

void GroupCardManager::notifyOneGroupInfoToMe(SceneUser& user, const DWORD index)
{
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    BUFFER_CMD(Cmd::stRetOneCardGroupInfoUserCmd, send, zSocket::MAX_USERDATASIZE);
    send->index = index;
    if(it != user.groupcardData.groupCardMap.end())
    {
	for(DWORD i=0; i<30; i++)
	{
	    if(it->second.cards[i] > 0)
	    {
		send->id[send->count] = it->second.cards[i];
		send->count++;
	    }
	}
    }
    user.sendCmdToMe(send, sizeof(Cmd::stRetOneCardGroupInfoUserCmd)+send->count*sizeof(send->id[0]));
    
}

bool GroupCardManager::initOneChallengeCards(SceneUser& user, const DWORD index, std::vector<DWORD> &lib)
{
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it == user.groupcardData.groupCardMap.end())
	return false;
    for(DWORD i=0; i<30; i++)
    {
	lib.push_back(it->second.cards[i]);
    }
    return true;
}

bool GroupCardManager::checkGroupNameExist(char *name, SceneUser& user)
{
    if(!name)
	return false;
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.begin();
    for(; it != user.groupcardData.groupCardMap.end(); it++)
    {
	if(strcmp(name, it->second.name) == 0)
	    return true;
    }
    return false;
}

DWORD GroupCardManager::countOneGroupCard(SceneUser& user, DWORD index)
{
    DWORD count = 0;
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it == user.groupcardData.groupCardMap.end())
	return 0;
    for(DWORD i=0; i<30; i++)
    {
	zCardB *cb = cardbm.get(it->second.cards[i]);
	if(cb)
	    count++;
    }
    return count;
}
//////////////////////////////////////分割线/////////////////////////////////////////////////////////
unsigned int GroupCardData::saveCardGroupData(unsigned char* dest)
{
    if(!dest)
	return 0;
    int len = 0;
    int size = this->groupCardMap.size();
    memcpy(dest, &size, sizeof(size));
    len += sizeof(int);
    std::map<DWORD, t_group>::iterator iter = this->groupCardMap.begin();
    while(iter != this->groupCardMap.end())
    {
	memcpy(dest+len, &(iter->first), sizeof(DWORD));
	len += sizeof(DWORD);
	memcpy(dest+len, &(iter->second), sizeof(t_group));
	len += sizeof(t_group);
	++iter;
    }
    Zebra::logger->debug("[套牌信息]二进制保存字节数:%u", len);
    return len;
}

unsigned int GroupCardData::loadCardGroupData(unsigned char* src)
{
    if(!src)
	return 0;
    int len = 0;
    int size = *((int*)(src));
    len += sizeof(int);
    DWORD id = 0;
    while(size-- > 0)
    {
	id = *((DWORD*)(src+len));
	len += sizeof(DWORD);
	memcpy(&this->groupCardMap[id], src+len, sizeof(t_group));
	len += sizeof(t_group);
    }
    Zebra::logger->debug("[套牌信息]二进制加载字节数:%u", len);
    return len;
}
