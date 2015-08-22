#ifndef _DBMetaData_h_
#define _DBMetaData_h_
#include <mysql.h>
#include <pthread.h>
#include <unistd.h>
#include <zlib.h>
#include <iostream>
#include <ext/hash_map>
#include <sstream>
#include <string>
#include <map>
#include "zDBConnPool.h"
/**
* \brief �ֶ���Ϣ��
*
* ά���ֶ����Ƽ����ͣ�Ŀǰϵͳֻ�ܴ���MYSQL�����б�׼���ͣ�
*
* ����֧��Zebra�ж����ZIP,ZIP2��������
* ����֧�ָ�����ֶ����ͣ����޸�zMysqlDBConnPool::exeSelect,exeUpdate,exeInsert,exeDelete�ĸ�����
*
*/

class DBField
{
public:
	/**
	* \brief ���췽��
	*
	*
	*/
	DBField(int fieldType,const std::string& fieldName) 
		: type(fieldType),name(fieldName)
	{
	}

	/**
	* \ brief ��������
	*
	*/
	~DBField(){}

	/// �ֶ�����
	int type;

	/// �ֶ�����
	std::string name;
};


/**
* \brief �ֶ�����
*
* ά��һ����������ֶ�
*
*/
class DBFieldSet
{
public:
	/**
	* \brief Ĭ�Ϲ��췽��
	*
	*/
	DBFieldSet(){}


	/**
	* \brief ��ʼ�������Ĺ��췽��
	*
	* ��֧����ʽ����ת��
	*/
	explicit DBFieldSet(const std::string& tbn) : tableName(tbn)
	{
	}


	/**
	* \brief ��������
	*
	*/
	virtual ~DBFieldSet();

	/**
	* \brief ȡ�ֶθ���
	*
	* \return �����ֶθ���
	*/    
	DWORD size();


	/**
	* \brief ����operator[]�����
	*
	* \param pos�� ָ���������ĳ���ֶε�λ�� 
	*
	* \return ����ҵ����ֶ��򷵻ظ��ֶε�ָ�룬���û�ҵ����򷵻�NULL
	*/
	DBField* operator[](DWORD pos);


	/**
	* \brief ����operator[]�����
	*
	* \param name�� ָ���������ĳ���ֶε�����
	*
	* \return ����ҵ����ֶ��򷵻ظ��ֶε�ָ�룬���û�ҵ����򷵻�NULL
	*/
	DBField* operator[](const std::string& name);



	DBField* getField(unsigned int pos)
	{
	    return fields[pos];
	}
	/**
	* \brief �����µ��ֶ�
	*
	*  �ֶ�����Ŀǰ֧����������:
	*
	*  FIELD_TYPE_TINY TINYINT field
	*  FIELD_TYPE_SHORT SMALLINT field
	*  FIELD_TYPE_LONG INTEGER field
	*  FIELD_TYPE_INT24 MEDIUMINT field
	*  FIELD_TYPE_LONGLONG BIGINT field
	*  FIELD_TYPE_DECIMAL DECIMAL or NUMERIC field
	*  FIELD_TYPE_FLOAT FLOAT field
	*  FIELD_TYPE_DOUBLE DOUBLE or REAL field
	*  FIELD_TYPE_TIMESTAMP TIMESTAMP field
	*  FIELD_TYPE_DATE DATE field
	*  FIELD_TYPE_TIME TIME field
	*  FIELD_TYPE_DATETIME DATETIME field
	*  FIELD_TYPE_YEAR YEAR field
	*  FIELD_TYPE_STRING CHAR field
	*  FIELD_TYPE_VAR_STRING VARCHAR field
	*  FIELD_TYPE_BLOB BLOB or TEXT field 
	*  FIELD_TYPE_SET SET field
	*  FIELD_TYPE_ENUM ENUM field
	*  FIELD_TYPE_NULL NULL-type field
	*  FIELD_TYPE_CHAR Deprecated; use FIELD_TYPE_TINY instead
	*
	* \param fieldType: �ֶ�����
	* \param fieldName: �ֶ�����
	*
	*
	*/
	bool addField(int fieldType,const std::string& fieldName);

	/**
	* \brief �ṩ��һ������ֶεķ���
	*
	*  ����addField
	*/
	bool addField(DBField* field);

	/**
	* \brief ���ø��ֶμ���֮�����ı���
	*
	* ��ͨ��Ĭ�Ϲ��캯�����ɵĶ����������ʽ���øú�����������
	*
	* \param name������
	*/
	void setTableName(const std::string& name);

	/**
	* \brief �õ�����
	*
	* \return ���ر���
	*/
	const char* getTableName() const
	{
		return tableName.c_str();  
	}

protected:
	/// �ֶ�����
	std::vector<DBField*> fields;

	/// ����
	std::string tableName;  
};


/**
* \brief ��ṹ������
*
* �������Builderģʽ����֧�ֲ�ͬ���ݿ�ı�ṹ����
*
*/
class DBMetaData
{
public:
	/**
	* \brief builder������ͨ��������������������ɶ�Ӧ��ʵ��
	*
	* \param type ���ݿ����ͣ�Ŀǰֻ֧��MYSQL������ʱ����Ϊ�ա�Ҳ������"MYSQL"
	* 
	* \return  ���ػ���ָ��
	*/
	static DBMetaData* newInstance(const char* type);

	/**
	* \brief Ĭ�Ϲ��캯��
	*
	*/
	DBMetaData()
	{
	}

	/**
	* \brief ��������
	*
	*/
	virtual ~DBMetaData();

	/**
	* \brief ��ʼ����ṹ
	*
	* �������ݿ����ӣ���ȡ�ø����ݿ������б�ı�ṹ
	*  
	* \param url:  ���ݿ����Ӵ�
	*/
	virtual bool init(const std::string& url) = 0;

	/**
	* \brief ͨ��ָ����������ȡ�ñ�ı�ṹ
	*
	* \param tableName: ����
	*
	* \return ����ҵ��ñ����ر�ṹָ��,���򣬷���Ϊ��
	*/
	DBFieldSet* getFields(const std::string& tableName);

protected:

	typedef std::map<std::string,DBFieldSet*> TableManager;
	typedef TableManager::iterator TableMember;
	typedef TableManager::value_type valueType;

	/// �ڲ���ṹ������
	TableManager tables;
};


class DBVarType
{
public:
	DBVarType()
	{
		val_us = 0;
		val_short = 0;
		val_int = 0;
		val_dword = 0;
		val_qword = 0;
		val_sqword = 0;
		val_long = 0;

		val_float = 0.0;
		val_double = 0.0;
		val_byte = 0;

		val_pstr = NULL;
		val_bin.clear();
		valid = false;
	}

	operator WORD() const
	{
		return val_us;
	}

	operator short() const
	{
		return val_short;
	}

	operator int() const
	{
		return val_int;
	}

	operator DWORD() const
	{
		return val_dword;
	}

	operator QWORD() const
	{
		return val_qword;
	}

	operator SQWORD() const
	{
		return val_sqword;
	}

	operator long() const
	{
		return val_long;
	}

	operator float() const
	{
		return val_float;
	}

	operator double() const
	{
		return val_double;
	}


	operator const char*() const
	{
		return val_pstr;
	}

	operator BYTE() const
	{
		return val_byte;
	}
	
	const char* getBin() const
	{
	    return &val_bin[0];
	}
	DWORD getBinSize()
	{
	    return val_bin.size();
	}
	void setValid(bool value)
	{
		valid = value;
	}

	bool isValid()
	{
		return valid;
	}

	WORD val_us;
	short val_short;
	int val_int;
	DWORD val_dword;
	QWORD val_qword;
	SQWORD val_sqword;
	long val_long;
	float val_float;
	double val_double;
	BYTE val_byte;

	const char* val_pstr;
	//const BYTE* val_ustr;
	std::vector<char> val_bin;
	bool valid;
};
/**
* \brief ��¼��
*
* ά��һ�����ݿ��¼
*/
class DBRecord
{
public:
	/**
	* \brief Ĭ�Ϲ��췽��
	*/
	DBRecord()
	{
		//      std::cout << "DBRecord" << std::endl;
		fields = NULL;
	}

	/**
	* \brief ��������
	*/
	virtual ~DBRecord()
	{
		//      std::cout << "~DBRecord" << std::endl;
		field.clear();
	}

	/**
	* \brief ����operator[]�����
	*
	* ��ͨ��ָ���ֶ�������ȡ����ֶε�ֵ��
	* ������ֶ�����Ϊ��ֵ�ͣ�ͨ���ú���Ҳ�ɷ�����ֵ��Ӧ�ó���Ա��Ҫ�Լ�������Ӧ��������ת��
	* ������ʽ�����������ƥ���get����
	*
	* \param name: �ֶ����������ִ�Сд
	* 
	* \return ������ֶδ��ڣ��򷵻���ֵ����������ڣ��򷵻�ΪNULL
	*/
	DBVarType operator[](const std::string& name);



	/**
	* \brief ����operator[]�����
	*
	* ͨ��ָ���е�λ�û�ȡ��ֵ�����Ƽ��ڶ�λ���������Ĵ�����ʹ�ã���Ϊ�е�λ�ò�һ���ǹ̶��ġ�
	* 
	* \param idx: ָ����λ��
	*
	* \return ���ָ��������ֵ���򷵻���ֵ�����򣬷���ΪNULL
	*/
	const char* operator[](DWORD idx);

	/**
	* \brief �����
	*
	* ע�⣺�ڶ�����������������ΪNULLֵ������ᵼ�³������
	*
	* \param fieldName: �ֶ�����
	* \param value: �ֶ�ֵ
	* 
	*/
	template <typename X>
	void put(const char* fieldName,const X& value,bool binary = false)
	{
		if (fieldName == NULL)
		{
			return;
		}

		std::ostringstream oss;
		std::string tempname = fieldName;

		std::transform(tempname.begin(),tempname.end(),
			tempname.begin(),
			::toupper);

		if (binary)
		{

		}
		else
		{
			oss << value;
		}

		field_it member = field.find(tempname);

		if (member==field.end())
		{
			field.insert(valType(tempname,oss.str()));
		}
		else
		{
			field.erase(member);
			field.insert(valType(tempname,oss.str()));
		}

	}

	/**
	* \brief �����
	*
	* ע�⣺�ڶ�����������������ΪNULLֵ������ᵼ�³������
	*
	* \param fieldName: �ֶ�����
	* \param value: �ֶ�ֵ
	* 
	*/
	/*template <> void put<const char*>(const char* fieldName,const char* value)
	{
	if (fieldName == NULL)
	{
	return;
	}

	std::ostringstream oss;
	std::string tempname = fieldName;

	std::transform(tempname.begin(),tempname.end(),
	tempname.begin(),
	::toupper);

	if (value)
	{
	oss << value;
	}

	field_it member = field.find(tempname);

	if (member==field.end())
	{
	field.insert(valType(tempname,oss.str()));
	}
	else
	{
	field.erase(member);
	field.insert(valType(tempname,oss.str()));
	}

	}*/

	/**
	* \brief �����
	*
	*  ��Ҫ���ڵ����DBRecord����,SELECTʱ��column,groupby�Ӿ䡣
	*  ���һ���У�������е�ֵΪ��
	*
	* \param fieldName: �ֶ�����
	* 
	*/
	void put(const char* fieldName);

	/**
	* \brief ���������
	*
	*/
	void clear()
	{
		field.clear();
	}

	/**
	* \brief ��ȡָ���ֶε�ֵ��ͨ�÷���
	* 
	* �ɻ�������ֶ����͵�ֵ�������ַ�������ʽ������ֵ��
	* ���谴�ֶ����ͻ����ֵ���������Ӧ��get����
	*/
	DBVarType get(const std::string& fieldName);


	//bool getBool(const std::string& fieldName);
	//double getDouble(const std::string& fieldName);
	//int    getInt(const std::string& fieldName);

	/**
	* \brief �ж�ĳ���ֶ��Ƿ���Ч 
	*
	* \param fieldName: �ֶ�����

	* \return ����ü�¼�������ֶΣ�����TRUE,����ΪFALSE
	*/
	bool find(const std::string& fieldName);

	/**
	* \brief ��ȡ�ü�¼������
	*
	* \return ���ظü�¼��������Ϊ0��ʾû���С�
	*/
	DWORD size()
	{
		return field.size();
	}


	DBFieldSet* fields;

private:
	typedef std::map<std::string,std::string> FIELD;
	typedef FIELD::value_type valType;
	typedef FIELD::iterator field_it;

	/// �ֶ�-ֵ��
	FIELD field;
};


class DBRecordSet
{
public:
	/**
	* \brief Ĭ�Ϲ��캯��
	*
	*/
	DBRecordSet()
	{
		//      std::cout << "DBRecordSet" << std::endl;
	}

	/**
	* \brief ��������
	*
	*/
	virtual ~DBRecordSet();

	/**
	* \brief ����operator[]�����
	*
	* ͨ��ָ������������ȡ��Ӧ�ļ�¼
	*
	* \param idx:ָ��������
	*
	* \return ���ָ����������Ч���򷵻���Ӧ�ļ�¼ָ�룬�����Ч���򷵻�NULL
	*/
	DBRecord* operator[](DWORD idx);

	/**
	* \brief ��ȡ��¼��
	*
	* \return ���ؼ�¼�������û�м�¼������Ϊ0
	*/
	DWORD size();

	/**
	* \brief ��ȡ��¼��
	*
	* \return ���ؼ�¼�������û�м�¼������Ϊ0
	*/
	bool empty(){return recordSet.empty();}

	/**
	* \brief ��Ӽ�¼
	*
	*/
	void put(DBRecord* rec);


	/**
	* \brief ������м�¼
	*
	*/
	void clear()
	{
		recordSet.clear();
	}

	/**
	* \brief ��ȡָ������
	*
	* ���������ص�operator[]�������ͬ��
	*/
	DBRecord* get(DWORD idx);

private:
	/// ��¼��
	std::vector<DBRecord*>    recordSet;
};
#endif

