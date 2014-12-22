#ifndef EXCELTBLSORT_H
#define EXCELTBLSORT_H

#include "tinyxml.h"
#include <QtCore/QtCore>
#include "cstring"
#include "ExcelTbl.hxx"

class ExcelTblSort : public ExcelTbl
{
protected:

public:
	ExcelTblSort();
	~ExcelTblSort();

	virtual bool ExcelReaderCom(
					TiXmlElement* pXmlEmtFields,	//第一个字段
					const char* lpszExcelFile,		//Excel文件的全路径（包括文件名称）
					const char* lpszDB,
					const char* lpszTable,			
					const char* lpszOutputFile,		//tbl文件的全路径（包括文件名称）
					const char* lpszTableName,		//tbl文件名称本身
					const char* lpszsheetname,		//excel 中表单的名字   
					std::string& strStructDef,		// 最终结构体定义
					const char* provider = "Provider=Microsoft.Jet.OLEDB.4.0;",		// 数据引擎提供者  
					const char* extendedProperties = "Extended Properties=\'Excel 8.0;HDR=Yes;IMEX=1\';"		// 扩展属性    
				);
};

#endif	// ExcelTblSort