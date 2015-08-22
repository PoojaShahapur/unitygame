/**
* \brief ��ṹ��������������ʵ��
*
* 
*/
#include <mysql.h>
#include "zMetaData.h"
#include "zType.h"
#include "Zebra.h"
#include "zMutex.h"
#include "zNoncopyable.h"



using namespace Zebra;

/**
* \brief ��������
*
*/
DBMetaData:: ~DBMetaData()
{
	TableMember it;
	for (it=tables.begin(); it!=tables.end(); it++)
	{
		DBFieldSet* temp = it->second;
		SAFE_DELETE(temp);
	}  
}

class MySQLMetaData : public DBMetaData
{
	/**
	* \brief ��ʼ����ṹ
	*
	* �������ݿ����ӣ���ȡ�ø����ݿ������б�ı�ṹ
	*  
	* \param url:  ���ݿ����Ӵ�
	*/
	bool init(const std::string& url)
	{
		UrlInfo urlInfo(0,url,false);  
		//	MessageBox(NULL, "��ʼ��ȡ���ݱ�.", "", 0);
		if (!this->loadMetaDataFromDB(urlInfo))
		{
			return false;
		}

		// TODO:����һЩ��Ҫ��ʼ���Ĵ���д������
		//	MessageBox(NULL, "���ݱ��ȡ���.", "", 0);
		return true;
	}  

	/**
	* \brief ͨ��ָ�������ӣ��������ݱ�ṹ
	*
	*/
	bool loadMetaDataFromDB(const UrlInfo& url)
	{
		MYSQL* mysql_conn = NULL;
		MYSQL_RES* table_res = NULL;
		
		mysql_conn = mysql_init(NULL);

		if (mysql_conn==NULL)
		{
			logger->error("Initiate mysql error...");
			return false;
		}

		if (mysql_real_connect(mysql_conn,url.host,url.user,url.passwd,url.dbName,url.port,NULL,CLIENT_COMPRESS|CLIENT_INTERACTIVE)==NULL)
		{
			logger->error("loadMetaDataFromDB():connect mysql://%s:%u/%s failed...",url.host,url.port,url.dbName);
			logger->error("loadMetaDataFromDB():reason: %s",mysql_error(mysql_conn));
			return false;
		}

		logger->info("loadMetaDataFromDB():connect mysql://%s:%u/%s successful...",url.host,url.port,url.dbName);

		if (mysql_conn)
		{
			if((table_res = mysql_list_tables(mysql_conn, NULL)) == NULL)
			{
			
			}
			MYSQL_ROW row;
			while((row=mysql_fetch_row(table_res)))
			{
				this->addNewTable(mysql_conn, row[0]);
			}
			mysql_free_result(table_res);
		}
		if (mysql_conn)
		{
			mysql_close(mysql_conn);
		}
		

		return true;
	}

	/**
	* \brief ����һ���±�
	*/
	bool addNewTable(MYSQL* mysql_conn, const char* tableName)
	{	
		MYSQL_RES* field_res = NULL;
		field_res = mysql_list_tables(mysql_conn, NULL);
		if(field_res)
		{
		unsigned int num_fields = mysql_num_fields(field_res);
		MYSQL_FIELD* mysql_fields = NULL;
		mysql_fields = mysql_fetch_fields(field_res);
		DBFieldSet *fields = new DBFieldSet(tableName);
		if(!fields)
		{
		mysql_free_result(field_res);
		return false;
		}
		for(unsigned int i=0; i<num_fields; i++)
		{
			if(!fields->addField(mysql_fields[i].type, mysql_fields[i].name))
			{
			mysql_free_result(field_res);
		return false;
			}
		}
		tables.insert(valueType(tableName, fields));
		mysql_free_result(field_res);
		}
		else
		{
		return false;
		}
		return true;
	}
};

/**
* \brief ȡ�ֶθ���
*
* \return �����ֶθ���
*/    
DWORD DBFieldSet::size()
{
	return fields.size();
}

/**
* \brief ����operator[]�����
*
* \param pos�� ָ���������ĳ���ֶε�λ�� 
*
* \return ����ҵ����ֶ��򷵻ظ��ֶε�ָ�룬���û�ҵ����򷵻�NULL
*/
DBField* DBFieldSet::operator[] (DWORD pos)
{
	if (pos<0 || pos>=fields.size())
	{
		return NULL;
	}

	return fields[pos];
}

/**
* \brief ����operator[]�����
*
* \param pos�� ָ���������ĳ���ֶε�����
*
* \return ����ҵ����ֶ��򷵻ظ��ֶε�ָ�룬���û�ҵ����򷵻�NULL
*/
DBField* DBFieldSet::operator[](const std::string& name)
{
	for (DWORD i=0; i<fields.size(); i++)
	{
		DBField* ret = fields.at(i);

		if (ret)
		{
			if (ret->name == name)
			{
				return ret;
			}
		}
	}

	return NULL;
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
bool DBFieldSet::addField(int fieldType,const std::string& fieldName)
{
	std::string tempname = fieldName;

	transform(tempname.begin(),tempname.end(),
		tempname.begin(),
		toupper);

	DBField* field = new DBField(fieldType,tempname);

	if (field)
	{
		fields.push_back(field);
		return true;
	}

	return false;
}

/**
* \brief �ṩ��һ������ֶεķ���
*
*  ����addField
*/
bool DBFieldSet::addField(DBField* field)
{
	if (!field)
	{
		fields.push_back(field);
		return true;
	}

	return false;
}

/**
* \brief ��������
*
* �ͷſռ�
*/
DBFieldSet::~DBFieldSet()
{
	DWORD num_field = fields.size(); 

	for (DWORD i=0; i<num_field; i++)
	{
		SAFE_DELETE(fields[i]);
	}

}
/**
* \brief ͨ��ָ����������ȡ�ñ�ı�ṹ
*
* \param tableName: ����
*
* \return ����ҵ��ñ����ر�ṹָ��,���򣬷���Ϊ��
*/
DBFieldSet* DBMetaData::getFields(const std::string& tableName)
{
	std::string tempname = tableName;

	//free table name under windows is lower
	transform(tempname.begin(),tempname.end(),
		tempname.begin(),
		tolower);

	TableMember tm = tables.find(tempname);  
	if (tm!=tables.end())
	{
		return (DBFieldSet*)(tm->second);
	}

	return NULL;
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
DBVarType DBRecord::operator[](const std::string& name)
{
	/*  std::string tempname = name;

	transform(tempname.begin(),tempname.end(),
	tempname.begin(),
	toupper);

	field_it it = field.find(tempname);


	if (it != field.end())
	{
	return it->second.c_str();
	}

	return NULL;*/
	return this->get(name);
}

/**
* \brief ����operator[]�����
*
* ͨ��ָ���е�λ�û�ȡ��ֵ�����Ƽ��ڶ�λ���������Ĵ�����ʹ�ã���Ϊ�е�λ�ò�һ���ǹ̶��ġ�
* 
* \param idx: ָ����λ��
*
* \return ���ָ��������ֵ���򷵻���ֵ�����򣬷���ΪNULL
*/
const char* DBRecord::operator[](DWORD idx)
{
	field_it it;
	DWORD i=0;

	for (it = field.begin(); it!=field.end(); i++,it++)
	{
		if (idx == i)
		{
			return it->second.c_str();  
		}
	}

	return NULL;
}

/**
* \brief �����
*
* \param fieldName: �ֶ�����
* \param value: �ֶ�ֵ
* 
*/
void DBRecord::put(const char* fieldName)
{
	if (fieldName == NULL)
	{
		return;
	}

	std::string tempname = fieldName;

	transform(tempname.begin(),tempname.end(),
		tempname.begin(),
		toupper);


	field.insert(valType(tempname,""));

}

/**
* \brief ��ȡָ���ֶε�ֵ��ͨ�÷���
* 
* �ɻ�������ֶ����͵�ֵ�������ַ�������ʽ������ֵ��
* ���谴�ֶ����ͻ����ֵ���������Ӧ��get����
*/
DBVarType DBRecord::get(const std::string& fieldName)
{
	std::string tempname = fieldName;
	DBVarType ret;

	transform(tempname.begin(),tempname.end(),
		tempname.begin(),
		toupper);

	field_it it = field.find(tempname);

	if (it == field.end())
	{
		return ret;
	}

	if (fields)
	{
		DBField* fl = (*fields)[tempname];

		if (fl)
		{
			ret.setValid(true);

			switch (fl->type)
			{
			case FIELD_TYPE_TINY:
			 case FIELD_TYPE_SHORT:
			     case FIELD_TYPE_LONG:
			     case FIELD_TYPE_INT24:
			     case FIELD_TYPE_LONGLONG:
			     case FIELD_TYPE_DECIMAL:

				{// �������������ﴦ��
					ret.val_us = atoi(it->second.c_str());
					ret.val_short = atoi(it->second.c_str());
					ret.val_int = atoi(it->second.c_str());
					ret.val_dword = strtoul(it->second.c_str(),NULL,10);
					ret.val_qword = strtoul(it->second.c_str(),NULL,10);
					ret.val_sqword = strtoul(it->second.c_str(),NULL,10);
					ret.val_long = atol(it->second.c_str());
					ret.val_byte = atoi(it->second.c_str());
					break;
				}
			case FIELD_TYPE_FLOAT:
			case FIELD_TYPE_DOUBLE:
				{//���и����������ﴦ��`
					ret.val_float = atof(it->second.c_str());
					ret.val_double = atof(it->second.c_str());
					break;
				}
			case FIELD_TYPE_BLOB:
				{
				    ret.val_pstr = (const char*)&it->second[0];
				    ret.val_bin.resize(it->second.size());
				    memcpy(&ret.val_bin[0], &it->second[0], it->second.size());
				    break;
				}
			default:
				{// �����������Ͱ��ַ�������
					ret.val_pstr = it->second.c_str();
				}
			}
		}
	}
	else
	{
		ret.setValid(true);
		ret.val_pstr = it->second.c_str();
	}

	return ret;
}

/**
* \brief �ж�ĳ���ֶ��Ƿ���Ч 
*
* \param fieldName: �ֶ�����

* \return ����ü�¼�������ֶΣ�����TRUE,����ΪFALSE
*/
bool DBRecord::find(const std::string& fieldName)
{
	std::string tempname = fieldName;

	transform(tempname.begin(),tempname.end(),
		tempname.begin(),
		toupper);

	if (field.find(tempname) == field.end())
	{
		return false;
	}

	return true;

}

/**
* \brief ��������
*/
DBRecordSet::~DBRecordSet()
{
	//  DWORD num_record = recordSet.size();
	//  std::cout << "~DBRecordSet" << std::endl;
	for (std::vector<DBRecord*>::iterator pos = recordSet.begin(); pos!=recordSet.end(); pos++)
	{
		SAFE_DELETE(*pos);
	}
}

/**
* \brief ����operator[]�����
*
* ͨ��ָ������������ȡ��Ӧ�ļ�¼
*
* \param idx:ָ��������
*
* \return ���ָ����������Ч���򷵻���Ӧ�ļ�¼ָ�룬�����Ч���򷵻�NULL
*/
DBRecord* DBRecordSet::operator[](DWORD idx)
{
	return this->get(idx);
}

/**
* \brief ��ȡ��¼��
*
* \return ���ؼ�¼�������û�м�¼������Ϊ0
*/
DWORD DBRecordSet::size()
{
	return recordSet.size();
}

/**
* \brief ��Ӽ�¼
*
*/
void DBRecordSet::put(DBRecord* rec)
{
	recordSet.push_back(rec);
}

/**
* \brief ��ȡָ������
*
* ���������ص�operator[]�������ͬ��
*/
DBRecord* DBRecordSet::get(DWORD idx)
{
	return recordSet[idx];
}

/**
* \brief builder������ͨ��������������������ɶ�Ӧ��ʵ��
*
* \param ���ݿ����ͣ�Ŀǰֻ֧��MYSQL������ʱ����Ϊ�ա�Ҳ������"MYSQL"
* 
* \return  ���ػ���ָ��
*/
DBMetaData* DBMetaData::newInstance(const char* type)
{
	if (type == NULL || 0 == strcmp(type,"MYSQL"))
		return (new MySQLMetaData());
	else 
		return (new MySQLMetaData());
}

