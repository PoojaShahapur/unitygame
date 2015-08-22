/**
* \brief 网关信息列表
*
* 登陆服务器需要保存最新的所有网关的信息列表,便于分配网关
* 
*/

#include <map>
#include "zType.h"
#include "zMisc.h"
#include "GYListManager.h"
#include "Zebra.h"
#include "LoginManager.h"

GYListManager *GYListManager::instance = NULL;

/**
* \brief 添加网关信息
* 如果已经存在,直接更新信息,没有需要新建立记录
* \param gameZone 游戏区信息
* \param gy 网关信息
* \return 添加是否成功
*/
bool GYListManager::put(const GameZone_t &gameZone,const GYList &gy)
{
	Zebra::logger->debug("GYListManager::put");
	zMutex_scope_lock scope_lock(mlock);
	std::pair<GYListContainer_iterator,GYListContainer_iterator> hps = gyData.equal_range(gameZone);
	for(GYListContainer_iterator it = hps.first; it != hps.second; ++it)
	{
		if (it->second.wdServerID == gy.wdServerID)
		{
			//找到了,只是更新,只限于网关连接数和网关状态
			bcopy(gy.pstrIP,it->second.pstrIP,MAX_IP_LENGTH);
			it->second.wdPort = gy.wdPort;
			it->second.wdNumOnline = gy.wdNumOnline;
			it->second.state = gy.state;
			return true;
		}
	}

	//没有找到,需要插入新的记录
	gyData.insert(GYListContainer_value_type(gameZone,gy));
	return true;
}

void GYListManager::disableAll(const GameZone_t &gameZone)
{
	Zebra::logger->debug("GYListManager::disableAll");
	zMutex_scope_lock scope_lock(mlock);
	std::pair<GYListContainer_iterator,GYListContainer_iterator> hps = gyData.equal_range(gameZone);
	for(GYListContainer_iterator it = hps.first; it != hps.second; ++it)
	{
		it->second.wdPort = 0;
		it->second.wdNumOnline = 0;
		it->second.state = state_maintain;
		Zebra::logger->debug("������(%u)״̬Ϊά����",gameZone.zone);
	}
}

/**
* \brief 随机获取一个人数最小的网关信息
* \return 网关信息
*/
GYList *GYListManager::getAvl(const GameZone_t &gameZone)
{
	Zebra::logger->debug("GYListManager::getAvl");
	zMutex_scope_lock scope_lock(mlock);
	GYList *ret = NULL,*tmp = NULL;

	Zebra::logger->debug("GYListSize = %d",gyData.size());
	std::pair<GYListContainer_iterator,GYListContainer_iterator> hps = gyData.equal_range(gameZone);
	for(GYListContainer_iterator it = hps.first; it != hps.second; ++it)
	{
		tmp = &(it->second);
		if (state_none == tmp->state
			&& (NULL == ret
			|| ret->wdNumOnline >= tmp->wdNumOnline))
		{
			ret = tmp;
		}
	}
	return ret;
}

DWORD GYListManager::getOnline(void)
{
	DWORD                    dwCount;
	GYList                   *pGYL;
	GYListContainer_iterator pGYLCI;

	Zebra::logger->debug("GYListManager::getOnline");
	zMutex_scope_lock scope_lock(mlock);

	dwCount = 0;
	for(pGYLCI=gyData.begin();pGYLCI != gyData.end();pGYLCI++)
	{
		pGYL = &(pGYLCI->second);
		dwCount += pGYL->wdNumOnline;
	}

	return dwCount;
}

/**
* \brief 获取网关列表
* \return 网关信息
*/
void GYListManager::full_ping_list(Cmd::stPingList* cmd,const GameZone_t& gameZone)
{
	Zebra::logger->debug("GYListManager::full_ping_list");
	zMutex_scope_lock scope_lock(mlock);
	GYList *ret = NULL;
	const int per_num = 5;   // 档数
	int server_num = gyData.count(gameZone);
	int max_per = server_num * 2000;  // 最大人数
	int per_per = max_per/per_num; // 分成五档,每一档的人数
	int total_personal = 0; // 该区总人数
	int i=0;

	std::pair<GYListContainer_iterator,GYListContainer_iterator> hps = gyData.equal_range(gameZone);

	cmd->zone_id = gameZone.id;
	//      Cmd::ping_element* tempElement = cmd->ping_list;

	for (GYListContainer_iterator it = hps.first; it != hps.second; ++it,i++)
	{
		ret = &(it->second);
		if (state_none == ret->state)
		{
			if (i<server_num)
			{
				strncpy((char*)cmd->ping_list.gateway_ip,(char*)ret->pstrIP,15);
				total_personal += ret->wdNumOnline;
			}
			else
			{
				break;
			}
		}
	}

	for (int i=0; i<5; i++)
	{
		if (total_personal>=per_per*i && total_personal<(per_per*(i+1)-1))
		{
			cmd->ping_list.state = i;
			break;
		}
	}
}

bool GYListManager::verifyVer(const GameZone_t &gameZone,DWORD verify_client_version,BYTE &retcode)
{
	Zebra::logger->debug("GYListManager::verifyVer");
#ifdef _ALL_SUPER_GM
	return true;
#endif
	zMutex_scope_lock scope_lock(mlock);
	bool retval = false;
	GYList *ret = NULL,*tmp = NULL;
	std::pair<GYListContainer_iterator,GYListContainer_iterator> hps = gyData.equal_range(gameZone);
	for(GYListContainer_iterator it = hps.first; it != hps.second; ++it)
	{
		tmp = &(it->second);
		if (state_none == tmp->state
			&& (NULL == ret
			|| ret->wdNumOnline >= tmp->wdNumOnline))
		{
			ret = tmp;
		}
	}
	if (NULL == ret)
	{
		retcode = Cmd::LOGIN_RETURN_GATEWAYNOTAVAILABLE;
		Zebra::logger->error("����û�п�Ŷ,game:%u zone:%u",gameZone.game,gameZone.zone);
	}
	else if (ret->zoneGameVersion && ret->zoneGameVersion != verify_client_version)
	{
		Zebra::logger->error("�ͻ�������û��ͨ���汾����֤");
		retcode = Cmd::LOGIN_RETURN_VERSIONERROR;
	}
	else
	{
		retval = true;
	}
	return retval;
}

