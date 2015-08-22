#ifndef _zProperties_h_
#define _zProperties_h_

#include <iostream>
#include <string>
#include <algorithm>
#include <cctype>
#include <ext/hash_map>
#include "zString.h"
#include "zType.h"
/**
* \brief ���Թ������������������Թؼ��ֺ�ֵ��ʹ���ַ��������ؼ��ֲ����ִ�Сд
*
*/
class zProperties
{

public:

	/**
	* \brief ��ȡһ������ֵ
	*
	* \param key �ؼ���
	* \return ������ؼ��ֶ�Ӧ������ֵ
	*/
	const std::string &getProperty(const std::string &key)
	{
		return properties[key];
	}

	/**
	* \brief ����һ������
	*
	* \param key �ؼ���
	* \param value �ؼ��ֶ�Ӧ������
	*/
	void setProperty(const std::string &key,const std::string &value)
	{
		properties[key] = value;
	}

	/**
	* \brief ���ز�������������ؼ��ֶ�Ӧ������ֵ
	*
	* \param key �ؼ���
	* \return ����ֵ
	*/
	std::string & operator[] (const std::string &key)
	{
		//fprintf(stderr,"properties operator[%s]\n",key.c_str());
		return properties[key];
	}

	/**
	* \brief ����洢����������ֵ
	*
	*/
	void dump(std::ostream &out)
	{
		property_hashtype::const_iterator it;
		for(it = properties.begin(); it != properties.end(); it++)
			out << it->first << " = " << it->second << std::endl;
	}

	DWORD parseCmdLine(const std::string &cmdLine);
	DWORD parseCmdLine(const char *cmdLine);

protected:

	/**
	* \brief hash����
	*
	*/
	struct key_hash : public std::unary_function<const std::string,size_t>
	{
	size_t operator()(const std::string &x) const
	{
	std::string s = x;
	__gnu_cxx::hash<const char *> H;
	// _Hash<string> H;
	//ת���ַ���ΪСд
	Zebra::to_lower(s);
	//tolower(s);
	//return H(s);
	return H(s.c_str());
	}
	};
	/**
	* \brief �ж������ַ����Ƿ����
	*
	*/
	struct key_equal : public std::binary_function<const std::string,const std::string,bool>
	{
	    bool operator()(const std::string &s1,const std::string &s2) const
	    {
		return strcasecmp(s1.c_str(),s2.c_str()) == 0;
	    }
	};

	/**
	* \brief �ַ�����hash_map
	*
	*/
	//typedef hash_map<std::string,std::string,key_hash,key_equal> property_hashtype;

	//typedef hash_map<std::string,std::string,key_hash,key_equal> property_hashtype;
	typedef __gnu_cxx::hash_map<std::string, std::string, key_hash, key_equal> property_hashtype;

	property_hashtype properties;      /**< �������Եļ�ֵ�� */

};

#endif
