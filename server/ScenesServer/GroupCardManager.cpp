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
 * \brief   �����Ƿ����
 * \param   index 
 * \return  ��:true ,��:false
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
	Zebra::logger->error("[����]���:%s(%u) �½���һ�������� ְҵ�Ƿ� occupation,%u",user.name, user.id, occupation);
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
	Zebra::logger->error("[����]���:%s(%u) �½���һ�������� ����ʧ��,�Ѿ���:%u��������",user.name, user.id, count);
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
	char name[MAX_NAMESIZE+1] = "�Զ�������";
	WORD count = 1;
	while(checkGroupNameExist(name, user))
	{
	    count++;
	    sprintf(name, "�Զ�������%d", count);
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
	Zebra::logger->debug("[����]���:%s(%u) �½���һ�������� index,%u occupation,%u",user.name, user.id, index, occupation);
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
	Zebra::logger->debug("[����]���:%s(%u) ɾ����һ������ index,%u ",user.name, user.id, index);
    }
    else
    {
	send.success = 0;
	user.sendCmdToMe(&send, sizeof(send));
	Zebra::logger->error("[����]���:%s(%u) ����ɾ���Ƿ������� index,%u ",user.name, user.id, index);
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
	Zebra::logger->debug("��ӡ���յ��Ĵ洢������Ϣ ��ɫ:%s(%u) �� %u ��,%u",user.name, user.id, i, id[i]);
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
	Zebra::logger->error("�洢��������������30�� �洢ʧ��");
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
	    Zebra::logger->error("���Ʊ��� û�����ID:%u �Ŀ��� �洢ʧ��",cardID);
	    break;
	}
	if((cb->occupation > 0) && (occupation != cb->occupation))		//ְҵ�ж�
	{
	    flag = 1;
	    Zebra::logger->error("��ǰҪ�洢��ְҵ �� %u���Ƶ�ְҵ�����",cardID);
	    break;
	}
	if(getOneIdTimes(cardID, id, count) > CardTujianManager::getMe().getOneTujianNum(user, cardID))	    //����ӵ���ж�
	{
	    flag = 1;
	    Zebra::logger->error("����ͼ���� û�����ID:%u �Ŀ��� �洢ʧ��",cardID);
	    break;
	}
	if(getOneNameTimes(cardID, id, count) > cardtujian.limit.sameNameNum)//ͬ����������
	{
	    flag = 1;
	    Zebra::logger->error("��ͬ���ֵĿ�����������:%u �洢ʧ��", cardtujian.limit.sameNameNum());
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
    Zebra::logger->debug("[����]���:%s(%u) ������һ������ index,%u ",user.name, user.id, index);
    user.save(Cmd::Record::OPERATION_WRITEBACK);
    return true;
}

bool GroupCardManager::handleRenameOneGroup(SceneUser& user, const DWORD index, char *name)
{
    if (strlen(name) > 12)
    {
	Zebra::logger->warn("�������ֳ���̫��(%d)",strlen(name));
	return true;
    }
    std::map<DWORD, t_group>::iterator it = user.groupcardData.groupCardMap.find(index);
    if(it != user.groupcardData.groupCardMap.end())
    {
	Zebra::logger->debug("[����]���:%s(%u) �����Ƹ��� index,%u %s-->%s",user.name, user.id, index, it->second.name, name);
	strncpy(it->second.name, name, MAX_NAMESIZE);
    }
    else
    {
	Zebra::logger->error("[����]���:%s(%u) ��������Ƿ������� index,%u ",user.name, user.id, index);
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
	Zebra::logger->debug("[����]׼������������Ϣ %u:%u:%u:%s",send->info[send->count].index, send->info[send->count].occupation, send->info[send->count].cardNum, send->info[send->count].name);
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
//////////////////////////////////////�ָ���/////////////////////////////////////////////////////////
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
    Zebra::logger->debug("[������Ϣ]�����Ʊ����ֽ���:%u", len);
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
    Zebra::logger->debug("[������Ϣ]�����Ƽ����ֽ���:%u", len);
    return len;
}
