#ifndef EXCELTBL_H
#define EXCELTBL_H

//#include <Windows.h>
//#include <AfxWin.h>
//#include <afx.h>
#include "tinyxml.h"
#include <QtCore/QtCore>
#include <cstring>		// std::string
#include <cstdio>		// File
#include <tchar.h>		// _T
#include "TabelAttr.hxx"

class ExcelTbl
{
protected:
	QString m_xmlPath;	// 绝对路径的文件名字   
	QString m_tblPath;	// tbl 输出路径  
	QString m_strOutput;	// 字符串日志输出    
	TableAttr m_tableAttr;	// 定义的表的属性
	QMutex mutex;
public:
	ExcelTbl();
	~ExcelTbl();
	void setXmlPath(QString file);
	void setOutputPath(QString path);
	bool convExcel2Tbl();
	QString UTF82GBK(const QString &inStr);

	virtual bool ExcelReaderCom(
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
};

#endif	// EXCELTBL_H