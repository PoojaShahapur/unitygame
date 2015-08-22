#include "CountryInfo.h"
#include "zXMLParser.h"
#include "Zebra.h"

/*


* \brief 根据国家id得到国家信息
*
*
* \param country_id: 国家id
* \return 国家信息
*/
CountryInfo::Info *CountryInfo::getInfo(DWORD country_id)
{
	for(StrVec_iterator iter = country_info.begin() ; iter != country_info.end() ; iter ++)
	{
		if (iter->countryid == country_id)
		{
			return &*iter;
		}
	}
	return NULL;
}
/**
* \brief 从配置文件中读取国家信息
*
* \return 读取是否成功
*/
bool CountryInfo::reload()
{
	return true;
}
/**
* \brief 从配置文件中读取国家信息
*
* \return 读取是否成功
*/
bool CountryInfo::init()
{
	bool inited = false;
	zXMLParser parser;

	if (parser.initFile(Zebra::global["configdir"] + "scenesinfo.xml"))
	{
		xmlNodePtr root=parser.getRootNode("ScenesInfo");
		xmlNodePtr countryNode=parser.getChildNode(root,"countryinfo");
		if (countryNode)
		{
			country_info.clear();
			xmlNodePtr subnode = parser.getChildNode(countryNode,"country");
			while(subnode)
			{
				if (strcmp((char*)subnode->name,"country") == 0)
				{
					CountryDic info;
					bzero(&info,sizeof(info));
					parser.getNodePropNum(subnode,"id",&info.id,sizeof(info.id));
					parser.getNodePropStr(subnode,"name",info.name,sizeof(info.name));
					parser.getNodePropNum(subnode,"mapID",&info.mapid,sizeof(info.mapid));
					parser.getNodePropNum(subnode,"function",&info.function,sizeof(info.function));
					Zebra::logger->info("加载国家名称(%d,%s,%d,%d,%d)",info.id,info.name,info.mapid,info.function,info.type);
					Info info_1;
					info_1.countryid = info.id;
					info_1.countryname = info.name;
					country_info.push_back(info_1);
				}
				subnode = parser.getNextNode(subnode,NULL);
			}
		}
	}
	if (inited)
	{
		for(StrVec_iterator iter = country_info.begin() ; iter != country_info.end() ; iter++)
		{
			Zebra::logger->info("读取国家信息:%s(%d),%s",
				(*iter).countryname.c_str(),(*iter).countryid,(*iter).mapname.c_str());
		}
	}
	return inited;
}

/**
* \brief 检查国家id是否合法
*
*
* \param country_id:国家id
* \return 如果存在该国家id返回国家id,否则返回-1
*/
DWORD CountryInfo::getCountryID(DWORD country_id)
{
	Info *info = getInfo(country_id);
	if (info)
	{
		return info->countryid;
	}
	return (DWORD)-1;
}
/**
* \brief 根据国家id得到国家名称
*
*
* \param country_id:国家id
* \return 找到返回国家名称否则返回""
*/
std::string CountryInfo::getCountryName(DWORD country_id)
{
	Info *info = getInfo(country_id);
	if (info)
	{
		return info->countryname;
	}
	return "";
}
/**
* \brief 更具国家id得到该国家出生地map的名称
*
*
* \param country_id:国家名称
* \return 出生地地图名称
*/
std::string CountryInfo::getMapName(DWORD country_id)
{
	Info *info = getInfo(country_id);
	if (info)
	{
		return info->mapname;
	}
	return "";
}

