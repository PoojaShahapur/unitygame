#ifndef _CTASK_H
#define _CTASK_H

#include <string>	// include string
#include <vector>	// include vector
#include "tinyxml2.h"	// include xml
#include "EnValue.hxx"
#include "TabelAttr.hxx"

// import namespace
using namespace std;
//using namespace tinyxml2;

#include "Platform.hxx"
BEGIN_NAMESPACE

/**
* @brief field 字段
*/
class XmlField
{
public:
	const char* m_fieldName;
	const char* m_fieldType;
	int m_fieldSize;
	int m_fieldBase;
	const char* m_defaultValue;

public:
	XmlField();
};

/**
 * @brief Table 内容
 */
class Table
{
public:
	const char* m_tableName;		// 输出的表的名字，没有扩展名字
	const char* m_ExcelFile;		// excel 文件名字和扩展名字
	const char* m_lpszsheetname;	// 表单的名字
	const char* m_lpszDB;			// 数据库的名字
	const char* m_lpszTable;		// 表的名字

	const char* m_lpId;			// id 过滤字段
	string m_strOutput;		// 临时的表的描述输出字段
	TableAttr m_tableAttr;	// 定义的表的属性
	char* m_strOutputFile;	// 输出的表的目录和名字和扩展名字
	std::string m_strStructDef;		// 表的结构体描述
	std::string m_strExcelDir;		// Excel 文件所在的目录
	std::string m_strExcelDirAndName;	// Excel 文件的目录和名字

	EnExcelType m_enExcelType;
	bool m_bExportTable;			// 是否导出这个表
	std::string m_strStructDef;		// 表的定义
	bool m_bRecStructDef;	// 是否已经生成表的定义

	std::vector<XmlField*> m_fieldsList;

public:
	Table();
	static void parseXML(tinyxml2::XMLElement* pXmlEmtFields, std::vector<XmlField*>& fieldsList);
	static bool buildTableDefine(std::string& strStructDef, const char* lpszTableName, std::vector<XmlField*>& fieldsList, bool& bRecStructDef);
};

/**
 * @brief package a table
 */
class CPackage
{
protected:
	string m_xml;		// xml path + name
	string m_output;	// output path

public:
	CPackage();
	~CPackage();

	string getXml();
	string getOutput();

	void setXml(string xml);
	void setOutput(string output);
	void initByXml(tinyxml2::XMLElement* elem);
	bool loadTableXml(std::vector<Table*>& tablesList);
	void destroy();
};


/**
 * @brief one solution
 */
class CSolution
{
protected:
	string m_name;		// solution name
	string m_cmd;		// excute cmd, usually is a bat file
	string m_xmlRootPath;		// Xml 文件根目录
	string m_defaultOutput;		// 默认的输出目录
	vector<CPackage*> m_lstPack;	// need pack list

public:
	CSolution();
	~CSolution();

	string getName();
	string getCmd();

	void setName(string name);
	void setCmd(string cmd);
	void initByXml(tinyxml2::XMLElement* elem);
	vector<CPackage*>& getPackLst();
	void loadTableXml(std::vector<Table*>& tablesList);
	void destroy();
};

/**
 * @brief task
 */
class CTask
{
protected:
	vector<CSolution*> m_lstSolution;
	std::vector<Table*> m_tablesList;

public:
	CTask();
	~CTask();

	vector<CSolution*>& getSolutionLst();
	void readXML();		// read config.xml
	void destroy();

	std::vector<Table*>& getTableList();
};

END_NAMESPACE

#endif