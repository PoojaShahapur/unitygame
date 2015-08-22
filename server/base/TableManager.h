/*************************************************************************
 Author: wang
 Created Time: 2015年04月02日 星期四 10时19分31秒
 File Name: base/TableManager.h
 Description: 
 ************************************************************************/
#ifndef _TableManager_h
#define _TableManager_h

#include "zSingleton.h"
#include <string>
#include <map>
#include <vector>
/**
 * \brief 表名管理器
*/
class TableManager : public Singleton<TableManager>
{
    public:
	friend class SingletonFactory<TableManager>;
	TableManager();
	~TableManager();
	static unsigned int hashString(const char* __s);
	static unsigned int dbHashCode(const void* anyArg);
	static unsigned int tableHashCode(const void* anyArg);
	static unsigned int get_dummy_hash(const void* anyArg);

	const char* TableName(const std::string &tablename, const unsigned int hash) const;
	bool addTableNames(const std::string &tablename);

	static unsigned int myDBHashCode(const void *anyArg);
	static unsigned int mySaleDBHashCode(const void *anyArg);
	const char* myTableName(const std::string &tablename, const int count) const;
	bool myAddTableNames(const std::string &tablename, const int count);

    private:

	const int dbCount;
	const int tableCount;
	const int tablePerDB;
	const int tableSize;

	typedef std::map<const std::string, std::vector<std::string> > tableNames_mapper;
	typedef tableNames_mapper::value_type tableNames_valType;
	typedef tableNames_mapper::iterator tableNames_iter;
	typedef tableNames_mapper::const_iterator tableNames_const_iter;
	typedef std::pair<tableNames_iter, bool> tableNames_pairType;
	tableNames_mapper TableNames;

};
#endif

