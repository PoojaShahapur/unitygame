/*************************************************************************
 Author: wang
 Created Time: 2015年04月02日 星期四 10时31分23秒
 File Name: base/TableManager.cpp
 Description: 
 ************************************************************************/
#include "TableManager.h"
#include "Zebra.h"
TableManager::TableManager() :dbCount(atoi(Zebra::global["dbCount"].c_str())),
    tableCount(atoi(Zebra::global["tableCount"].c_str())),
    tablePerDB(tableCount/dbCount),
    tableSize(atoi(Zebra::global["tableSize"].c_str()))
{
    
}

TableManager::~TableManager()
{
}

unsigned int TableManager::hashString(const char* __s)
{
    unsigned int __h = 0;
    for(; *__s; ++__s)
    {
	__h = 5*__h + *__s;
    }
    return __h;
}

unsigned int TableManager::dbHashCode(const void* anyArg)
{
    return (((*((unsigned int *)anyArg))%getMe().tableCount) /getMe().tablePerDB);
}


unsigned int TableManager::myDBHashCode(const void* anyArg)
{
    return (*((unsigned int *)anyArg))% getMe().dbCount;
}

unsigned int TableManager::mySaleDBHashCode(const void* anyArg)
{
    return (*(unsigned int *)anyArg);
}

const char* TableManager::myTableName(const std::string &tablename, const int count) const
{
    std::string UPtablename = tablename;
    std::transform(UPtablename.begin(), UPtablename.end(), UPtablename.begin(), toupper);
    tableNames_const_iter it = TableNames.find(UPtablename);
    if(it != TableNames.end())
    {
	return (it->second.at(count).c_str());
    }
    else
    {
	return NULL;
    }
}

bool TableManager::myAddTableNames(const std::string &tablename, const int count)
{
    std::vector<std::string> full_tablenames;
    std::string UPtablename = tablename;
    std::transform(UPtablename.begin(), UPtablename.end(), UPtablename.begin(), toupper);
    for(int i=0; i<count; i++)
    {
	char buffer[80];
	bzero(buffer, sizeof(buffer));
	snprintf(buffer, sizeof(buffer)-1, "%s%04u", UPtablename.c_str(), i);
	full_tablenames.push_back(buffer);
    }
    tableNames_pairType pt = TableNames.insert(tableNames_valType(UPtablename, full_tablenames));
    return pt.second;
}

unsigned TableManager::tableHashCode(const void* anyArg)
{
    return ((*(unsigned int *)anyArg) % getMe().tableCount);
}

unsigned TableManager::get_dummy_hash(const void* anyArg)
{
    return (((*(unsigned int *)anyArg)/getMe().tableSize)-1) % 256;
}

const char* TableManager::TableName(const std::string & tablename, const unsigned int hash) const
{
    std::string UPtablename = tablename;
    std::transform(UPtablename.begin(), UPtablename.end(), UPtablename.begin(), toupper);
    tableNames_const_iter it = TableNames.find(UPtablename);
    if(it != TableNames.end())
    {
	unsigned int hc = tableHashCode(&hash);
	return it->second.at(hc).c_str();
    }
    else
    {
	return NULL;
    }
}


bool TableManager::addTableNames(const std::string &tablename)
{
    std::vector<std::string> full_tablenames;
    std::string UPtablename = tablename;
    std::transform(UPtablename.begin(), UPtablename.end(), UPtablename.begin(), toupper);
    for(int i=0; i<tableCount; i++)
    {
	char buffer[80];
	bzero(buffer, sizeof(buffer));
	snprintf(buffer, sizeof(buffer)-1, "%s%04u", UPtablename.c_str(), i);
	full_tablenames.push_back(buffer);
    }
    tableNames_pairType pt = TableNames.insert(tableNames_valType(UPtablename, full_tablenames));
    return pt.second;
}
