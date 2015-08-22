#include "CountryInfo.h"
#include "zXMLParser.h"
#include "Zebra.h"

/*


* \brief ���ݹ���id�õ�������Ϣ
*
*
* \param country_id: ����id
* \return ������Ϣ
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
* \brief �������ļ��ж�ȡ������Ϣ
*
* \return ��ȡ�Ƿ�ɹ�
*/
bool CountryInfo::reload()
{
	return true;
}
/**
* \brief �������ļ��ж�ȡ������Ϣ
*
* \return ��ȡ�Ƿ�ɹ�
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
					Zebra::logger->info("���ع�������(%d,%s,%d,%d,%d)",info.id,info.name,info.mapid,info.function,info.type);
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
			Zebra::logger->info("��ȡ������Ϣ:%s(%d),%s",
				(*iter).countryname.c_str(),(*iter).countryid,(*iter).mapname.c_str());
		}
	}
	return inited;
}

/**
* \brief ������id�Ƿ�Ϸ�
*
*
* \param country_id:����id
* \return ������ڸù���id���ع���id,���򷵻�-1
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
* \brief ���ݹ���id�õ���������
*
*
* \param country_id:����id
* \return �ҵ����ع������Ʒ��򷵻�""
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
* \brief ���߹���id�õ��ù��ҳ�����map������
*
*
* \param country_id:��������
* \return �����ص�ͼ����
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

