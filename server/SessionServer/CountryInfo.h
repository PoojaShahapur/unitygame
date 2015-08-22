#ifndef __COUNTRYINFO_H_
#define __COUNTRYINFO_H_

#include <vector>
#include <string>
#include <map>
#include "zType.h"
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
		std::string countryname;
		std::string mapname;
		Info()
		{
			countryid=0;
		}
	};
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
private:
	typedef std::vector<Info> StrVec;
	typedef StrVec::iterator StrVec_iterator;
	StrVec country_info;

public:
	CountryInfo()
	{
	}
	~CountryInfo(){}
	Info *getInfo(DWORD country_id);
	bool init();
	bool reload();
	DWORD getCountryID(DWORD country_id);
	std::string getMapName(DWORD country_id);
	std::string getCountryName(DWORD country_id);
};



#endif

