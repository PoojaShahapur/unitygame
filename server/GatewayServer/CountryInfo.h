#ifndef __COUNTRYINFO_H_
#define __COUNTRYINFO_H_

#include "zSubNetService.h"
#include "Zebra.h"
#include "zMisc.h"
#include "SuperCommand.h"
#include "SessionClient.h"
#include <vector>
#include <string>
#include "SceneCommand.h"
#include "GateUser.h"

/**
* \brief 国家配置文件信息
*
*/
class CountryInfo
{
public:
	struct Info
	{
		DWORD countryid;
		DWORD function;
		int   type;   // [ranqd Add] 服务器类型，参考enum SERVER_TYPE
 		DWORD Online_Now;
 		DWORD Online_Max;
		std::string countryname;
		std::string mapname;
		Info()
		{
			countryid=0;
			function=0;
			Online_Now = 0;
			Online_Max = 2000; // 默认一个服务器最大在线2000人
		}
	};
private:
	typedef std::vector<Info> StrVec;
	typedef StrVec::iterator StrVec_iterator;
	StrVec country_info;
	// 国家排序锁
	zMutex mutex;
	DWORD country_order[100];

	struct CountryDic
	{
		DWORD id;
		char name[MAX_NAMESIZE];
		DWORD mapid;
		DWORD function;
		int  type;  //[ranqd Add] 国家类型
		CountryDic()
		{
			id=0;
			mapid=0;
			bzero(name,sizeof(name));
			function=0; 
		}
	};
	struct MapDic
	{
		DWORD id;
		char name[MAX_NAMESIZE];
		char filename[MAX_NAMESIZE];
		DWORD backto;
		MapDic()
		{
			id=0;
			bzero(name,sizeof(name));
			bzero(filename,sizeof(filename));
			backto=0;
		}
	};
	typedef std::map<DWORD,CountryDic> CountryMap;
	typedef CountryMap::iterator CountryMap_iter;
	typedef CountryMap::value_type CountryMap_value_type;
	CountryMap country_dic;
	typedef std::map<DWORD,MapDic> MapMap;
	typedef MapMap::value_type MapMap_value_type;
	typedef MapMap::iterator MapMap_iter;
	MapMap map_dic;

public:
	CountryInfo()
	{
		bzero(country_order,sizeof(country_order));
	}
	~CountryInfo(){}
	Info *getInfo(DWORD country_id);
	bool init();
	bool reload();
	int getCountrySize();
	int getCountryState( CountryInfo::Info cInfo );
	int getAll(char *buf);
	DWORD getCountryID(DWORD country_id);
	DWORD getRealMapID(DWORD map_id);
	const char *getRealMapName(const char *name);
	void setCountryOrder(Cmd::Session::CountrOrder *ptCmd);
	std::string getCountryName(DWORD country_id);
	std::string getMapName(DWORD country_id);
	bool isEnableLogin(DWORD country_id);
	bool isEnableRegister(DWORD country_id);
	void processChange(GateUser *pUser,Cmd::Scene::t_ChangeCountryStatus *rev);
	// [ranqd] 更新国家在线人数
	void UpdateCountryOnline( DWORD country_id, DWORD online_numbers ); 
};





#endif
