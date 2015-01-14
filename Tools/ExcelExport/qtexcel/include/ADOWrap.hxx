#ifndef _ADOWRAP_H
#define _ADOWRAP_H

#include "Platform.hxx"
#include <string>

#import "C:\Program Files\Common Files\System\ado\msado15.dll" \
	no_namespace rename("EOF","adoEOF") \
	rename("BOF","adoBOF")

BEGIN_NAMESPACE

class Table;

class ADOWrap
{
public:
	// 添加一个指向Connection对象的指针
	_ConnectionPtr m_pConnection;
	// 添加一个指向Recordset 对象的指针
	_RecordsetPtr m_pRecordset;
	// 总的记录行数    
	int m_count;	// 数据表中总的行数 

public:
	ADOWrap();
	~ADOWrap();

	bool opemDB(Table* tableItem);
	void closeDB();
	bool isAdoEOF();
	void moveNext();

	unsigned int getCollectUInt(std::string fieldName);
	_variant_t getCollect(const char* fieldName);
};

END_NAMESPACE

#endif