#ifndef _EXCELTBL_H
#define _EXCELTBL_H

//#include <Windows.h>
//#include <AfxWin.h>
//#include <afx.h>
#include "tinyxml2.h"
#include <QtCore/QtCore>
#include <cstring>		// std::string
#include <cstdio>		// File
#include <tchar.h>		// _T
#include "TabelAttr.hxx"
#include <vector>
#include "PropertyIns.hxx"
#include "PropertyStr.hxx"
#include "DataItem.hxx"
#include <cwchar>				// wchat_t
#include <windows.h>			// WCHAR

#include "Platform.hxx"

//using namespace tinyxml2;

BEGIN_NAMESPACE

//class DataItem;

class ExcelExport
{
protected:
	std::string m_xmlPath;	// 绝对路径的文件名字   
	std::string m_tblPath;	// tbl 输出路径  
	std::string m_strOutput;	// 字符串日志输出    
	TableAttr m_tableAttr;	// 定义的表的属性
	QMutex mutex;
	WCHAR* m_wBuf;			// 临时数据
	char* m_bytes;

public:
	ExcelExport();
	~ExcelExport();
	void setXmlPath(std::string file);
	void setOutputPath(std::string path);
	bool exportExcel();
	QString UTF82GBK(const QString &inStr);

	// 导出 Excel 到 Property Vector
	virtual bool exportExcelInternal(
					tinyxml2::XMLElement* pXmlEmtFields,
					const char* lpszExcelFile,
					const char* lpszDB,
					const char* lpszTable,
					const char* lpszOutputFile,
					const char* lpszTableName,
					const char* lpszsheetname,		//excel 中表单的名字   
					std::string& strStructDef,
					const char* provider = "Provider=Microsoft.Jet.OLEDB.4.0;",		// 数据引擎提供者  
					const char* extendedProperties = "Extended Properties=\'Excel 8.0;HDR=Yes;IMEX=1\';"		// 扩展属性   
				);

	// 导出 Property Vector 到文件
	virtual void exportPropertyVec2File(const char* lpszOutputFile, std::vector<DataItem*>& _rowList, bool isClient);
	void exportPropertyVec2FileServer(std::vector<DataItem*>& _rowList, ByteBuffer& byteBuffer);
	void exportPropertyVec2FileClient(std::vector<DataItem*>& _rowList, ByteBuffer& byteBuffer);
	void exportPropertyVec2FileMobile(std::vector<DataItem*>& _rowList, ByteBuffer& byteBuffer);

	template <class T>
	void addProperty(DataItem* row, T value, bool isIDField = false)
	{
		PropertyIns<T>* propIns = new PropertyIns<T>();
		propIns->m_propData = value;
		propIns->m_isIDField = isIDField;
		row->getPropVec().push_back((PropertyBase*)propIns);
	}

	void addPropertyStr(DataItem* row, const char* value, size_t cfgLen)
	{
		PropertyStr* propIns = new PropertyStr();
		propIns->m_propData = value;
		propIns->m_cfgLen = cfgLen;
		row->getPropVec().push_back((PropertyBase*)propIns);
	}
};

END_NAMESPACE

#endif	// EXCELTBL_H