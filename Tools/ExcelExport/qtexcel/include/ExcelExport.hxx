#ifndef _EXCELTBL_H
#define _EXCELTBL_H

//#include <Windows.h>
//#include <AfxWin.h>
//#include <afx.h>
#include "tinyxml.h"
#include <QtCore/QtCore>
#include <cstring>		// std::string
#include <cstdio>		// File
#include <tchar.h>		// _T
#include "TabelAttr.hxx"
#include <vector>
#include "PropertyIns.hxx"
#include "PropertyStr.hxx"
#include "DataItem.hxx"

#include "Platform.hxx"
BEGINNAMESPACE(NSExcelExport)

//class DataItem;

class ExcelExport
{
protected:
	QString m_xmlPath;	// 绝对路径的文件名字   
	QString m_tblPath;	// tbl 输出路径  
	QString m_strOutput;	// 字符串日志输出    
	TableAttr m_tableAttr;	// 定义的表的属性
	QMutex mutex;
public:
	ExcelExport();
	~ExcelExport();
	void setXmlPath(QString file);
	void setOutputPath(QString path);
	bool exportExcel();
	QString UTF82GBK(const QString &inStr);

	// 导出 Excel 到 Property Vector
	virtual bool exportExcel2PropertyVec(
					TiXmlElement* pXmlEmtFields,
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

	template <class T>
	void addProperty(DataItem* row, T value)
	{
		PropertyIns<T>* propIns = new PropertyIns<T>();
		propIns->m_propData = value;
		row->getPropVec().push_back((PropertyBase*)propIns);
	}

	void addProperty(DataItem* row, const char* value, size_t cfgLen)
	{
		PropertyStr* propIns = new PropertyStr();
		propIns->m_propData = value;
		propIns->m_cfgLen = cfgLen;
		row->getPropVec().push_back((PropertyBase*)propIns);
	}
};

ENDNAMESPACE(NSExcelExport)

#endif	// EXCELTBL_H